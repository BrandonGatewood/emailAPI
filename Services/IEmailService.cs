using emailAPI.Models;

namespace emailAPI.Services
{
    public interface IEmailService
    {
        Task SendEmail(ContactForm form);
    }
}