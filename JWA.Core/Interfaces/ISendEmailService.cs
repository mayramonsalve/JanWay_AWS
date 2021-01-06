using System;
using System.Collections.Generic;
using System.Text;

namespace JWA.Core.Interfaces
{
    public interface ISendEmailService
    {
        void send_email(string emailTo, string emailSubject, string emailBody);
    }
}
