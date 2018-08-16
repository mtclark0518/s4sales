using System;
using S4Sales.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

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

       [HttpGet("index")]
        public IEnumerable<Reimbursement> Index()
        {
            return _data.ReportIndex();
        }

        [HttpGet("reimbursement")]
        public Task<IEnumerable<Reimbursement>> Reimbursements()
        {
            var a = Request.Headers["chart_type"];
            var b = Request.Headers["ct_value"];
            var c = Request.Headers["date_type"];
            var d = Request.Headers["dt_value"];
            return _data.Reimbursements(a, b, c, d);
        }

        [HttpGet("reporting")]
        public Task<IEnumerable<CrashEvent>> Reporting()
        {
            var a = Request.Headers["chart_type"];
            var b = Request.Headers["ct_value"];
            var c = Request.Headers["date_type"];
            var d = Request.Headers["dt_value"];
            return _data.Reporting(a, b, c, d);        
        }
    }
}
