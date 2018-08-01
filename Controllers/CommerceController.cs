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
        private readonly CartStore _cart_store;
        private readonly StripeService _stripe;


        public CommerceController(CommerceRepository ec, CartStore cs, StripeService str)
        {
            _ec_repo = ec;
            _cart_store = cs;
            _stripe = str;
        }

        [HttpPost("create")]
        public Task CreateOrder([FromBody] S4Transaction order)
        {
            var charge =  _stripe.CreateCharge(order);
            return Task.FromResult(charge);
        }
    }
}