using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using S4Sales.Services;

namespace S4Sales.Services
{
    // This class is used by the application to send email for account confirmation and password reset.
    // For more details see https://go.microsoft.com/fwlink/?LinkID=532713
    public class SendEmail : IEmailSender
    {
        
        public Task SendEmailAsync(string email, string subject, string message)
        {
            const string sender = "tylerclark.codes@gmail.com";
            const string recipient = "mtclark0518@gmail.com";
            const string pass = "ReturnOfTheJedi";
            var from = new MailAddress(sender, "Tyler's Test");
            var to = new MailAddress(recipient, "To Name");
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(sender, pass)
            };

            var outgoing = new MailMessage(from, to)
            {
                Subject = subject,
                Body = message
            };
            
            return smtp.SendMailAsync(outgoing);
        }


    }
    
}

