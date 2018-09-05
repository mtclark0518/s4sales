using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Npgsql;

namespace S4Sales.Identity
{
    public class S4IdentityStore: 
        IUserStore<S4Identity>,  
        IUserEmailStore<S4Identity>,
        IUserPasswordStore<S4Identity>,
        IUserRoleStore<S4Identity>
    {
        private readonly string _conn;
        public S4IdentityStore(IConfiguration config)
        {
            _conn = config["ConnectionStrings:tc_dev"];
        }

        #region IUserStore
            public async Task<IdentityResult> CreateAsync(S4Identity user, CancellationToken cancellationToken)
            {
                if (user == null) { throw new ArgumentNullException(nameof(user)); }
                cancellationToken.ThrowIfCancellationRequested();

                // creates new s4Identity
                    var _query = $@"
                        INSERT INTO s4_identity 
                        VALUES (@s4_id, @user_name, 
                            @email,@password_hash, 
                            @normalized_user_name,@normalized_email, 
                            @password_salt,@active,
                            @created_on )";
                      
                    var _params = new 
                    {
                        s4_id = user.s4_id,
                        user_name = user.user_name,
                        email = user.email,
                        password_hash = user.password_hash,
                        normalized_user_name = user.normalized_user_name,
                        normalized_email = user.normalized_email,
                        password_salt = user.password_salt,
                        active = user.active,
                        created_on = user.created_on
                    };

                using (var conn = new NpgsqlConnection(_conn))
                {
                    // int in_existence = await conn.QuerySingleOrDefaultAsync<int>(existing._query, existing._params);
                    // if(in_existence > 0){return IdentityResult.Failed(new[] { new IdentityError() { Description = "Username already exists" } });}
                    await conn.QuerySingleOrDefaultAsync<S4Identity>(_query, _params);
                    
                    // populate user role associations
                    if (user.s4roles.Count > 0)
                    {
                        foreach(var role in user.s4roles)
                        {
                            await SetS4RoleAssignmentAsync(user, role.normalized_role_name, cancellationToken);
                        }
                    }
                    return IdentityResult.Success;
                }
            }


            public Task<IdentityResult> DeleteAsync(S4Identity user, CancellationToken cancellationToken)
            {
                throw new NotImplementedException();
            }
            public void Dispose(){}

            public async Task<S4Identity> FindByIdAsync(string userId, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                    var _query = $@"SELECT * FROM s4_identity WHERE s4_id = @s4_id";
                    var _params = new { s4_id = userId };
                using (var conn = new NpgsqlConnection(_conn))
                {
                    S4Identity account = await conn.QuerySingleOrDefaultAsync<S4Identity>(_query, _params);
                    return account;
                }
            }
            public async Task<S4Identity> FindByNameAsync(string normal_name, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                    var _query = $@"SELECT * FROM s4_identity WHERE normalized_user_name = @normalized_user_name";
                    var _params = new { normalized_user_name = normal_name };
                using (var conn = new NpgsqlConnection(_conn))
                {
                    S4Identity account = await conn.QuerySingleOrDefaultAsync<S4Identity>(_query, _params);
                    return account;
                }
            }
            public Task<string> GetNormalizedUserNameAsync(S4Identity user, CancellationToken cancellationToken)
            {
                return Task.FromResult(user.normalized_user_name);
            }
            public Task<string> GetUserIdAsync(S4Identity user, CancellationToken cancellationToken)
            {
                return Task.FromResult(user.s4_id);
            }
            public Task<string> GetUserNameAsync(S4Identity user, CancellationToken cancellationToken)
            {
                return Task.FromResult(user.user_name);
            }
            public Task SetNormalizedUserNameAsync(S4Identity user, string normalized, CancellationToken cancellationToken)
            {
                user.normalized_user_name = normalized;
                return Task.FromResult(0);
            }
            public Task SetUserNameAsync(S4Identity user, string name, CancellationToken cancellationToken)
            {
                user.user_name = name;
                return Task.FromResult(0);
            }
            public async Task<IdentityResult> UpdateAsync(S4Identity user, CancellationToken cancellationToken)
            {
                if (user == null) { throw new ArgumentNullException(nameof(user));}

                var _query = $@"
                    UPDATE s4_identity 
                    SET 
                        user_name = @user, 
                        email = @email, 
                        normalized_user_name = @normalized_name, 
                        normalized_email = @normalized_email 
                    WHERE 
                        s4_id = @s4_id";
                var _params = new 
                { 
                    user = user.user_name,
                    email = user.email,
                    normalized_name = user.normalized_user_name,
                    normalized_email = user.normalized_email,
                    s4_id = user.s4_id
                };

                using (var conn = new NpgsqlConnection(_conn))
                {
                    await conn.ExecuteAsync(_query, _params);


                    foreach(var role in user.s4roles)
                    {   
                        IdentityResult update = await SetS4RoleAssignmentAsync(user, role.normalized_role_name, cancellationToken );
                        if(update != Microsoft.AspNetCore.Identity.IdentityResult.Success){throw new ApplicationException(nameof(role.normalized_role_name));}
                    }
                    return IdentityResult.Success;
                }
            }

        #endregion

        #region IUserRoleStore

            public Task AddToRoleAsync(S4Identity user, string role, CancellationToken cancellationToken)
            {
                user.AddRole(new S4IdentityRole(role));
                Task<IdentityResult> update = SetS4RoleAssignmentAsync(user, role, cancellationToken);
                return Task.FromResult(update);
            }

            async Task<IdentityResult> SetS4RoleAssignmentAsync(S4Identity user, string role, CancellationToken ct)
            {
                IList<S4Identity> AssignedRole = new List<S4Identity>();
                AssignedRole = await GetUsersInRoleAsync(role, ct);
                if (AssignedRole.Contains(user))
                {
                    return IdentityResult.Success;
                }
                var _query = $@"INSERT INTO s4_role_assignment (s4_role_name, user_name)
                    VALUES(@s4_role_name, @user_name)";

                var _params = new {s4_role_name = role, user_name = user.user_name};
                
                using(var conn = new NpgsqlConnection(_conn))
                {
                    var result = await conn.ExecuteAsync(_query, _params);
                    return IdentityResult.Success;
                }
            }

            public Task<IList<string>> GetRolesAsync(S4Identity user, CancellationToken cancellationToken)
            {
                IList<string> roles = user.s4roles.Select(r => r.s4_role_name).ToList();
                return Task.FromResult(roles);
            }

            // holds the query for GetRolesAsync 
            public async Task<IList<string>> UserRoles(S4Identity user)
            {
                IList<string> RoleNames = new List<string>();
                
                var _query = $@"
                    SELECT s4_role_name FROM s4_role_assignment 
                    WHERE user_name = @user_name";
                
                var _params = new {user_name = user.user_name};

                using (var conn = new NpgsqlConnection(_conn))
                {
                    return RoleNames = await conn.QuerySingleOrDefaultAsync(_query, _params);
                }
            }

            public async Task<IList<S4Identity>> GetUsersInRoleAsync(string name, CancellationToken cancellationToken)
            {
                var check_role = new 
                {
                    _query = $@"
                        SELECT user_name FROM s4_role_assignment
                        WHERE s4_role_name = @role_name",
                    _params = new {role_name = name},
                };
                using (var conn = new NpgsqlConnection(_conn))
                {
                    var user_names = await conn.QueryAsync<string>(check_role._query, check_role._params);
                    IList<S4Identity> users_in_role = user_names
                        .Select(async normalized_user_name => await FindByNameAsync(normalized_user_name, cancellationToken))
                        .Select( task => task.Result).ToList();

                        return users_in_role;
                }
            }

            public Task<bool> IsInRoleAsync(S4Identity user, string name, CancellationToken cancellationToken)
            {
                bool in_role = user.s4roles.Any(r => r.normalized_role_name.Equals(name));
                return Task.FromResult(in_role);
            }

                public Task RemoveFromRoleAsync(S4Identity user, string roleName, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                user.RemoveRole(new S4IdentityRole(roleName));
                return Task.FromResult(0);
            }

        #endregion

        #region IUserPasswordStore
            public Task<string> GetPasswordHashAsync(S4Identity user, CancellationToken cancellationToken)
            {
                return Task.FromResult(user.password_hash);
            }
            public Task<bool> HasPasswordAsync(S4Identity user, CancellationToken cancellationToken)
            {
                return Task.FromResult( user.password_hash != null );
            }
            public Task SetPasswordHashAsync(S4Identity user, string hashed, CancellationToken cancellationToken)
            {
                user.password_hash = hashed;
                return Task.FromResult(0);
            }
        #endregion

        #region IUserEmailStore
            public async Task<S4Identity> FindByEmailAsync(string normal_email, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var find_user = new 
                {
                    _query = $@"SELECT * FROM s4_identity s WHERE s.normalized_email = @normalized_email",
                    _params = new { normalized_email = normal_email }
                };
                using (var conn = new NpgsqlConnection(_conn))
                {
                    S4Identity s4 = await conn.QuerySingleOrDefaultAsync<S4Identity>(find_user._query, find_user._params);
                    return s4;
                }
            }
            public Task<string> GetEmailAsync(S4Identity user, CancellationToken cancellationToken)
            {
                return Task.FromResult(user.email);
            }
            public Task<bool> GetEmailConfirmedAsync(S4Identity user, CancellationToken cancellationToken)
            {
                return Task.FromResult(true);
            }
            public Task<string> GetNormalizedEmailAsync(S4Identity user, CancellationToken cancellationToken)
            {
                return Task.FromResult(user.normalized_email);
            }
            public Task SetEmailAsync(S4Identity user, string email, CancellationToken cancellationToken)
            {
                user.email = email;
                return Task.FromResult(0);
            }
            public Task SetEmailConfirmedAsync(S4Identity user, bool confirmed, CancellationToken cancellationToken)
            {
                return Task.FromResult(0);
            }
            public Task SetNormalizedEmailAsync(S4Identity user, string normal_email, CancellationToken cancellationToken)
            {
                user.normalized_email = normal_email;
                return Task.FromResult(0);
            }
        #endregion
    }
}
