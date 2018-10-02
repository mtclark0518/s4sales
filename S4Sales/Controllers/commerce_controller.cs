using Microsoft.AspNetCore.Mvc;
using S4Sales.Models;
using System.Threading.Tasks;

namespace S4Sales.Controllers
{
    [Route("api/[controller]")]
    public class CommerceController : Controller
    {
        private readonly CommerceRepository _ec;
        public CommerceController(CommerceRepository ec)
        {
            _ec = ec;
        }

        [HttpPost("create")]
        public Task CreateOrder([FromBody] PurchaseRequest order)
        {
            var result = _ec.HandleTransaction(order);
            return Task.FromResult(result);
        }

        [HttpGet("download")]
        public Task Download()
        {
            string token = Request.Headers["download"]; 
            var result = _ec.HandleDownload(token);
            return Task.FromResult(result);
        }
    }
}