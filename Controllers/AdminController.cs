using Microsoft.AspNetCore.Mvc;
using S4Sales.Identity;
using System;
using System.Collections;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Collections.Generic;
using System.Linq;

namespace S4Sales.Controllers
{
    [Route("api/[controller]")]
    public class AdminController: Controller
    {
        private readonly S4IdRequestRepository _s4request;
        public AdminController(S4IdRequestRepository s4request)
        {
            _s4request = s4request;
        }

        [HttpGet("approval")]
        public Task<IEnumerable> AwaitingApproval()
        {
            return _s4request.AwaitingApproval();
        }

        [HttpPost("approve")]
        [AllowAnonymous]
        public Task<S4Response> ApprovalResponse([FromBody]ResponseBody details)
        {
            var response = _s4request.SubmitApprovalResponse(details);
            
            S4Response result = new S4Response()
            {
                message = response.ToString()
            };
            return Task.FromResult(result);
        }

        [HttpGet("verification")]
        public Task<IEnumerable> AwaitingVerification()
        {
            var organization = Request.Headers["organization"];
            return _s4request.AwaitingVerification(organization);
        }

        [HttpPost("verification")]
        public void Verification()
        {
            throw new NotImplementedException();
        }
    }
}