using System.Net;
using System.Net.Mail;
using emailAPI.Models;

namespace emailAPI.Services
{
    public class EmailService
    {
        public readonly string _smtpServer;
        public readonly int _smtpPort;
        public readonly string _email;
        public readonly string _password;

        public EmailService()
        {
            _smtpServer = Environment.GetEnvironmentVariable("SMTP_SERVER")
                ?? throw new InvalidOperationException("SMTP_SERVER is not set in environment variables.");
            _smtpPort = int.Parse(Environment.GetEnvironmentVariable("SMTP_PORT")
                ?? throw new InvalidOperationException("SMTP_PORT is not set in environment variables."));
            _email = Environment.GetEnvironmentVariable("EMAIL")
                ?? throw new InvalidOperationException("EMAIL is not set in environment variables.");
            _password = Environment.GetEnvironmentVariable("PASSWORD")
                ?? throw new InvalidOperationException("PASSWORD is not set in environment variables.");
        }

        public async Task SendEmail(ContactForm form)
        {
            using var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(_email, _password),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_email),
                Subject = $"{form.Name} wants to connect!!",
                Body = $"Name: {form.Name}\nEmail: {form.Email}\nMessage: {form.Message}",
                IsBodyHtml = false
            };
            mailMessage.To.Add(_email);

            await client.SendMailAsync(mailMessage);
        }
    }
}