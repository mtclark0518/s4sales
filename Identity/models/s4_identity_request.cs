using System;

namespace S4Sales.Identity
{

    public class S4Request
    {
        public string request_number { get;set;}
        public RequestType request_type {get;set;}
        public Status request_status {get;set;}
        public string email { get; set; }
        public string organization { get; set; }
        public string user_name { get; set; }
        public string first_name { get; set; }
        public string last_name { get; set; }
        public string verified_by {get;set;}
        public DateTime requested_on { get; internal set; }
        public Status initial_status {get;set;}
        public DateTime updated_on {get;set;}
        public S4Request ()
        {
        }
    }

}