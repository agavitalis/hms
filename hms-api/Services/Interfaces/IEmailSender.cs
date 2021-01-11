using HMS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HMS.Services.Interfaces
{
    public interface IEmailSender
    {
        void SendEmail(EmailMessage message);
    }
}
