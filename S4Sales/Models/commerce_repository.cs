
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

        public Task HandleTransaction(reqTransaction order)
        {
            Purchase po = FormatNewPurchase(order); // creates a purchase object
            var charge =  _stripe.CreateCharge(order); // create Stripe charge
            
            po.stripe_charge_token = charge.Id; // add charge token to purchase
            po.charge_token_result = charge.Status; // add charge outcome to purchase

            if( charge.Status == "succeeded") 
            {
                List<string> purchased = new List<string>(); // empty list will hold download tokens for purchase
                var cart = _cart.GetContent(order.cart_id); // pull the item-list from order
                foreach(var c in cart)
                {
                    string hsmv_token = String.Empty; // holds the formated download token
                    CrashEvent hsmv_report = _crash.FindByHsmvReportNumber(c.hsmv_report_number); // pull report

                    // if(hsmv_report.timely) // TODO if report is timely
                    // {
                    //     Reimbursement funds = FormatReimbursement(hsmv_report, po.cart_id); // create Stripe transfer
                    // }

                    string token = new DownloadToken( 
                        po.cart_id,
                        hsmv_report.hsmv_report_number.ToString(),
                        po.stripe_charge_token).Mint();

                    hsmv_token += hsmv_report + "." + token; // concatenate report number to the token
                    purchased.Add(hsmv_token);
                }
                if (!LogTransaction(po)) // saves our purchase object
                {
                    var error = "error writing purchase details";
                    throw new Exception(nameof(error));
                };
                return Task.FromResult(purchased); // return download tokens
            }
            else // path is hit if the Stripe charge fails (ie insufficient funds, incorrect card number)
            {
                if(!LogTransaction(po)) // save the attempted purchase
                {
                    var fail = "error writing purchase details";
                    throw new Exception(nameof(fail));
                }

                var failure = new StandardResponse() // return the failure message to the client
                {
                    message = charge.FailureMessage
                };
                return Task.FromResult(failure);
            }
        }


        ///<Note> TODO
        // create a stripe transfer to reimburse the reporting agency
        // create a reimbursement object (holding transfer token) to save for s4 records
        ///</Note>
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

        ///<Note>
        // returns purchase object
        // log purchase called by handleTransaction
        ///</Note>
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


        ///<Note>
        // saves Reimbursement to databsase
        ///</Note>
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

        ///<Note>
        // saves the purchase object to the databbase
        ///</Note>
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
    }
}