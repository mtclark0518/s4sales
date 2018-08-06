
using Microsoft.Extensions.Configuration;
using S4Sales.Models;

namespace S4Sales.Logging
{
    /// <Note>
    // Holds logic for recording / logging site analytics
    ///</Note>
    public class S4Logger
    {
        private string _conn;
        public S4Logger(IConfiguration config)
        {
            _conn = config["ConnectionStrings:tc_dev"];
        }
        public void Reimbursement(Reimbursement r)
        {
        }
        public void Transaction(Purchase po)
        {
        }
    }
}
