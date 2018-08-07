
using System;
using Microsoft.Extensions.Configuration;
using S4Sales.Models;

namespace S4Sales.Models
{
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

    public class DataRepository
    {
        private string _conn;
        public DataRepository(IConfiguration config)
        {
            _conn = config["ConnectionStrings:tc_dev"];
        }


    }
}
