using HMS.Models;
using HMS.Services.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;


namespace HMS.Services.Repositories
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailConfiguration _emailConfiguration;
        
        public EmailSender(EmailConfiguration emailConfiguration)
        {
            _emailConfiguration = emailConfiguration;
        }
        public void SendEmail(EmailMessage message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(EmailMessage message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(_emailConfiguration.From, _emailConfiguration.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(TextFormat.Html)
            {
                Text = message.Content
            };
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfiguration.SmtpServer, _emailConfiguration.Port, false);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfiguration.UserName, _emailConfiguration.Password);
                    client.Send(mailMessage);
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }
        }
    }
}
