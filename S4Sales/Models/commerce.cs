using System;

namespace S4Sales.Models
{
    /// <Note>
    // Model definitions purchase reimbursements
    ///</Note>
    public class Purchase
    {
        public string cart_id { get; set; }
        public float purchase_amount = 1425;
        public DateTime initiated_at { get;set;}
        public DateTime completed_at { get;set;}
        public string stripe_src_token {get;set;}
        public string stripe_charge_token {get;set;}
        public string charge_token_result {get;set;}
        
        // default constructor
        public Purchase(){}
    }
    public class Reimbursement 
    {
        public string cart_id {get; set;}
        public int hsmv_report_number { get; set; }
        public bool timely {get;set;}
        public string reporting_agency { get; set; }
        public float reimbursement_amount { get; set; }
        public DateTime reimbursement_date { get; set;}
        // default constructor
        public Reimbursement(){}
        
        // within purchase constructor
        public Reimbursement(CrashEvent e, string cart)
        {
            cart_id = cart;
            hsmv_report_number = e.hsmv_report_number;
            timely = false;
            reimbursement_amount = 0;
            reporting_agency = e.reporting_agency; 
        }
    }


    // FromBody client model
    public class fkTransaction
    {
        public string first_name {get;set;}        
        public string last_name {get;set;}        
        public int amount {get;set;}        
        public string token {get;set;}   
        public string cart_id {get;set;}     
    }
}