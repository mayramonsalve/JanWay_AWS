using JWA.AUTH.Interface;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWA.AUTH.Repository
{
    public class GeneralService: IGeneralServices
    {
        public async Task<Response> send_email_sendgrid(string apiKey, string emailTo, string emailSubject, string emailBody)
        {
            var client = new SendGridClient(apiKey);
            var msg = new SendGridMessage()
            {
                From = new EmailAddress("jcloud@thejanway.com", "Jan-Way"),
                Subject = emailSubject,
                HtmlContent = emailBody
            };
            msg.AddTo(new EmailAddress(emailTo, "user"));
            var response = await client.SendEmailAsync(msg);
            return response;
        }
    }
}
