using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using S4Sales.Models;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using S4Sales.Identity;
using System.Collections.Generic;

namespace S4Sales.Controllers
{




    [Route("api/[controller]")]
    [Authorize]
    public class IdentityController: Controller
    {
        private readonly IOptions<IdentityOptions> _identity_options;
        private readonly UserManager<S4Identity> _user_manager;
        private readonly SignInManager<S4Identity> _signin_manager;
        // private readonly AccountRequestManager _s4request;
        public IdentityController(
            IOptions<IdentityOptions> identity_options,
            UserManager<S4Identity> user_manager, 
            SignInManager<S4Identity> signins
            // AccountRequestManager s4
            )
        {
            _identity_options = identity_options;
            _user_manager = user_manager;
            _signin_manager = signins;
            // _s4request = s4;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] Credentials attempted)
        {
            const string FailureMessage = "Username or password is incorrect.";
            S4Identity user = await _user_manager.FindByNameAsync(attempted.user_name);
            
            if (user == null) { return BadRequest(FailureMessage); }
            var signin_result = await _signin_manager.PasswordSignInAsync(
                user, attempted.password, false, lockoutOnFailure: false);
            
            if (signin_result != Microsoft.AspNetCore.Identity.SignInResult.Success)
            {
                // sign out in case they had logged in successfully prior to this failed attempt
                await _signin_manager.SignOutAsync();
                return BadRequest(FailureMessage);
            }
            return new ObjectResult(signin_result);
        }

        [HttpPost("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signin_manager.SignOutAsync();
            return new ObjectResult(new { success = true });
        }

       
        
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<StandardResponse> Register([FromBody] S4Request requested)
        {
            StandardResponse registration_attempt = await _s4request.InitiateS4IdRequest(requested);
            return registration_attempt;
        }



        [HttpGet("current")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCurrentUser()
        {
            S4Identity user = await _user_manager.GetUserAsync(User);
            if(user != null)
            {
                return new ObjectResult( new 
                {
                    user = true,
                    name = user.user_name,
                    roles = await _user_manager.GetRolesAsync(user)
                });
            }
            return new ObjectResult(new {user = false});
        }
    }
}