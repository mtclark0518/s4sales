using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using S4Sales.Models;
namespace S4Sales.Services
{
    public class StripeService
    {
        public StripeCharge CreateCharge(S4Transaction order)
        {
            StripeConfiguration.SetApiKey("sk_test_Sj63bPC7k61WRHSJL5oRfsvW");
            var options = new StripeChargeCreateOptions
            {
                Amount = order.amount,
                Capture = true,
                Currency = "usd",
                SourceTokenOrExistingSourceId = order.token
            };

            var service = new StripeChargeService();
            StripeCharge charge = service.Create(options);
            return charge;
        }
        public StripePayout CreatePayout(dynamic opt)
        {
            var options = new StripePayoutCreateOptions()
            {
                Amount = opt.amount,
                Currency = "usd",
                Destination = opt.payment_to,
            };
            var payout = new StripePayoutService();
            return payout.Create(options);
        } 
    }
}