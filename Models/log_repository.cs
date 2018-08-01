
using Microsoft.Extensions.Configuration;

namespace S4Sales.Models
{
    /// <Note>
    // Holds logic for recording / logging site analytics
    ///</Note>
    public class LogRepository
    {
        private string _conn;
        public LogRepository(IConfiguration config)
        {
            _conn = config["ConnectionStrings:tc_dev"];
        }
    }
}
