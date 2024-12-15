using Microsoft.AspNetCore.Mvc;
using MimeKit;
using MailKit.Net.Smtp;
using Project.Models;
using Project.DTOs;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EmailController : ControllerBase
    {
        [HttpPost("send-email")]
        public IActionResult SendEmail([FromBody] EmailRequest emailRequest)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("MonoCept", "suraj.sakhare5869@gmail.com"));
            emailMessage.To.Add(new MailboxAddress("", emailRequest.To));
            emailMessage.Subject = emailRequest.Subject;
            emailMessage.Body = new TextPart("html") { Text = emailRequest.Body };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
             
                    client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    client.Authenticate("suraj.sakhare5869@gmail.com", "oynm hlim sevo lhlr"); // Use App Password
                    client.Send(emailMessage);
                    client.Disconnect(true);

                    return Ok(new { message = "Email sent successfully!" });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { messageDeliveryStatus = $"Failed to send email: {ex.Message}" });
                }
            }
        }
        [HttpPost("send-email/Agents")]
        public IActionResult SendEmailAgents([FromBody] EmailRequestAgentDto emailRequest)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress("MonoCept", "suraj.sakhare5869@gmail.com"));

            // Add all recipients to the 'To' field
            foreach (var recipient in emailRequest.To)
            {
                emailMessage.To.Add(new MailboxAddress("", recipient));
            }

            emailMessage.Subject = emailRequest.Subject;
            emailMessage.Body = new TextPart("html") { Text = emailRequest.Body };

            using (var client = new MailKit.Net.Smtp.SmtpClient())
            {
                try
                {
                    client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
                    client.Authenticate("suraj.sakhare5869@gmail.com", "oynm hlim sevo lhlr"); // Use App Password
                    client.Send(emailMessage);
                    client.Disconnect(true);

                    return Ok(new { message = "Email sent successfully to multiple recipients!" });
                }
                catch (Exception ex)
                {
                    return BadRequest(new { messageDeliveryStatus = $"Failed to send email: {ex.Message}" });
                }
            }
        }
    }
}
