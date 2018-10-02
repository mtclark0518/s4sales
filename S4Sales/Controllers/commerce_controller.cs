using Microsoft.AspNetCore.Mvc;
using S4Sales.Models;
using System.Threading.Tasks;

namespace S4Sales.Controllers
{
    [Route("api/[controller]")]
    public class CommerceController : Controller
    {
        private readonly CommerceRepository _ec_repo;


        public CommerceController(CommerceRepository ec)
        {
            _ec_repo = ec;
        }
        
        [HttpPost("create")]
        public Task CreateOrder([FromBody] reqTransaction order)
        {
            var result = _ec_repo.HandleTransaction(order);
            return Task.FromResult(result);
        }
        
        [HttpGet("download")]
        public void Download()
        {

            // TODO
            // var result = _ec_repo.HandleTransaction(order);
            // return Task.FromResult(result);
        }
    }
}