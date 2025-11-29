using NotificationService.Events;
using NotificationService.Helpers;

namespace NotificationService.Services
{
    public class RegistrationNotificationService
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<RegistrationNotificationService> _logger;

        public RegistrationNotificationService(IEmailService emailService, ILogger<RegistrationNotificationService> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task HandleAsync(RegistrationEvent evt)
        {
            try
            {
                string subject = "Welcome to Our Service!";
                string body = TemplateHelper.LoadTemplate("RegistrationEmail.html", new Dictionary<string, string>
                {
                    { "Name", evt.Name },
                    { "Email", evt.Email },
                    { "Date", evt.RegistrationDate.ToString("dd MMM yyyy") },
                    { "ButtonLink", "https://yourwebsite.com/login" },
                    { "ButtonText", "Login Now" }
                });

                await _emailService.SendEmail(evt.Email, subject, body);
                _logger.LogInformation($"Registration email sent to {evt.Email}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send registration email to {evt.Email}");
                throw;
            }
        }
    }
}
