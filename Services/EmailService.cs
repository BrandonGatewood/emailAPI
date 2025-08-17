using emailAPI.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

namespace emailAPI.Services
{
    public class EmailService(IConfiguration config) : IEmailService
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
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress("My Site", _email));
            message.To.Add(new MailboxAddress("Me", _email));
            message.Subject = $"{form.Name} wants to connect!!";
            message.Body = new TextPart("plain")
            {
                Text = $"Name: {form.Name}\nEmail: {form.Email}\nMessage: {form.Message}"
            };

            using var client = new SmtpClient();
            try
            {
                // Connect securely using SSL
                await client.ConnectAsync(_smtpServer, _smtpPort, SecureSocketOptions.StartTls);

                // Authenticate
                await client.AuthenticateAsync(_email, _password);

                // Send the email
                await client.SendAsync(message);

                // Disconnect cleanly
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to send email.", ex);
            }
        }
    }
}