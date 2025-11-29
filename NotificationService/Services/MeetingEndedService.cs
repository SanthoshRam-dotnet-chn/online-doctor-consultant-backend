using NotificationService.Events;
using NotificationService.Helpers;

namespace NotificationService.Services
{
    public class MeetingEndedService
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<MeetingEndedService> _logger;

        public MeetingEndedService(IEmailService emailService, ILogger<MeetingEndedService> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task HandleAsync(MeetingEndedEvent evt)
        {
            try
            {
                string subject = "Thank You for Attending Your Appointment";
                string body = TemplateHelper.LoadTemplate("MeetingEnded.html", new Dictionary<string, string>
                {
                    { "Name", evt.PatientName },
                    { "Date", evt.AppointmentDate.ToString("dd MMM yyyy") },
                    { "AppointmentTime", evt.AppointmentTime },
                    { "FeedbackLink", evt.FeedbackLink ?? "#" },
                    { "ButtonLink", evt.FeedbackLink ?? "#" },
                    { "ButtonText", "Give Feedback" }
                });

                await _emailService.SendEmail(evt.PatientEmail, subject, body);
                _logger.LogInformation($"Meeting ended email sent to {evt.PatientEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send meeting ended email to {evt.PatientEmail}");
                throw;
            }
        }
    }
}
