using System;
using S4Sales.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace S4Sales.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class ChartingController : Controller
    {
        private readonly DataRepository _repo;


        public ChartingController(DataRepository repo)
        {
            _repo = repo;
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
