using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using S4Sales.Models;

namespace S4Sales.Controllers
{
    [Route("api/[controller]")]
    public class AdminController: Controller
    {
        private readonly AccountRequestManager _s4request;
        public AdminController(AccountRequestManager s4request)
        {
            _s4request = s4request;
        }

        // retrieves account requests
        [HttpGet("approval")]
        public Task<IEnumerable> AwaitingApproval()
        {
            return _s4request.AwaitingApproval();
        }
        
        // submits response to account requests
        [HttpPost("approve")]
        [AllowAnonymous]
        public Task<StandardResponse> ApprovalResponse([FromBody]ResponseBody details)
        {
            var response = _s4request.SubmitApprovalResponse(details);
            
            StandardResponse result = new StandardResponse()
            {
                message = response.ToString()
            };
            return Task.FromResult(result);
        }


        // may or may not be required 
        [HttpGet("verification")]
        public Task<IEnumerable> AwaitingVerification()
        {
            var organization = Request.Headers["organization"];
            return _s4request.AwaitingVerification(organization);
        }
        // may or may not be required 
        [HttpPost("verification")]
        public void Verification()
        {
            throw new NotImplementedException();
        }
    }
}