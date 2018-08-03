
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using S4Sales.Services;

namespace S4Sales.Models
{
    /// <Note>
    // Holds methodology for processing purchases, recording sale data 
    ///</Note>

    public class CommerceRepository 
    {
        private string _conn;
        private StripeService _stripe;
        private CrashRepository _crash;
        public CommerceRepository(IConfiguration config, StripeService str, CrashRepository cr)
        {
            _conn = config["ConnectionStrings:tc_dev"];
            _stripe = str;
            _crash = cr;
        }

        public Task InitiateTransaction(fkTransaction order, Cart cart)
        {
            // create a 1 time charge
            var charge =  _stripe.CreateCharge(order);
            if( charge.Status == "succeeded")
            {
                // successfully charged the customer
                List<CrashEvent> purchased = new List<CrashEvent>();
                // pull the order for download
                foreach(var c in cart.cart_content)
                {
                    CrashEvent crash_report = _crash.FindByHsmvReportNumber(c.hsmv_report_number);
                    // review order contents
                    var report = new 
                    { 
                        agency = crash_report.reporting_agency, 
                        occured = crash_report.crash_date_and_time, 
                        reported = crash_report.hsmv_entry_date
                    };
                    
                    var funds = new Reimbursement(crash_report);
                    if(isWithinDateRange(report.occured, report.reported, 10))
                    {
                        // adjsut fund allocation
                        funds.reimbursement_amount += 5;
                        funds.incentivized = true;
                    }

                    // Log the transaction

                }
                // save final result of the transaction
                // how to send as filestream for download??
                return Task.FromResult(purchased);
            } 
            else
            {
                // on failure
                // charge.Outcome charge.FailureMessage
                // save failure to the database
                return null;
            }
        }



        #region Private methods
        // Date check ??? did b occur within c days of a
        private bool isWithinDateRange(DateTime a, DateTime b, int c)
        {
            return a.AddDays(c) > b;
        }
        // private bool setForDispersal(Reimbursement funds)
        // {
        //     return true;
        // }
        #endregion
    }
}