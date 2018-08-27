
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using S4Sales.Log;
using S4Sales.Models;
using S4Sales.Services;
namespace S4Sales.Controllers
{
    [Route("api/[controller]")]
    public class CrashController : Controller
    {
        private readonly CrashRepository _repo;
        private readonly Logg _log;
        private readonly SessionUtility _sess;
        public CrashController(CrashRepository repo, Logg log, SessionUtility sess)
        {
            _repo = repo;
            _log = log;
            _sess = sess;
        }

        [HttpGet("report")]
        public IActionResult SearchByHSMVNumber()
        {
            QueryLog details = CreateLog(Request.Path);
            var hsmv_report_number = Request.Headers["hsmv"];
            details.parameters.Add(hsmv_report_number);

            var result = _repo.FindByHsmvReportNumber(int.Parse(hsmv_report_number));
            if(result.GetType() != typeof(CrashEvent))
            {
                details.succeeded = true;
                details.return_count = 1;
            }
            // _log.Query(details);
            return new OkObjectResult(result);
        }

        [HttpGet("vehicle")]
        public IActionResult SearchByVIN()
        {
            QueryLog details = CreateLog(Request.Path);
            var vin = Request.Headers["vin"];
            details.parameters.Add(vin);
            if (ModelState.IsValid)
            {
                var result = _repo.FindByVIN(vin);
                if(result.Count() != 0)
                {
                    details.return_count = result.Count();
                    details.succeeded = true;
                }
                // _log.Query(details);
                return new OkObjectResult(result);
            }
            // _log.Query(details);
            return new BadRequestObjectResult(ModelState);
        }

        [HttpGet("detailed")]
        public IActionResult SearchByDateAndName()
        {
            QueryLog details = CreateLog(Request.Path);

            var participant = Request.Headers["participant"];
            details.parameters.Add(participant);

            var date = Request.Headers["crash"];
            details.parameters.Add(date);

            var result = _repo.FindByDateAndName(participant, date);
            if(result.Count() != 0)
            {
                details.succeeded = true;
                details.return_count = result.Count();
            }
            // _log.Query(details);
            return new OkObjectResult(result);
        }

        private QueryLog CreateLog(string method)
        {
            return new QueryLog()
            {
                cart_id = _sess.GetSession("cart"),
                method = method,
                parameters = new List<string>(),
                succeeded = false,
                return_count = 0
            };

        }
    }

}