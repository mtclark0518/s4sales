using System;
using Microsoft.AspNetCore.Mvc;

namespace S4Sales.Models
{
    public class Credentials
    {
        public string user_name;
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

    public class Overview 
    {
        public int total_reports { get; set;}
        public int total_revenue {get;set;}
        public int total_reimbursed {get;set;}
        public DateTime as_of_date {get;set;}
    }

    public class Timeliness
    {
        public int total_incidents {get;set;}
        public int total_timely {get;set;}
        public float percent_timely {get;set;}
        public int avg_days2_upload {get;set;}
        public int total_sales {get;set;}
        public int total_reimbursed {get;set;}
        public float percent_sales {get;set;}
        public TimeSpan report_span {get;set;}
    }
}
