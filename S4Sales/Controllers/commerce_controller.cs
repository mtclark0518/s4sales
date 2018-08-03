using Microsoft.AspNetCore.Mvc;
using S4Sales.Models;
using S4Sales.Services;
using System;
using System.Threading.Tasks;

namespace S4Sales.Controllers
{
    [Route("api/[controller]")]
    public class CommerceController : Controller
    {
        private readonly CommerceRepository _ec_repo;
        private readonly StripeService _stripe;


        public CommerceController(CommerceRepository ec, StripeService str)
        {
            _ec_repo = ec;
            _stripe = str;
        }
        
        [HttpPost("create")]
        public Task CreateOrder([FromBody] fkTransaction order)
        {
            var charge =  _stripe.CreateCharge(order);
            return Task.FromResult(charge);
        }
    }
}