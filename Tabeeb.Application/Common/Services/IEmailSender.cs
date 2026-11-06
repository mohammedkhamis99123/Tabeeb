using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Tabeeb.Application.Common.Services
{
    public  interface IEmailSender
    {
        Task SendEmail(string email,string Body, byte[]?Attachments);
    }
}
