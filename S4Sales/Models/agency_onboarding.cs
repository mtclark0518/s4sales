using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;
using Dapper;
using S4Sales.Identity;
using S4Sales.Services;
using System;

namespace S4Sales.Models
{
    public class AgencyOnboarding
    {
        private readonly UserManager<S4Identity> _user_manager;
        private readonly RoleManager<S4IdentityRole> _role_manager;
        private readonly string _conn;
        private readonly S4Emailer _email;
        public AgencyOnboarding(
            IConfiguration config, 
            UserManager<S4Identity> um,  
            S4Emailer email, 
            RoleManager<S4IdentityRole> rm)
        {
            _conn = config["ConnectionStrings:tc_dev"];
            _user_manager = um;
            _role_manager = rm;
            _email = email;
        }
        
        // Agency Registration Process
        public async Task<IdentityResult> ClaimAgencyAsync(AgencyRequest dhsmv)
        {
            if(AlreadyOnboard(dhsmv.agency)) // check that agency has an active account
            {
                // Account is active already send to login
                var msg = "This agency has already completed the onboarding process";
                IdentityError error = BuildError(msg);
                return IdentityResult.Failed(error);
            }


            if(IsInProgress(dhsmv.agency, dhsmv.email)) // check that agency has an active account
            {

                // agency has registered s4sales identity
                // but not clicked the onboarded with stripe button and NOW is attempting to re-register with
                // a different email address
                var msg = "This agency has already begun onboarding with a different email";
                IdentityError error = BuildError(msg);
                return IdentityResult.Failed(error);
            }

            S4LookupNormalizer normalizer = new S4LookupNormalizer();
            S4Identity agency = new S4Identity(dhsmv.agency)
            {
                email = dhsmv.email,
                normalized_user_name = normalizer.Normalize(dhsmv.agency),
                normalized_email = normalizer.Normalize(dhsmv.email)
            };

            IdentityResult creating = await _user_manager.CreateAsync(agency, dhsmv.password);
            if(creating != IdentityResult.Success)
            {

                var msg = "Error claiming agency";
                IdentityError error = BuildError(msg);
                return IdentityResult.Failed(error);
            }
            
            IList<string> roles = new List<string>(); // add roles to account
            roles.Add("agency");
            foreach(var role in roles) // in case accounts ever need multiple roles
            {
                await _user_manager.AddToRoleAsync(agency, role);
            }

            if(!AddAgencyDetails(dhsmv, agency))
            {
                var msg = "Error adding the agency details";
                IdentityError error = BuildError(msg);
                return IdentityResult.Failed(error);
            }

            return IdentityResult.Success;
        }
        public async Task<bool> ActivateAgency(OnboardingDetails agency)
        {
            var _query = $@"
                UPDATE 
                    agency_account
                SET
                    active = @active,
                    stripe_account_id = @stripe_account_id,
                    timestamp = @timestamp,
                WHERE 
                    agency = @agency";
            var _params = new 
            { 
                agency = agency.agency,
                stripe_account_id = agency.token,
                active = true,
                timestamp = DateTime.Now,
            };

            using (var conn = new NpgsqlConnection(_conn))
            {
                var result = await conn.ExecuteAsync(_query, _params);
                return result == 1;
            }
        }

        private bool AddAgencyDetails(AgencyRequest agency, S4Identity account)
        {
            var _query = $@"
                UPDATE agency_account
                SET
                    contact_email = @email,
                    contact_first_name = @first,
                    contact_last_name = @last,
                    s4_id = @s4_id,
                WHERE 
                    agency = @agency";
            var _params = new 
            {
                agency = agency.agency,
                email = agency.email,
                first = agency.first_name,
                last = agency.last_name,
                s4_id = account.s4_id,
            };
            using (var conn = new NpgsqlConnection(_conn))
            {
                var result = conn.Execute(_query, _params);
                return result == 1;
            }
        }
        private bool AlreadyOnboard(string agency)
        {
            var _query = $@"
                SELECT COUNT(*) FROM agency_account 
                WHERE agency = @agency
                AND active = @active";
            var _params = new 
            { 
                agency = agency,
                active = true
            };

            using (var conn = new NpgsqlConnection(_conn))
            {
                var result = conn.Execute(_query, _params);
                return result == 1;
            }
        }
        private bool IsInProgress(string agency, string email)
        {
            var _query = $@"
                SELECT COUNT(*) FROM agency_account 
                WHERE agency = @agency
                AND contact_email = @email
                AND active = @active";
            var _params = new 
            { 
                agency = agency,
                email = email,
                active = false
            };
            using (var conn = new NpgsqlConnection(_conn))
            {
                var result = conn.Execute(_query, _params);
                return result == 1;
            }
        }

        private IdentityError BuildError(string str)
        {
            IdentityError error = new IdentityError();
            error.Description = str;
            return error;
        }

    }
}