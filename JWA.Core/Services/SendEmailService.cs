using JWA.Core.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;

namespace JWA.Core.Services
{
    public class SendEmailService: ISendEmailService
    {
        public void send_email(string emailTo,string emailSubject,string emailBody)
        {
            string sender_Email_Address = "mike.jhonson1808@gmail.com";
            var sender_Email = new MailAddress(sender_Email_Address, sender_Email_Address);
            var receiverEmail = new MailAddress(emailTo, emailTo);

            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new System.Net.NetworkCredential(sender_Email.Address, "Lov3melikeyoudo")
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
    }
}
