
using Microsoft.Extensions.Configuration;
using S4Sales.Models;

namespace S4Sales.Logging
{
    public class DataRepository
    {
        private string _conn;
        public DataRepository(IConfiguration config)
        {
            _conn = config["ConnectionStrings:tc_dev"];
        }
    }
}
