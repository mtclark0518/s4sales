using Microsoft.AspNetCore.Mvc;

namespace S4Sales.Log
{
    [Route("api/[controller]")]
    public class LogController: Controller
    {
        private readonly Log _log;
        public LogController(Log log)
        {
            _log = log;
        }
    }
}