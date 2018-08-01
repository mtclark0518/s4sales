using System;

namespace S4Sales.Models
{
    public enum Status
    {
        Unverified,
        Pending,
        Approved,
        Rejected
    }
    public enum RequestType
    {
        Admin,
        Member,
        Organization,
        Primary,
    }
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
        public S4Request (){}
    }
    
    public class S4Response
    {
        public string response_number {get;set;}
        public S4Request request {get;set;}
        public Status response_status {get;set;}
        public RequestType request_type {get;set;}
        public string message { get; set; }
        public string handled_by {get;set;}
        public DateTime handled_on { get; set;}
        public S4Response(S4Request req)
        {
            response_number = Guid.NewGuid().ToString();
            request = req;
            handled_on = DateTime.Now;
        }
    }
    
    public class ResponseBody
    {
        public string request_number {get; set;}
        public Status response_status { get; set;}
        public RequestType request_type {get; set;}
        public string message {get; set;}
    }

}