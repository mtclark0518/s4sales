
using Microsoft.Extensions.Configuration;

namespace S4Sales.Logging
{
    /// <Note>
    // Holds logic for recording / logging site analytics
    ///</Note>
    public class DataRepository
    {
        private string _conn;
        public DataRepository(IConfiguration config)
        {
            _conn = config["ConnectionStrings:tc_dev"];
        }
    }
}
