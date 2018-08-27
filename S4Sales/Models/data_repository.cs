
using Dapper;
using System;
using System.Collections.Generic;
using Microsoft.Extensions.Configuration;
using Npgsql;
using System.Collections;
using System.Threading.Tasks;
using System.Globalization;

namespace S4Sales.Models
{
    public class DataRepository
    {
        private string _conn;
        public DataRepository(IConfiguration config)
        {
            _conn = config["ConnectionStrings:tc_dev"];
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
        public async Task<IEnumerable> RnTReport(string date_start, string date_end)
        {

            var _query = $@"
            SELECT c.reporting_agency as agency,
                COUNT(c.reporting_agency) as total_incidents,
                COUNT(c.reporting_agency) 
                    FILTER(WHERE(
                            date_part('doy', c.hsmv_entry_date) - 
                            date_part('doy', c.crash_date_and_time) < 10)) as total_timely,
                ROUND(AVG(
                        date_part('day', age(c.hsmv_entry_date, c.crash_date_and_time)))) as avg_days2_upload,
                ROUND(((100 * COUNT(c.reporting_agency) 
                        FILTER(WHERE(
                                date_part('doy', c.hsmv_entry_date) - 
                                date_part('doy', c.crash_date_and_time) < 10))) 
                                / 
                                COUNT(c.reporting_agency)), 2) as percent_timely,
                COUNT(r.reporting_agency) as total_sales,
                SUM(r.reimbursement_amount) as total_reimbursed
                FROM event_crash c 
                    FULL JOIN reimbursement r
                        ON c.reporting_agency = r.reporting_agency 
                            AND c.hsmv_report_number = r.hsmv_report_number
 							WHERE c.crash_date_and_time::timestamp without time zone
 							BETWEEN @start AND @end
                                GROUP BY agency 
                                ORDER BY agency";
            
            var _params = new 
            {
                start = DateTime.Parse(date_start), 
                end = DateTime.Parse(date_end) 
            };
            using (var conn = new NpgsqlConnection(_conn))
            {
                var result = await conn.QueryAsync(_query, _params);
                return result;
            }
        }
    }
}
