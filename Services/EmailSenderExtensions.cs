using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using S4Sales.Services;

namespace S4Sales.Services
{
    public static class EmailSenderExtensions
    {
        public static Task SendEmailConfirmationAsync(this IEmailSender emailSender, string email, string callbackUrl)
        {
            return emailSender.SendEmailAsync(email, "Confirm your account", 
               "Please confirm your account by clicking this link: <a href=\"" + callbackUrl + "\">link</a>");
        }
    }
}
