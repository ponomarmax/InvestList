using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using System.Net.Mail;
using System.Net;
using InvestList.Configs;

namespace InvestList.Services
{
    public class EmailSender(IOptions<EmailConfig> smtpSettings): IEmailSender
    {
        private readonly EmailConfig _smtpSettings = smtpSettings.Value;

        public Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            var smtpClient = new SmtpClient
            {
                Host = _smtpSettings.Host,
                Port = _smtpSettings.Port,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(_smtpSettings.Username, _smtpSettings.Password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_smtpSettings.FromEmail),
                Subject = subject,
                Body = htmlMessage,
                IsBodyHtml = true
            };

            mailMessage.To.Add(email);

            return smtpClient.SendMailAsync(mailMessage);
        }
    }
}
