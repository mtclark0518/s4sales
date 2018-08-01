
using System;
using Microsoft.AspNetCore.Mvc;

namespace S4Sales.Identity
{
#region Misc utility classes
    public class Email
    {
        public string recipient {get;set;}
        public string subject { get;set;}
        public string body{get;set;}
    }
    // using for testing purposes...basic response with message
    public class S4Response
    {
        public string message { get; set;}
        public StatusCodeResult code {get; internal set;}
        public string s4_id { get; internal set; }
    }
    public class UserRole
    {
        public string s4_role_name {get; set;}
        public string user_name {get; set;}
    }

#endregion

#region  FromBody
    public class ResponseBody
    {
        public string request_number {get; set;}
        public Status response_status { get; set;}
        public RequestType request_type {get; set;}
        public string message {get; set;}
    }
#endregion

#region  Enums
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
#endregion

}