using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Npgsql;
using S4Sales.Identity;
using S4Sales.Models;
namespace S4Sales.Services
{
    public class S4Emailer
    {
        private string _conn;
        private dynamic _emailOptions;
        private SmtpClient _smtp;
        private string _email;
        private string _pass;
        private string _host;
        private int _port;
        private bool _ssl;

        public S4Emailer(IConfiguration config)
        {
            _conn = config["ConnectionStrings:tc_dev"];
            _emailOptions = config.GetSection("EmailOptions");
            _email = config["EmailOptions:Sender"];
            _pass = config["EmailOptions:Password"];
            _host = _emailOptions["SmtpServer"];
            _port = int.Parse(_emailOptions["SmtpPort"]);
            _ssl = Convert.ToBoolean(_emailOptions["EnableSsl"]);
            _smtp = new SmtpClient
            {
                Host = _host,
                Port = _port,
                EnableSsl = _ssl,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_email, _pass)
            };
        }

        ///<Note>
        // Only public method is initiate response. It builds out the email response based on that response
        ///</Note>
        public async Task<StandardResponse> initSendEmail(StandardResponse res, S4Response s4)
        {
            var email = new Email();
            var cc = new List<string>();
            email.recipient = s4.request.email;
            // first add primaries to email chain if applicable
            if(s4.request_type == RequestType.Member || s4.request_type == RequestType.Admin)
            {
                cc = await AddChainOfCommand(s4, cc);
            }

            // If we reject the membership or there was a problem creating the account
            if(s4.request.request_status == Status.Rejected)
            {
                email = await GenerateRejection(s4, email);
            }

            if(s4.request.request_status == Status.Approved)
            {
                // add our subject and opening remark
                email = await GenerateApproval(s4, email);
                if(s4.request_type == RequestType.Primary)
                {
                    // add information pertinent only to account primaries
                    email.body += await AddPrimaryApproval(s4, email);
                }
                // add link to verify email / complete registration / set password 
                email.body += await AddActivationLink(s4, email);
            }

            // Actually sends the email
            await SendEmailAsync(email, cc);
            
            // probably put something here to record the result n yada yada yada...
            return res;
        }

        private Task<string> AddActivationLink(S4Response s4, Email email)
        {
            var s4id = s4.request.request_number;
            var link = $@"http://localhost:5000/setup/{s4.request.request_number}";

            return Task.FromResult(
                $@"<div>   
                    Follow the link to activate your account and set your password: <br><br>
                    <a href={link}>Click to complete registration</a><br>
                    <br>
                    Thanks for yadda bo badda, <br>
                    <br>
                    Your Mom<br>
                </div>");
        }
        private Task<List<string>> AddChainOfCommand(S4Response s4, List<string> cc)
        {
            var _query = $@"TODO";
            var _params = new {};
            using(var conn = new NpgsqlConnection(_conn))
            {
                var primaryAccounts = conn.Query<string>(_query, _params);
                cc.AddRange(primaryAccounts);
                return Task.FromResult(cc);
            }

        }
        private Task<string> AddPrimaryApproval(S4Response s4, Email email)
        {
            return Task.FromResult(
                $@"<div>   
                    You are registered as the primary account holder for {s4.request.organization}. Bestowed upon you is the ability to: <br>
                    <ul>
                        <li>verify account requests under your agency</li>
                        <li>create accounts under your agency</li>
                        <li>delete accounts under your agency</li>
                        <li>view agency statistics dashboard</li>
                        <li>approve/revoke or transfer primary account status</li>
                        <li>update/delete your agency</li>
                    </ul>
                </div>");
        }



        // Generate email object based on status
        private Task<Email> GenerateApproval(S4Response s4, Email email)
        {
            email.subject = $@"Action Required: ***** Account Created";
            email.body = $@"<div>Dear {s4.request.first_name},</div>";
            return Task.FromResult(email);
        }
        private Task<Email> GenerateRejection(S4Response s4, Email email)
        {
            email.subject = $@"Your request for a ***** account has been rejected";
            email.body = $@"
                <div>Dear {s4.request.first_name},<br><br>
                    Your request for a ***** account has been denied.<br><br>
                    The reason given: {s4.message}<br><br>
                    If you believe there has been an error please ****** <br><br>
                    Respectfully<br>
                </div>";
            return Task.FromResult(email);
        }

        private async Task SendEmailAsync(Email email, List<string> cc)
        {
            MailMessage msg = new MailMessage()
            {
                IsBodyHtml = true,
                Subject = email.subject,
                From = new MailAddress(_email),
                Sender = new MailAddress(_email),
                Body = new StringBuilder(email.body).ToString()
            };
            msg.To.Add(new MailAddress(email.recipient));
            
            if(cc != null)
            {
                foreach(var addrs in cc)
                {
                    msg.CC.Add(new MailAddress(addrs));
                }
            }
            await _smtp.SendMailAsync(msg);
        }
    }
}