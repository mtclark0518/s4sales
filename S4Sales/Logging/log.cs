using System;
namespace S4Sales.Logging
{
    /// <Note>
    // Model definition for various site usage logs
    ///</Note>

    public enum EntryType 
    {
        Download,
        Cart,
        Search
    }
    public class SessionLog
    {
        public string session_id {get;set;}
        public DateTime init_dt { get; set; }
        public string client_ip { get; set; }
        public string cart_id { get; set; }
    }

    public class CartLog
    {
        public string cart_id { get; set; }
        public DateTime event_time { get; set; }
        public string action_name {get; set;}
        public string targetted_report {get;set;}
    }

    public class PurchaseLog
    {
        public string cart_id { get; set; }
        public float purchase_amount { get; set; }
        public DateTime initiated_at { get;set;}
        public DateTime completed_at { get;set;}
        public string stripe_src_token {get;set;}
        public string src_token_result {get;set;}
        public string stripe_charge_token {get;set;}
        public string charge_token_result {get;set;}
    }

    public class SearchLog
    {
        public string type_of_search { get; set; }
        public string search_parameters { get; set; }
        public bool succeeded { get; set; }
        public int number_of_reports { get; set; }
        public DateTime executed_at {get;set;}

    }
}