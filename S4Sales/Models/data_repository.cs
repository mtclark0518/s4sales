
using Dapper;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Npgsql;

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

        // total incidents
        public IEnumerable<Reimbursement> ReportIndex()
        {
            var _query = $@"SELECT * FROM reimbursement WHERE EXTRACT(YEAR FROM r.reimbursement_date) = @date";

            var _params = new {date = 2018};
            using (var conn = new NpgsqlConnection(_conn))
            {
                return conn.Query<Reimbursement>(_query, _params);
            }
        }
        
        // build string and execute query
        // return reimbursement list
        public IEnumerable<Reimbursement> Reimbursements(string type, string value, string dkey, string dvalue)
        {
            // simplest revenue query is a count
            var _query = $@"SELECT COUNT(*) FROM reimbursements r"
            // search by county
            if(type == 'county'){_query += JoinCounty(value);}
            // search by reporting agency
            if(type == 'agency'){_query += " WHERE r.reporting_agency = " + value;}
            // extract date filter
            _query += " AND EXTRACT(@dkey FROM r.reimbursement_date) = @dvalue";

            var _params = new { dkey = dkey, dkvalue = dvalue }
            using(var conn = new NpgsqlConnection(_conn))
            {
                return conn.Query<Reimbursement>(_query, _params);
            }
        }

        // county || agency -- all
        // date[year, month] || value
        // returns a count of the number of reports with provided params
        public int Reporting(string a, string b, string c, string d)
        {
            var _query = $@"SELECT COUNT(*) FROM crash_event c"

            if(a == "county"){_query += " WHERE c.county_of_crash = " + b;}
            if(a == "agency"){_query += " WHERE c.reporting_agency = " + b;}
            if(a == null){_query += " WHERE c.reporting_agency = *";}
            _query += " AND EXTRACT(@c FROM crash_date_and_time) = @d";

            var _params = new {c = c,d = d};
            using (var conn = new NpgsqlConnection(_conn))
            {
                return conn.Execute(_query, _params);
            }
        }

        private string JoinCounty(string county)
        {
            return $@" JOIN event_crash c 
            ON c.hsmv_report_number = r.hsmv_report_number 
            WHERE c.county_of_crash = " + county;
        }
        // timliness by month
    }
}




// SELECT hsmv_report_number, 
// crash_date_and_time,
// crash_date_and_time + interval '10 days' AS timely_date
// FROM event_crash


// SELECT county_of_crash,
// COUNT(county_of_crash) as count
// FROM event_crash
// WHERE EXTRACT (YEAR FROM crash_date_and_time) = 2018
// GROUP BY county_of_crash
// ORDER BY count DESC;