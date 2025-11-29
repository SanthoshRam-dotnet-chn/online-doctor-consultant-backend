using NotificationService.Events;
using NotificationService.Helpers;

namespace NotificationService.Services
{
    public class MeetingCancelledService
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<MeetingCancelledService> _logger;

        public MeetingCancelledService(IEmailService emailService, ILogger<MeetingCancelledService> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task HandleAsync(MeetingCancelledEvent evt)
        {
            try
            {
                string subject = "Your Appointment Has Been Cancelled";
                string body = TemplateHelper.LoadTemplate("MeetingCancelled.html", new Dictionary<string, string>
                {
                    { "Name", evt.PatientName },
                    { "Date", evt.AppointmentDate.ToString("dd MMM yyyy") },
                    { "AppointmentTime", evt.AppointmentTime },
                    { "Reason", evt.Reason },
                    { "ButtonLink", "https://yourwebsite.com/reschedule" },
                    { "ButtonText", "Reschedule" }
                });

                await _emailService.SendEmail(evt.PatientEmail, subject, body);
                _logger.LogInformation($"Meeting cancelled email sent to {evt.PatientEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send meeting cancelled email to {evt.PatientEmail}");
                throw;
            }
        }
    }
}
