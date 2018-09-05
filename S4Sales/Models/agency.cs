using System;

namespace S4Sales.Models
{    
    public class Agency
    {
        public string agency { get;}
        public string contact_email {get;set;}
        public string contact_first_name {get;set;}
        public string contact_last_name {get;set;}
        public string stripe_account_id {get;set;}
        public string s4_id {get;set;}
        public bool active { get; set; }
        public Agency(){} // default constructor
    }
    public class AgencyRequest
    {
        public string agency {get;set;}
        public string email {get;set;}
        public string password {get;}
        public string first_name {get;set;}
        public string last_name {get;set;}
        public AgencyRequest(){} // default constructor
    }
}