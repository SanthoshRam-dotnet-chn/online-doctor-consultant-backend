using NotificationService.Events;
using NotificationService.Helpers;

namespace NotificationService.Services
{
    public class MeetingConfirmedService
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<MeetingConfirmedService> _logger;

        public MeetingConfirmedService(IEmailService emailService, ILogger<MeetingConfirmedService> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        public async Task HandleAsync(MeetingConfirmedEvent evt)
        {
            try
            {
                string subject = "Your Appointment is Confirmed!";
                string body = TemplateHelper.LoadTemplate("MeetingConfirmed.html", new Dictionary<string, string>
                {
                    { "Name", evt.PatientName },
                    { "MeetingLink", evt.MeetingLink },
                    { "Purpose", evt.Purpose },
                    { "Date", evt.AppointmentDate.ToString("dd MMM yyyy") },
                    { "StartTime", evt.StartTime.ToString(@"hh\:mm") },
                    { "EndTime", evt.EndTime.ToString(@"hh\:mm") },
                    { "ButtonLink", evt.MeetingLink },
                    { "ButtonText", "Join Meeting" }
                });

                await _emailService.SendEmail(evt.PatientEmail, subject, body);
                _logger.LogInformation($"Meeting confirmed email sent to {evt.PatientEmail}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Failed to send meeting confirmed email to {evt.PatientEmail}");
                throw;
            }
        }
    }
}
