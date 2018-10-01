
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
using S4Sales.Services;

namespace S4Sales.Models
{
    public class CommerceRepository 
    {
        private string _conn;
        private StripeService _stripe;
        private SearchRepository _crash;
        private CartStore _cart;
        public CommerceRepository( 
            IConfiguration config, 
            StripeService str, 
            SearchRepository cr, 
            CartStore cs)
        {
            _conn = config["ConnectionStrings:tc_dev"];
            _stripe = str;
            _crash = cr;
            _cart = cs;
        }

        // steps through transaction event
        public Task HandleTransaction(reqTransaction order)
        {
            Purchase po = FormatNewPurchase(order); // create a purchase order
            var charge =  _stripe.CreateCharge(order); // create a time charge
            
            // add charge results to purchase
            po.stripe_charge_token = charge.Id;
            po.charge_token_result = charge.Status;

            if( charge.Status == "succeeded")
            {
                List<string> purchased = new List<string>();
                var cart = _cart.GetContent(order.cart_id); // pull the item-list from order
                // pull purchased crash events
                // Add purchased reports for download list
                // create the agency receipt / log
                foreach(var c in cart)
                {
                    CrashEvent crash_report = _crash.FindByHsmvReportNumber(c.hsmv_report_number);
                    Reimbursement funds = FormatReimbursement(crash_report, order.cart_id);
                    
                    var token = new DownloadToken(
                        po.cart_id,
                        crash_report.hsmv_report_number.ToString(),
                        po.stripe_charge_token).Mint();

                    purchased.Add(token);
                }
                // after creating agency reimbursements and fetching the reports
                //create purchase receipt / log
                if ( !LogTransaction(po) )
                {
                    var error = "error writing purchase details";
                    throw new Exception(nameof(error));
                };
                return Task.FromResult(purchased); // return download tokens
            }
            else // if charge fails
            {
                if(!LogTransaction(po))
                {
                    var fail = "error writing purchase details";
                    throw new Exception(nameof(fail));
                }
                // return reason for charge failure 
                var failure = new StandardResponse()
                {
                    message = charge.FailureMessage
                };
                return Task.FromResult(failure);
            }
        }

        #region Private methods
        private Reimbursement FormatReimbursement(CrashEvent crash_report, string cart_id)
        {
            var funds = new Reimbursement(crash_report, cart_id);
            // review order contents
            var accident = new 
            { 
                occured = crash_report.crash_date_and_time, 
                reported = crash_report.hsmv_entry_date
            };
            if(isWithinDateRange(accident.occured, accident.reported, 10))
            {
                // adjsut fund allocation
                funds.reimbursement_amount += 5;
                funds.timely = true;
            }
            if ( !LogReimbursement(funds) )
            {
                var error = "error writing reimbursement details";
                throw new Exception(nameof(error));
            };
            return funds;
        }

        private Purchase FormatNewPurchase(reqTransaction order)
        {
            return new Purchase()
            {
                cart_id = order.cart_id,
                initiated_at = DateTime.Now,
                stripe_src_token = order.token,
            };
        }

        // Date check ??? did b occur within c days of a
        private bool isWithinDateRange(DateTime a, DateTime b, int c)
        {
            return a.AddDays(c) > b;
        }

        private bool LogReimbursement(Reimbursement r)
        {
            var _query = $@"
            INSERT INTO reimbursement 
                (cart_id, hsmv_report_number, timely, 
                reporting_agency, reimbursement_amount, reimbursement_date) 
            VALUES
                (@cart, @hsmv, @timely, @agency, @amount, @date)";
            var _params = new 
            {
                cart = r.cart_id, 
                hsmv = r.hsmv_report_number, 
                timely = r.timely, 
                agency = r.reporting_agency, 
                amount = r.reimbursement_amount, 
                date = DateTime.Now
            };
            using (var conn = new NpgsqlConnection(_conn))
            {
                var result = conn.Execute(_query, _params);
                return result == 1;
            }
        }

        private bool LogTransaction(Purchase po)
        {
            var _query = $@"
                INSERT INTO purchase 
                    (cart_id, purchase_amount, initiated_at, completed_at, 
                    stripe_src_token, stripe_charge_token, charge_token_result) 
                VALUES
                    (@cart, @amount, @init, @complete, 
                    @src, @chrg, @result)";
            var _params = new 
            {
                cart = po.cart_id,
                amount = po.purchase_amount,
                init = po.initiated_at,
                complete = DateTime.Now,
                src = po.stripe_src_token,
                chrg = po.stripe_charge_token,
                result = po.charge_token_result
           };
            using (var conn = new NpgsqlConnection(_conn))
            {
                var result = conn.Execute(_query, _params);
                return result == 1;
            }
        }
        #endregion
    }
}