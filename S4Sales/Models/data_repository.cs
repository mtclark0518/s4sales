
using Dapper;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections;
using System.Threading.Tasks;

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
            var _query = $@"SELECT * FROM reimbursement";

            var _params = new {date = 2018};
            using (var conn = new NpgsqlConnection(_conn))
            {
                return conn.Query<Reimbursement>(_query, _params);
            }
        }
        
        // build string and execute query
        // return reimbursement list
        public async Task<IEnumerable<Reimbursement>> Reimbursements(string type, string value, string dkey, string dvalue)
        {
            // simplest revenue query is a count
            var _query = $@"SELECT * FROM reimbursement r";
            // search by county
            if(type == "County") 
            {
                _query += JoinCounty(value);
                _query += " AND EXTRACT( " + dkey.ToString().ToUpper() + " FROM r.reimbursement_date) = @date"; 
            }
            // search by reporting agency
            if(type == "Agency") 
            {
                 _query += " WHERE r.reporting_agency = " + value; 
                 _query += " AND EXTRACT( " + dkey.ToString().ToUpper() + " FROM r.reimbursement_date) = @date"; 
            }
            if(type == "State") 
            {
                _query += " WHERE EXTRACT( " + dkey.ToString().ToUpper() + " FROM r.reimbursement_date) = @date";

            }

            var _params = new 
            {
                date = int.Parse(dvalue) 
            };

            using(var conn = new NpgsqlConnection(_conn))
            {
                var result = await conn.QueryAsync<Reimbursement>(_query, _params);
                return result;            
            }
        }

        // county || agency -- all
        // date[year, month] || value
        // returns a count of the number of reports with provided params
        public async Task<IEnumerable<CrashEvent>> Reporting(string a, string b, string c, string d)
        {
            var _query = $@"SELECT * FROM event_crash c";

            if(a == "County") 
            {
                _query += " WHERE c.county_of_crash = " + b + " AND EXTRACT( " + c.ToString().ToUpper() + " FROM c.crash_date_and_time) = @date";
            }

            if(a == "Agency") 
            {
                _query += " WHERE c.reporting_agency = " + b + " AND EXTRACT( " + c.ToString().ToUpper() + " FROM c.crash_date_and_time) = @date";
            }

            if(a == "State") 
            {
                _query += " WHERE EXTRACT( " + c.ToString().ToUpper() + " FROM c.crash_date_and_time) = @date";
            }
            var _params = new 
            {
                date =  int.Parse(d)
            };

            using (var conn = new NpgsqlConnection(_conn))
            {
                var result = await conn.QueryAsync<CrashEvent>(_query,_params);
                return result;
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



// reports with time hsmv
// SELECT hsmv_report_number 
// FROM event_crash 
// WHERE crash_date_and_time + interval '10 days' < hsmv_entry_date

// most reports by county
// SELECT county_of_crash,
// COUNT(county_of_crash) as count
// FROM event_crash
// WHERE EXTRACT (YEAR FROM crash_date_and_time) = 2018
// GROUP BY county_of_crash
// ORDER BY count DESC;