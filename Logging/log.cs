using System;
namespace S4Sales.Logging
{
    /// <Note>
    // Model definition for various site usage logs
    ///</Note>
    public class LogDownload
    {
        public DateTime date_and_time { get; set; }
        public string client_ip_address { get; set; }
        public string name_of_file {get;set;}
        public int number_of_reports { get; set; }
        public string user_id { get; set; }
        public string connection_to_crash { get; set; }
        public string search_guid { get; set; }
    }
    public class LogPurchase
    {
        public DateTime date_and_time { get; set; }
        public float purchase_amount { get; set; }
        public string client_ip_address { get; set; }
        public string authorization_number { get; set; }
        public string number_of_reports { get; set; }
        public string user_id { get; set; }
        public string connection_to_crash { get; set; }
        public string search_guid { get; set; }
        
    }
    public class LogSearch
    {
        public DateTime date_and_time { get; set; }
        public string client_ip_address { get; set; }
        public float type_of_search { get; set; }
        public string search_parameters { get; set; }
        public string number_of_reports { get; set; }
        public string user_id { get; set; }
        public string connection_to_crash { get; set; }
        public string search_guid { get; set; }
        
    }
}