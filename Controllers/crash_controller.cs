
using Microsoft.AspNetCore.Mvc;
using S4Sales.Models;

namespace S4Sales.Controllers
{
    [Route("api/[controller]")]
    public class CrashController : Controller
    {
        private readonly CrashRepository _repo;
        private readonly CartStore _cart;
        public CrashController(CrashRepository repo, CartStore cart)
        {
            _repo = repo;
            _cart = cart;
        }

        [HttpGet("report")]
        public IActionResult SearchByHSMVNumber()
        {
            var hsmv_report_number = int.Parse(Request.Headers["hsmv"]);
            var result = _repo.FindByHsmvReportNumber(hsmv_report_number);
            return new OkObjectResult(result);
        }

        [HttpGet("vehicle")]
        public IActionResult SearchByVIN()
        {
            var vin = Request.Headers["vin"];
            if (ModelState.IsValid)
            {
                var result = _repo.FindByVIN(vin);
                return new OkObjectResult(result);
            }
            return new BadRequestObjectResult(ModelState);
        }

        [HttpGet("detailed")]
        public IActionResult SearchByDateAndName()
        {
            var participant = Request.Headers["participant"];
            var date = Request.Headers["crash"];
            var result = _repo.FindByDateAndName(participant, date);
            return new OkObjectResult(result);
        }
    }

}