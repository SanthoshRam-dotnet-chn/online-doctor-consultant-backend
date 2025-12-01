using System.Net;
using System.Net.Mail;

namespace NotificationService.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration configuration, ILogger<EmailService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task SendEmail(string receptor, string subject, string body)
        {
            try
            {
                var email = _configuration.GetValue<string>("EMAIL_CONFIGURATION:EMAIL");
                var password = _configuration.GetValue<string>("EMAIL_CONFIGURATION:PASSWORD");
                var host = _configuration.GetValue<string>("EMAIL_CONFIGURATION:HOST");
                var port = _configuration.GetValue<int>("EMAIL_CONFIGURATION:PORT");

                using var smtp = new SmtpClient(host, port)
                {
                    EnableSsl = true,
                    UseDefaultCredentials = false,
                    Credentials = new NetworkCredential(email, password)
                };

                var message = new MailMessage(email, receptor, subject, body)
                {
                    IsBodyHtml = true
                };

                await smtp.SendMailAsync(message);

                _logger.LogInformation($"Email sent successfully to {receptor} with subject '{subject}'");
            }
            catch (SmtpException ex)
            {
                _logger.LogError(ex, $"SMTP error sending email to {receptor}");
                throw new Exception("Email sending failed. Please try again later.", ex);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"General error sending email to {receptor}");
                throw new Exception("An unexpected error occurred while sending email.", ex);
            }
        }
    }
}
