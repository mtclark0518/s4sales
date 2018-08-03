using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using S4Sales.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Dapper;
using System.Collections;
using System.Linq;
using S4Sales.Identity;
using S4Sales.Services;

namespace S4Sales.Models
{
    ///<Notes>
    // middleware store for account generation requests
    // assigns roles to the account for authorization
    ///</Notes>
    public class AccountRequestManager
    {
        private readonly UserManager<S4Identity> _user_manager;
        private readonly RoleManager<S4IdentityRole> _role_manager;
        private readonly string _conn;
        private readonly S4Emailer _email;
        public AccountRequestManager(IConfiguration config, UserManager<S4Identity> um,  S4Emailer email, RoleManager<S4IdentityRole> rm)
        {
            _conn = config["ConnectionStrings:tc_dev"];
            _user_manager = um;
            _role_manager = rm;
            _email = email;
        }

        // returns s4identity requests marked Pending
        // approvalis performed by application admin accounts
        public async Task<IEnumerable> AwaitingApproval() 
        {
            var _query = $@"SELECT * FROM s4_request WHERE request_status = 'Pending'";
            using(var conn = new NpgsqlConnection(_conn))
            {
                return await conn.QueryAsync(_query);
            }
        }

        // returns s4identity requests marked Unverified. 
        // verification is required by primary accounts
        // is registered under the organization
        public async Task<IEnumerable> AwaitingVerification(string organization)
        {
            var _query = $@"SELECT * FROM s4_request
                WHERE request_status = 'Unverified'
                AND organization = @organization";
            var _params = new { organization = organization };
            using(var conn = new NpgsqlConnection(_conn))
            {
                return await conn.QueryAsync(_query, _params);
            }
        }

        // submitted by client
        // creates s4id request
        // returns results to client
        public async Task<StandardResponse> InitiateS4IdRequest(S4Request requested)
        {
            StandardResponse s4res = new StandardResponse();
            S4Request initiate = new S4Request()
            {
                request_number = Guid.NewGuid().ToString(),
                request_type = requested.request_type,
                request_status = (requested.request_type == RequestType.Member) ? Status.Unverified : Status.Pending,
                email = requested.email,
                organization = requested.organization,
                user_name = requested.user_name,
                first_name = requested.first_name,
                last_name = requested.last_name,
                requested_on = DateTime.Now
            };

            var _query = $@"
                INSERT INTO s4_request (
                    request_number, request_type, request_status, 
                    email, organization, user_name, 
                    first_name, last_name, requested_on, initial_status)
                VALUES (
                    @request_number, @request_type::request_type, 
                    @request_status::status, @email, 
                    @organization, @user_name, @first_name, 
                    @last_name, @requested_on, @initial_status::status)";
            var _params = new 
            {
                request_number = initiate.request_number,
                request_type = initiate.request_type.ToString(),
                request_status = initiate.request_status.ToString(),
                email = initiate.email,
                organization = initiate.organization,
                user_name = initiate.user_name,
                first_name = initiate.first_name,
                last_name = initiate.last_name,
                requested_on = initiate.requested_on,
                initial_status = initiate.request_status.ToString()
            };

            using(var conn = new NpgsqlConnection(_conn))
            {
                conn.Open();
                conn.MapEnum<Status>("status");
                conn.MapEnum<RequestType>("request_type");

                var result = await conn.ExecuteAsync(_query, _params);
                s4res.message = result.ToString();
                return s4res;
            }
        }
        
        // walks through the response details. processes that response. 
        // calls the HandleResponse method at conclusion
        public async Task<StandardResponse> SubmitApprovalResponse(ResponseBody details)
        {
            StandardResponse Messages = new StandardResponse();
            S4Request s4 = GetRequestByNumber(details.request_number);
            // add in the details to the response
            S4Response config = new S4Response(s4)
            {
                request_type = details.request_type,
                response_status = details.response_status,
                message = details.message,
            };

            if(!InsertInitialResponse(config))
            {
                Messages.message += "Error inserting s4 response in db";
            }
            // if we are rejecting the request we can skip to the end
            if(config.response_status == Status.Rejected)
            {
                // update the s4request status
                config.request.request_status = Status.Rejected;
                if(!UpdateS4Request(config))
                {
                    Messages.message += "Error setting the request status to reject";
                }
                var result = await HandleResponse(Messages, config);
                return result;
            }
            else 
            {
                // if the request is to create a new organization do that first
                // TODO error handler preventing org request if org already exists
                if(config.request_type == RequestType.Organization)
                {

                    // returns truthy if insert executes
                    var new_organization = AddOrganization(config);
                    if(!new_organization)
                    {
                        Messages.message += " Error in adding the organization. ";
                        return await HandleResponse(Messages, config);
                    }
                    // update request to create the accompanying user
                    config.request_type = RequestType.Primary;
                    config.request.request_type = RequestType.Primary;
                }

                // create the user account
                var new_identity = await GenerateAccountAsync(config);

                
                if(new_identity == IdentityResult.Success)
                {
                    config.request.request_status = Status.Approved;
                    config.request.updated_on = DateTime.Now;
                    if(!UpdateS4Request(config))
                    {
                        Messages.message += " Error updating reqeust status to approved ";
                    }
                    var result = await HandleResponse(Messages, config);
                    return result;
                }
                Messages.message += " Error in account generation methods. Identity result failed. ";
                return Messages;
            }
        }

        #region private methods
        private bool AddOrganization(S4Response res)
        {
            // adds a new organization
            Organization org = new Organization(res.request.organization);
            var _query = 
                $@"INSERT INTO organization (organization_id, name, active, approved_date)
                VALUES (@orgid, @name, @active, @date)";

            var _params = new 
            {
                orgid = org.organization_id,
                name = org.name,
                active = org.active,
                date = org.approved_date
            };

            using(var conn = new NpgsqlConnection(_conn))
            {
                var result = conn.Execute(_query, _params);
                return result == 1;
            }
        }
        private async Task<IdentityResult> GenerateAccountAsync(S4Response s4)
        {
            S4LookupNormalizer normalizer = new S4LookupNormalizer();
            S4Identity user = new S4Identity(s4.request.user_name)
            {
                email = s4.request.email,
                normalized_user_name = normalizer.Normalize(s4.request.user_name),
                normalized_email = normalizer.Normalize(s4.request.email)
            };
            var creating = await _user_manager.CreateAsync(user, GenerateFakePassword());
            
            if(creating == IdentityResult.Success)
            {
                // add roles to account
                IList<string> roles = new List<string>();
                roles.Add("user");
                roles.Add(s4.request_type.ToString().ToLower());
                foreach(var role in roles)
                {
                    var role_assignment = await _user_manager.AddToRoleAsync(user, role);
                }

                if(GenerateProfile(s4, user))
                {
                    return IdentityResult.Success;
                }
            }
            return IdentityResult.Failed();
        }

        // adds the profile and corresponding organization / role
        private bool GenerateProfile(S4Response s4, S4Identity user)
        {
                AccountProfile profile = new AccountProfile(user.s4_id);
                var _query = $@"
                    INSERT INTO s4_profile (s4_profile_id, name_first, name_last, active, identity)
                    VALUES (@s4pid, @first, @last, @active, @s4id);
                    INSERT INTO organization_membership(organization, s4_profile, admin)
                    VALUES (@org, @s4pid, @admin)";

                var _params = new 
                {
                    s4pid = profile.s4_profile_id,
                    first = s4.request.first_name,
                    last = s4.request.last_name,
                    active = profile.active,
                    s4id = user.s4_id,
                    org = s4.request.organization,
                    admin = s4.request_type == RequestType.Primary
                };
                
                using(var conn = new NpgsqlConnection(_conn))
                {
                    // TODO add error handler
                    var result = conn.Execute(_query, _params);
                    return result == 2;
                }
        }

        // upon successful account creation the user receives an email to complete registration where they change this password
        private string GenerateFakePassword()
        {
            const string band_aid = "changmeow!";
            return band_aid;
        }
        
        private S4Request GetRequestByNumber(string request_number)
        {
            var _query = $@"
                SELECT * FROM s4_request
                WHERE request_number = @request_number";
            var _params = new { request_number = request_number };
            using(var conn = new NpgsqlConnection(_conn))
            {
                return conn.Query<S4Request>(_query, _params).FirstOrDefault();
            }

        }


        private bool InsertInitialResponse(S4Response s4)
        {
                var _query = $@" 
                    INSERT INTO s4_response 
                        (response_number, response_status, request_number, request_type, message, handled_on)
                    VALUES 
                        (@response_number, @response_status::status, @request_number, @request_type::request_type, @message, @handled_on)";
                var _params = new
                {
                    response_number =  s4.response_number, 
                    response_status =  s4.response_status.ToString(),
                    request_number =  s4.request.request_number, 
                    request_type =  s4.request_type.ToString(),
                    message = s4.message,
                    handled_on = DateTime.Now
                };
                using(var conn = new NpgsqlConnection(_conn))
                {
                    var insert = conn.Execute(_query, _params);
                    return insert == 1;
                }
        }
        
        private bool UpdateS4Request(S4Response s4)
        {
            // write the response to the db
            // update request
            // email response to requester
            var _query = $@"
                UPDATE s4_request
                SET 
                    request_type = @request_type::request_type,
                    request_status = @request_status::status, 
                    updated_on = @updated_on
                WHERE 
                    request_number = @request_number";
            var _params = new
            {
                request_type = s4.request.request_type.ToString(),
                request_status =  s4.request.request_status.ToString(),
                updated_on = DateTime.Now,
                request_number =  s4.request.request_number, 

            };
            using(var conn = new NpgsqlConnection(_conn))
            {
                conn.Open();
                conn.MapEnum<Status>("status");
                conn.MapEnum<RequestType>("request_type");
                var update = conn.Execute(_query, _params);
                return update == 1;
            }
        }
        private async Task<StandardResponse> HandleResponse(StandardResponse res, S4Response s4)
        {
            var handled = await _email.initSendEmail(res, s4);
            return handled;
        }

        #endregion
    }

}