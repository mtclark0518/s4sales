using System;

namespace S4Sales.Identity
{
    public class S4IdentityResponse
    {
        public string response_number {get;set;}
        public S4Request request {get;set;}
        public Status response_status {get;set;}
        public RequestType request_type {get;set;}
        public string message { get; set; }
        public string handled_by {get;set;}
        public DateTime handled_on { get; set;}
        public S4IdentityResponse(S4Request req)
        {
            response_number = Guid.NewGuid().ToString();
            request = req;
            handled_on = DateTime.Now;
        }
    }
}