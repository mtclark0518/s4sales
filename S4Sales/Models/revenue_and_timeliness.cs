using System;

namespace S4Sales.Models
{
    public class RevenueAndTimelinessReport
    {
        public DateTime date_start {get;set;}
        public DateTime date_end {get;set;}
        public string agency { get; set;}
        public int total_incidents {get;set;}
        public int total_timely {get;set;}
        public float percent_timely {get;set;}
        public int avg_days2_upload {get;set;}
        public int total_sales {get;set;}
        public int total_reimbursed {get;set;}
        public float percent_of_sales {get;set;}
    }
}