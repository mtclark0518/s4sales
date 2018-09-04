using System;

namespace S4Sales.Models
{    
    public class Agency
    {
        public int agency_id { get; set;}
        public string agency_name { get; set;}
        public string agency_short_name {get;set;}
        public string first_name {get;set;}
        public string last_name {get;set;}
        public bool active { get; set; }
        public string stripe_account {get;set;}
        public DateTime timestamp {get; set;}
        public Agency(){} // default constructor
    }
}