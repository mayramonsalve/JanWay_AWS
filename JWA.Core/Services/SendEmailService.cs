using JWA.Core.Interfaces;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace JWA.Core.Services
{
    public class SendEmailService: ISendEmailService
    {
        public void send_email(string emailTo,string emailSubject,string emailBody)
        {
            string sender_Email_Address = "mayraalejandra3190@gmail.com";
            var sender_Email = new MailAddress(sender_Email_Address, sender_Email_Address);
            var receiverEmail = new MailAddress(emailTo, emailTo);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(sender_Email.Address, "Aleja3190++")
            };
            using (var mess = new MailMessage(sender_Email, receiverEmail)
            {
                //Subject = string.Format("Invite to collaborate with JanWay - {0}", "Jan-Way"),
                Subject = emailSubject,
                Body = emailBody
            })
            {
                //mess.Bcc.Add(obj.BCC);
                mess.IsBodyHtml = true;
                smtp.Send(mess);
            }
        }
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
