using System;

namespace S4Sales.Models
{
    /// <Note>
    // Model definitions purchase reimbursements
    ///</Note>
    public class Reimbursement 
    {
        public int hsmv_report_number { get; set; }
        public bool incentivized {get;set;}
        public string submitting_agency { get; set; }
        public float reimbursement_amount { get; set; }
        public string reimbursing_agency { get; set; }
        public DateTime reimbursement_date { get; set;}
        public Reimbursement(CrashEvent e)
        {
            hsmv_report_number = e.hsmv_report_number;
            incentivized = false;
            reimbursement_amount = 0;
            reimbursing_agency = e.reporting_agency; 
            reimbursement_date = DateTime.Now;

        }
    }
    public class fkTransaction
    {
        public string first_name {get;set;}        
        public string last_name {get;set;}        
        public int amount {get;set;}        
        public string token {get;set;}        
    }
}