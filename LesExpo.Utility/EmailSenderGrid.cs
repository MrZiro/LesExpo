using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LesExpo.Utility
{
    public class EmailSenderGrid : IEmailSender
    {
        public string SendGridSecret { get; set; }

        public EmailSenderGrid(IConfiguration _configuration)
        {
            SendGridSecret = _configuration.GetSection("SendGrid:SecretKey").Value ?? throw new ArgumentNullException("SendGrid secret is not configured.");
        }

        public async Task SendEmailAsync(string email, string subject, string htmlMessage)
        {
            try
            {
                var client = new SendGridClient(SendGridSecret);
                var from = new EmailAddress("lesexpofair@gmail.com", "LES-EXPO İletişim");
                var to = new EmailAddress(email);
                var msg = MailHelper.CreateSingleEmail(from, to, subject, "", htmlMessage);
                var response = await client.SendEmailAsync(msg);
                if (!response.IsSuccessStatusCode)
                {
                    throw new InvalidOperationException($"SendGrid failed: {response.StatusCode}");
                }
            }
            catch (Exception ex)
            {
                // Log the exception or handle it as needed
                throw new InvalidOperationException("Failed to send email.", ex);
            }
        }
    }
}
