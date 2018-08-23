
using Dapper;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections;
using System.Threading.Tasks;

namespace S4Sales.Models
{
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
        public async Task<IEnumerable<Reimbursement>> Reimbursements(
                string filter, string value, string date_filter, string dvalue)
        {
            // simplest revenue query is a count
            var _query = $@"SELECT * FROM reimbursement r";
            // search by county
            if(filter == "County") 
            {
                _query += $@" 
                    JOIN event_crash c 
                    ON c.hsmv_report_number = r.hsmv_report_number 
                    WHERE c.county_of_crash = UPPER(@value)
                    AND EXTRACT( " + date_filter.ToString().ToUpper() + 
                    " FROM r.reimbursement_date) = @date"; 
            }

            // search by reporting agency
            // checks abv and long name + std and upper casing
            // still not hitting everything
            if(filter == "Agency") 
            {
                _query += $@" WHERE r.reporting_agency IN (
                    (SELECT a.agency_name as long FROM dim_agency a WHERE a.agency_short_name = @value),
                    (SELECT a.agency_short_name as short FROM dim_agency a WHERE a.agency_short_name = @value),
                    (SELECT UPPER(a.agency_name) FROM dim_agency a WHERE a.agency_short_name = @value),
                    (SELECT UPPER(a.agency_short_name) FROM dim_agency a WHERE a.agency_short_name = @value)
                ) AND 
                EXTRACT ( " + date_filter.ToUpper() + " FROM c.crash_date_and_time) = @date";
            }

            if(filter == "State") 
            {
                _query += $@" 
                    WHERE EXTRACT( " + date_filter.ToString().ToUpper() + 
                    " FROM r.reimbursement_date) = @date";

            }

            var _params = new {date = int.Parse(dvalue), value = value};
            using(var conn = new NpgsqlConnection(_conn))
            {
                var result = await conn.QueryAsync<Reimbursement>(_query, _params);
                return result;            
            }
        }

        // county || agency -- all
        // date[year, month] || value
        // returns reports with provided params
        public async Task<IEnumerable<CrashEvent>> Reporting(
                string filter, string value, string date_filter, string df_value)
        {
            var _query = $@"SELECT * FROM event_crash c";

            if(filter == "County") 
            {
                _query += $@" 
                    WHERE c.county_of_crash = UPPER(@value) 
                    AND EXTRACT( " + date_filter.ToString().ToUpper() + 
                    " FROM c.crash_date_and_time) = @date";
            }

            if(filter == "Agency") 
            {
                _query += $@" WHERE c.reporting_agency IN (
                    (SELECT a.agency_name FROM dim_agency a WHERE a.agency_short_name = @value),
                    (SELECT a.agency_short_name FROM dim_agency a WHERE a.agency_short_name = @value),
                    (SELECT UPPER(a.agency_name) FROM dim_agency a WHERE a.agency_short_name = @value),
                    (SELECT UPPER(a.agency_short_name) FROM dim_agency a WHERE a.agency_short_name = @value)
                ) AND 
                EXTRACT ( " + date_filter.ToUpper() + " FROM c.crash_date_and_time) = @date";
            }

            if(filter == "State") 
            {
                _query += $@" 
                    WHERE EXTRACT( " + date_filter.ToUpper() + 
                    " FROM c.crash_date_and_time) = @date";
            }

            var _params = new { value = value, date = int.Parse(df_value) };

            using (var conn = new NpgsqlConnection(_conn))
            {
                var result = await conn.QueryAsync<CrashEvent>(_query,_params);
                return result;
            }
        }

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