using System;
using S4Sales.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using S4Sales.Logging;

namespace S4Sales.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class LoggingController : Controller
    {
        private readonly LogRepository _repo;


        public LoggingController(LogRepository repo)
        {
            _repo = repo;
        }
    
        [HttpGet("date")]
        public IActionResult GetCurrentDate()
        {
            var response = new { date = DateTime.Now.Date };
            return new ObjectResult(response);
        }

        [HttpGet("time")]
        public IActionResult GetCurrentTime()
        {
            var response = new { time = DateTime.Now };
            return new ObjectResult(response);
        }

       [HttpGet("chart")]
        public IActionResult GetChartData()
        {
            throw new NotImplementedException();
            // TODO
            // var chart = Request.Headers["chart"];
            // return new ObjectResult(chart);
        }
    }
}
