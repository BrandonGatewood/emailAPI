using emailAPI.Models;
using emailAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace emailAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailController(EmailService emailService) : ControllerBase
    {
        private readonly EmailService _emailService = emailService;

        [HttpPost]
        public async Task<IActionResult> SendMessage([FromBody] ContactForm form)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            try
            {
                await _emailService.SendEmail(form);
                return Ok(new { message = "Message sent successfully!" });
            }
            catch (Exception)
            {
                return StatusCode(500, new { message = "Failed to send message." });
            }
        }
    }
}