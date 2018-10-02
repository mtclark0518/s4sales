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
        public StripeCharge CreateCharge(reqTransaction order)
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

        public StripeTransfer CreateTransfer(Reimbursement reimbursement)
        {
            StripeConfiguration.SetApiKey(_stripe_key);
            var options = new StripeTransferCreateOptions
            {
                // TODO
            };

            var service = new StripeTransferService();
            StripeTransfer transfer = service.Create(options);
            return transfer;
        }
    }
}