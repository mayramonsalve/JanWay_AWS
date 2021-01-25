using SendGrid;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace JWA.AUTH.Interface
{
    public interface IGeneralServices
    {
        Task<Response> send_email_sendgrid(string apiKey, string emailTo, string emailSubject, string emailBody);
    }
}
