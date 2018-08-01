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

    public interface ICrashRepository<CrashEvent>
    {
        CrashEvent FindByHsmvReportNumber(int hsmv);
        IEnumerable<CrashEvent> FindByVIN(string vin);
        IEnumerable<CrashEvent> FindByDateAndName(string participant, string date);
    }
    
    public class CrashRepository : ICrashRepository<CrashEvent>
    {
        private string _conn;
        public CrashRepository(IConfiguration config)
        {
            _conn = config["ConnectionStrings:tc_dev"];
        }
 
        public CrashEvent FindByHsmvReportNumber(int hsmv)
        {
            var queryText = $@"
                SELECT * FROM event_crash 
                WHERE hsmv_report_number = @hsmv_report_number";
            using (var conn = new NpgsqlConnection(_conn))
            {
                return conn.Query<CrashEvent>(queryText, new {hsmv_report_number = hsmv}).FirstOrDefault();
            }
        }
        public IEnumerable<CrashEvent> FindByVIN(string vin)
        {
            // sql query for getting report by vin
            var queryText = 
                $@" SELECT c.* FROM crash c
                INNER JOIN vehicle v 
                ON v.hsmv_report_number = c.hsmv_report_number
                WHERE v.vehicle_identification_number = @vehicle_identification_number";
            using (var conn = new NpgsqlConnection(_conn))
            {
                return conn.Query<CrashEvent>(queryText, new {vehicle_identification_number = vin});
            };
        }
        public IEnumerable<CrashEvent> FindByDateAndName(string participant, string crash_date)
        {
            var queryText =
                // return reports on a specific date with matching last name
                $@" SELECT c.* FROM crash c
                INNER JOIN participant p
                ON p.hsmv_report_number = c.hsmv_report_number
                WHERE p.participant_last_name = @participant
                AND CAST(c.crash_date_and_time as DATE) = @crash_date";

            using (var conn = new NpgsqlConnection(_conn))
            {
                CultureInfo provider = CultureInfo.InvariantCulture;
                return conn.Query<CrashEvent>(queryText, new {participant = participant, crash_date = DateTime.Parse(crash_date)});
            }
        }
    }
}