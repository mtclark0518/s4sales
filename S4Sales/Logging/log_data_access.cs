using Microsoft.Extensions.Configuration;

namespace S4Sales.Log
{
    public class Log
    {
        private readonly string _conn;
        public Log(IConfiguration config)
        {
            _conn = config["ConnectionStrings:tc_dev"];
        }
    }
}