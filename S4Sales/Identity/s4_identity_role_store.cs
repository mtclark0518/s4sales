using System;
using System.Threading;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Npgsql;
namespace S4Sales.Identity
{
    public class S4RoleStore : IRoleStore<S4IdentityRole>
    {
        private string _conn;
        public S4RoleStore(IConfiguration config)
        {
            _conn = config["ConnectionStrings:tc_dev"];
        }


        #region role_store
            public async Task<IdentityResult> CreateAsync(S4IdentityRole role, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var pqsl_text = $@"
                    INSERT INTO s4_role 
                    VALUES(@s4_role_name, @normalized_role_name)";

                using(var conn = new NpgsqlConnection(_conn))
                {
                    await conn.ExecuteAsync(pqsl_text, new 
                    {
                        s4_role_name = role.s4_role_name,
                        normalized_role_name = role.s4_role_name
                    });

                    return IdentityResult.Success;
                }
            }
            public async Task<IdentityResult> DeleteAsync(S4IdentityRole role, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var _query = $@"
                    DELETE FROM s4_role s
                    WHERE s.s4_role_name = @role_name,
                    DELETE FROM s4_role_assignment ra
                    WHERE ra.s4_role_name = @role_name";
                
                var _params = new { role_name = role.s4_role_name };

                using (var conn = new NpgsqlConnection(_conn))
                {
                    await conn.QueryMultipleAsync(_query, _params);
                }
                return IdentityResult.Success;
            }
            public async Task<S4IdentityRole> FindByNameAsync(string normal_role_name, CancellationToken cancellationToken)
            {
                cancellationToken.ThrowIfCancellationRequested();
                var _query = $@"
                    SELECT * FROM s4_role
                    WHERE normalized_role_name = @role_name";
                var _params = new { role_name = normal_role_name};

                using(var conn = new NpgsqlConnection(_conn))
                {
                    return await conn.QuerySingleOrDefaultAsync<S4IdentityRole>(_query, _params);
                }
            }
            public Task<string> GetNormalizedRoleNameAsync(S4IdentityRole role, CancellationToken cancellationToken)
            {
                return Task.FromResult(role.normalized_role_name);
            }
            public Task<string> GetRoleNameAsync(S4IdentityRole role, CancellationToken cancellationToken)
            {
                return Task.FromResult(role.s4_role_name);
            }
            public Task SetNormalizedRoleNameAsync(S4IdentityRole role, string normal_name, CancellationToken cancellationToken)
            {
                role.normalized_role_name = normal_name;
                return Task.FromResult(0);
            }
            public Task SetRoleNameAsync(S4IdentityRole role, string name, CancellationToken cancellationToken)
            {
                role.s4_role_name = name;
                return Task.FromResult(0);
            }
            public void Dispose(){}

        #endregion
        


        #region unimplemented
            public Task<S4IdentityRole> FindByIdAsync(string roleId, CancellationToken cancellationToken)
            {
                throw new NotSupportedException("role_id are not supported");
            }
            public Task<string> GetRoleIdAsync(S4IdentityRole role, CancellationToken cancellationToken)
            {
                throw new NotSupportedException("role_id are not supported");
            }
            public Task<IdentityResult> UpdateAsync(S4IdentityRole role, CancellationToken cancellationToken)
            {
                throw new NotSupportedException("role updates are a no no");
            }
        #endregion
    }
}


// var existence = await conn.QuerySingleAsync<int>(already_exists_text, new 
                    // { 
                    //     role_name = role.normalized_role_name,
                    // });

                    // if (existence > 0)
                    // {
                    //     return IdentityResult.Failed(new[] { new IdentityError() { Description = "Role already exists" } }); 
                    // }