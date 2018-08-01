
using Microsoft.Extensions.Configuration;

namespace S4Sales.Models
{
    /// <Note>
    // Holds logic for recording / logging site analytics
    ///</Note>
    public class LogRepository
    {
        private string _conn;
        public LogRepository(IConfiguration Configuration)
        {
            _conn = Configuration.GetConnectionString("tc_dev");
        }
    }
}
