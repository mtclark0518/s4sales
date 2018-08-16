
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using Npgsql;
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
        private CartStore _cart;
        public CommerceRepository( 
            IConfiguration config, 
            StripeService str, 
            CrashRepository cr, 
            CartStore cs)
        {
            _conn = config["ConnectionStrings:tc_dev"];
            _stripe = str;
            _crash = cr;
            _cart = cs;
        }

        // steps through transaction event
        public Task HandleTransaction(fkTransaction order)
        {
            // create a purchase
            Purchase po = FormatNewPurchase(order); 
            
            // create a time charge
            var charge =  _stripe.CreateCharge(order);
            
            // add charge results to purchase
            po.stripe_charge_token = charge.Id;
            po.charge_token_result = charge.Status;

            if( charge.Status == "succeeded")
            {
                // pull the item-list from order.cart_id
                var cart = _cart.GetContent(order.cart_id);
                
                // pull purchased crash event
                List<CrashEvent> purchased = new List<CrashEvent>();
                foreach(var c in cart)
                {
                    CrashEvent crash_report = _crash.FindByHsmvReportNumber(c.hsmv_report_number);
                    // Add purchased report four download list
                    purchased.Add(crash_report);
                    
                    // create the agency receipt / log
                    Reimbursement funds = FormatReimbursement(crash_report, order.cart_id);
                    if ( !LogReimbursement(funds) )
                    {
                        var error = "error writing reimbursement details";
                        throw new Exception(nameof(error));
                    };
                }
                // after creating agency reimbursements and fetching the reports
                //create purchase receipt / log
                if ( !LogTransaction(po) )
                {
                    var error = "error writing purchase details";
                    throw new Exception(nameof(error));
                };

                // return the purchased items
                return Task.FromResult(purchased);
            }
            // if charge failed we log the attempt and result
            else
            {
                if(!LogTransaction(po))
                {
                    var fail = "error writing purchase details";
                    throw new Exception(nameof(fail));
                }
                // return reason for charge failure 
                var failure = new StandardResponse()
                {
                    message =   charge.FailureMessage
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
            return funds;
        }

        private Purchase FormatNewPurchase(fkTransaction order)
        {
            return new Purchase()
            {
                cart_id = order.cart_id,
                purchase_amount = order.amount/100, //convert from cent to dollar amount
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
                agency = r.submitting_agency, 
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