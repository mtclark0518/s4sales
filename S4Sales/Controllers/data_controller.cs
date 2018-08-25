using System;
using S4Sales.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Collections;

namespace S4Sales.Controllers
{
    [Route("api/[controller]")]
    [AllowAnonymous]
    public class DataController : Controller
    {
        private readonly DataRepository _data;

        public DataController(DataRepository repo)
        {
            _data = repo;
        }

        [HttpGet("Reimbursements")]
        public Task<IEnumerable<Reimbursement>> Reimbursements()
        {
            var a = Request.Headers["filter"];
            var b = Request.Headers["filter_lookup"];
            var c = Request.Headers["date_filter"];
            var d = Request.Headers["date_lookup"];
            return _data.Reimbursements(a, b, c, d);
        }

        [HttpGet("Reporting")]
        public Task<IEnumerable<CrashEvent>> Reporting()
        {
            var a = Request.Headers["filter"];
            var b = Request.Headers["filter_lookup"];
            var c = Request.Headers["date_filter"];
            var d = Request.Headers["date_lookup"];
            return _data.Reporting(a, b, c, d);        
        }
        
        [HttpGet("RnT")]
        public Task<IEnumerable> RnTReport()
        {
            var a = Request.Headers["date_start"];
            var b = Request.Headers["date_end"];
            return _data.RnTReport(a, b);        
        }
    }
}
