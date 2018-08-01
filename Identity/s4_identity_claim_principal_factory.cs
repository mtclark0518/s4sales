using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace S4Sales.Identity
{
    public class S4UserClaimsPrincipalFactory<TUser, TRole> : IUserClaimsPrincipalFactory<S4Identity>
    where TUser : S4Identity 
    where TRole : S4IdentityRole
    {
        public S4UserClaimsPrincipalFactory(
            UserManager<S4Identity> user_manager,
            RoleManager<S4IdentityRole> role_manager, 
            IOptions<IdentityOptions> identity_options) 
        {
            if ( identity_options == null || identity_options.Value == null)
            {
                throw new ArgumentException(nameof(identity_options));
            }
            users = user_manager;
            roles = role_manager;
            identity = identity_options;
        }
        public UserManager<S4Identity> users { get; private set;}
        public RoleManager<S4IdentityRole> roles { get; private set;}
        public IOptions<IdentityOptions> identity { get; private set;}



        public virtual async Task<ClaimsPrincipal> CreateAsync(S4Identity user)
        {
            if(user == null){throw new ArgumentNullException(nameof(user));}
            ClaimsIdentity claim = new ClaimsIdentity();
            
            
            var _params = new {
                s4_id = await users.GetUserIdAsync(user),
                user_name = await users.GetUserNameAsync(user),
                s4_roles = await users.GetRolesAsync(user)
            };



            claim.AddClaim( new Claim( identity.Value.ClaimsIdentity.UserIdClaimType, user.s4_id));
            claim.AddClaim( new Claim( identity.Value.ClaimsIdentity.UserNameClaimType, user.normalized_user_name));
            
            foreach( var role_name in _params.s4_roles)
            {
                claim.AddClaim( new Claim( identity.Value.ClaimsIdentity.RoleClaimType, role_name));
            }
            
            return new ClaimsPrincipal(claim);
        }
    }


}


// configured using
// https://digitalmccullough.com/posts/aspnetcore-auth-system-demystified.html