using System;
using Microsoft.AspNetCore.Mvc;

namespace S4Sales.Models
{
    public class Credentials
    {
        public string agency_name;
        public string password;
    }
    public class Email
    {
        public string recipient {get;set;}
        public string subject { get;set;}
        public string body{get;set;}
    }
    // using for testing purposes...basic response with message
    public class StandardResponse
    {
        public string message { get; set;}
        public StatusCodeResult code {get; internal set;}
        public string s4_id { get; internal set; }
    }
    public class CartItemRequest
    {
        public string cart_id {get; set;}
        public string hsmv {get; set;}
    }
}
