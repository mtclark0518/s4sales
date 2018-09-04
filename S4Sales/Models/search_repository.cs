using System.Collections.Generic;
using System.Data;
using System.Linq;
using Microsoft.Extensions.Configuration;
using Dapper;
using Npgsql;
using S4Sales.Services;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Globalization;

namespace S4Sales.Models
{
    /// <Note>
    // Search for report methods by -- number -- vin -- name/date
    ///</Note>

    
    public class SearchRepository
    {
        private string _conn;
        public SearchRepository(IConfiguration config)
        {
            _conn = config["ConnectionStrings:tc_dev"];
        }
 
        public CrashEvent FindByHsmvReportNumber(int hsmv)
        {
            var _query = $@"
                SELECT * FROM event_crash 
                WHERE hsmv_report_number = @hsmv_report_number";
            using (var conn = new NpgsqlConnection(_conn))
            {
                return conn.Query<CrashEvent>(_query, new {hsmv_report_number = hsmv}).FirstOrDefault();
            }
        }
        public IEnumerable<CrashEvent> FindByVIN(string vin)
        {
            // sql query for getting report by vin
            var _query = 
                $@" SELECT c.* FROM event_crash c
                INNER JOIN event_vehicle v 
                ON v.hsmv_report_number = c.hsmv_report_number
                WHERE v.vehicle_identification_number = @vehicle_identification_number";
            using (var conn = new NpgsqlConnection(_conn))
            {
                return conn.Query<CrashEvent>(_query, new {vehicle_identification_number = vin});
            };
        }
        public IEnumerable<CrashEvent> FindByDateAndName(string participant, string crash_date)
        {
            var _query =
                // return reports on a specific date with matching last name
                $@" SELECT c.* FROM event_crash c
                INNER JOIN event_participant p
                ON p.hsmv_report_number = c.hsmv_report_number
                WHERE p.participant_last_name = @participant
                AND CAST(c.crash_date_and_time as DATE) = @crash_date";

            using (var conn = new NpgsqlConnection(_conn))
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                return conn.Query<CrashEvent>(_query, new 
                {
                    participant = participant, 
                    crash_date = DateTime.Parse(crash_date)
                });
            }
        }
    }
}