using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using Tabeeb.Application.Common.Services;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace Tabeeb.Infrastructure.Services

{
    public class EmailSender : IEmailSender
    {


        public async Task SendEmail(string email, string body, byte[]? attachment)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("Tabeeb", "mohammedkhamis99123@gmail.com"));
            message.To.Add(MailboxAddress.Parse(email));
            message.Subject = "Tabeeb";

            var builder = new BodyBuilder
            {
                HtmlBody = body
            };

            if (attachment != null && attachment.Length > 0)
            {
                builder.Attachments.Add(
                    "Attachment.pdf",          // اسم الملف
                    attachment,                // البيانات
                    ContentType.Parse("application/pdf")
                );
            }

            message.Body = builder.ToMessageBody();

            using var smtp = new MailKit.Net.Smtp.SmtpClient();

            await smtp.ConnectAsync(
                "smtp.gmail.com",
                587,
                SecureSocketOptions.StartTls);

             await smtp.AuthenticateAsync("mohammedkhamis99123@gmail.com", "keodosulwzloxwoc");

            var x=await smtp.SendAsync(message);

            await smtp.DisconnectAsync(true);
        }
    
}
}
