using System.Net;
using System.Net.Mail;
using emailAPI.Models;

namespace emailAPI.Services
{
    public class EmailService(IConfiguration config)
    {
        private readonly string _smtpServer= config["SMTP_SERVER"]
            ?? throw new InvalidOperationException("SMTP_SERVER is not set in environment variables.");
        private readonly int _smtpPort = int.Parse(config["SMTP_PORT"]
            ?? throw new InvalidOperationException("SMTP_PORT is not set in environment variables."));
        private readonly string _email = config["EMAIL"]
            ?? throw new InvalidOperationException("EMAIL is not set in environment variables.");
        private readonly string _password = config["PASSWORD"]
            ?? throw new InvalidOperationException("PASSWORD is not set in environment variables.");

        public async Task SendEmail(ContactForm form)
        {
            using var client = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(_email, _password),
                EnableSsl = true
            };

            using var mailMessage = new MailMessage
            {
                From = new MailAddress(_email),
                Subject = $"{form.Name} wants to connect!!",
                Body = $"Name: {form.Name}\nEmail: {form.Email}\nMessage: {form.Message}",
                IsBodyHtml = false
            };
            mailMessage.To.Add(_email);

            try
            {
                await client.SendMailAsync(mailMessage);
            }
            catch (SmtpException ex)
            {
                throw new InvalidOperationException("Failed to send email.", ex);
            }
        }
    }
}