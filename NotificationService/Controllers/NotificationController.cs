using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using NotificationService.Events;
using NotificationService.Services;

namespace NotificationService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class NotificationController : ControllerBase
    {
        private readonly RegistrationNotificationService _registrationService;
        private readonly MeetingConfirmedService _meetingConfirmedService;
        private readonly MeetingCancelledService _meetingCancelledService;
        private readonly MeetingEndedService _meetingEndedService;
        private readonly ILogger<NotificationController> _logger;

        public NotificationController(
            RegistrationNotificationService registrationService,
            MeetingConfirmedService meetingConfirmedService,
            MeetingCancelledService meetingCancelledService,
            MeetingEndedService meetingEndedService,
            ILogger<NotificationController> logger)
        {
            _registrationService = registrationService;
            _meetingConfirmedService = meetingConfirmedService;
            _meetingCancelledService = meetingCancelledService;
            _meetingEndedService = meetingEndedService;
            _logger = logger;
        }

        [HttpPost("registration")]
        public async Task<IActionResult> SendRegistration([FromBody] RegistrationEvent request)
        {
            try
            {
                await _registrationService.HandleAsync(request);
                return Ok("Registration email sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending registration email to {request.Email}");
                return StatusCode(500, $"Error sending email: {ex.Message}");
            }
        }

        [HttpPost("meeting-confirmed")]
        public async Task<IActionResult> SendMeetingConfirmed([FromBody] MeetingConfirmedEvent request)
        {
            try
            {
                await _meetingConfirmedService.HandleAsync(request);
                return Ok("Meeting confirmed email sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending meeting confirmed email to {request.PatientEmail}");
                return StatusCode(500, $"Error sending email: {ex.Message}");
            }
        }

        [HttpPost("meeting-cancelled")]
        public async Task<IActionResult> SendMeetingCancelled([FromBody] MeetingCancelledEvent request)
        {
            try
            {
                await _meetingCancelledService.HandleAsync(request);
                return Ok("Meeting cancelled email sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending meeting cancelled email to {request.PatientEmail}");
                return StatusCode(500, $"Error sending email: {ex.Message}");
            }
        }

        [HttpPost("meeting-ended")]
        public async Task<IActionResult> SendMeetingEnded([FromBody] MeetingEndedEvent request)
        {
            try
            {
                await _meetingEndedService.HandleAsync(request);
                return Ok("Meeting ended email sent successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending meeting ended email to {request.PatientEmail}");
                return StatusCode(500, $"Error sending email: {ex.Message}");
            }
        }
    }
}
