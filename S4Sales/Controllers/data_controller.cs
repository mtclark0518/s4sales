using System;
using S4Sales.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

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

       [HttpGet("chart")]
        public IEnumerable<Reimbursement> GetChartIndex()
        {
            return _data.ReportIndex();
        }
    }
}
