using SendGrid;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace JWA.Core.Interfaces
{
    public interface ISendEmailService
    {
        void send_email(string emailTo, string emailSubject, string emailBody);
        Task<Response> send_email_sendgrid(string apiKey, string emailTo, string emailSubject, string emailBody);
    }
}
