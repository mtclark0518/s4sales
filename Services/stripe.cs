using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using Stripe;
using S4Sales.Models;
using Microsoft.Extensions.Configuration;

namespace S4Sales.Services
{
    public class StripeService
    {
        private readonly string _stripe_key;
        public StripeService(IConfiguration config)
        {
            _stripe_key = config["Stripe:SecretKey"];
        }
        public StripeCharge CreateCharge(fkTransaction order)
        {
            StripeConfiguration.SetApiKey(_stripe_key);
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