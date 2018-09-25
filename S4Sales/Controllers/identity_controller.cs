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
        private readonly AgencyManager _agency;
        public IdentityController(
            IOptions<IdentityOptions> identity_options,
            UserManager<S4Identity> user_manager, 
            SignInManager<S4Identity> signins,
            AgencyManager agency)
        {
            _identity_options = identity_options;
            _user_manager = user_manager;
            _signin_manager = signins;
            _agency = agency;
        }


        [HttpPut("activate")]
        [AllowAnonymous]

        public async Task<IEnumerable<Agency>> ActivateAgencyAccount([FromBody]OnboardingDetails details)
        {
            IEnumerable<Agency> agency = await _agency.ActivateAgency(details);
            return agency;
        }


        // checks for agency and returns the id
        [HttpGet("current")]
        [AllowAnonymous]
        public async Task<IActionResult> GetCurrentUser()
        {
            S4Identity user = await _user_manager.GetUserAsync(User);
            if(user == null)
            {
                return new ObjectResult(new {user = false});
            }
            Agency details = await _agency.GetAgencyDetails(user.user_name);
            var result = new 
            {
                user = true,
                name = user.user_name,
                roles = await _user_manager.GetRolesAsync(user),
                details = details
            };
            return new ObjectResult(result);
        }



        [HttpGet("details")]
        public async Task<Agency> GetUserDetails()
        {
            Agency details = await _agency.GetAgencyDetails(Request.Headers["agency"]);
            return details;
        }

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<IActionResult> Login([FromBody] Credentials attempted)
        {
            const string FailureMessage = "Username or password is incorrect.";

            int username = await _agency.GetAgencyId(attempted.agency_name);

            S4Identity user = await _user_manager.FindByNameAsync(username.ToString());
            
            if (user == null) { return BadRequest(FailureMessage); }
            var signin_result = await _signin_manager.PasswordSignInAsync(
                user, attempted.password, false, false);
            
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
        public async Task<IActionResult> Register([FromBody]AgencyRequest requested)
        {
            IdentityResult registration_attempt = await _agency.ClaimAgencyAsync(requested);
            if(registration_attempt != IdentityResult.Success)
            {
                return BadRequest(registration_attempt);             
            }

            S4Identity agency = await _user_manager.FindByEmailAsync(requested.email);
            if (agency == null) 
            { 
                return BadRequest("error locating new account"); 
            }

            var signin_result = await _signin_manager.PasswordSignInAsync(
                    agency, requested.password,
                    false, false);

            if(signin_result == Microsoft.AspNetCore.Identity.SignInResult.Failed)
            {
                return BadRequest(signin_result); 
            }

            return new OkObjectResult(registration_attempt);
        }
        
        [HttpPost("recover")]
        [AllowAnonymous]

        public async Task<IActionResult> RecoverAccount([FromBody]string email)
        {
            S4Identity agency = await _user_manager.FindByEmailAsync(email);
            if (agency == null) 
            { 
                return BadRequest("no agency associated with this email"); 
            }

            var recovery = await _agency.RecoverAccount(agency);
            if(recovery != "sent")
            {
                return BadRequest("recovery attempt failure");
            }
            return new OkObjectResult( new { message = recovery });
        }
    }
}