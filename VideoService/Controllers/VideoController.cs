using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using VideoService.Application;
using VideoService.Domain.Entities;
using VideoService.Infrastructure;
using VideoService.Services;

namespace VideoService.Controllers
{
    [ApiController]
    [Route("video")]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _svc;
        private readonly IJoinTokenService _tokenService;
        private readonly ApplicationDbContext _db;

        public VideoController(IVideoService svc, IJoinTokenService tokenService, ApplicationDbContext db)
        {
            _svc = svc;
            _tokenService = tokenService;
            _db = db;
        }

        [HttpPost("rooms")]
        public async Task<IActionResult> CreateRoom([FromBody] CreateRoomRequest req)
        {
            // In production validate appointment exists, caller permissions.
            var room = await _svc.CreateVideoRoomForAppointmentAsync(req.AppointmentId, req.WaitingRoom);
            return Ok(new { room.VideoRoomId, room.AppointmentId, room.ExternalRoomUrl });
        }

        [HttpPost("rooms/{appointmentId:guid}/generate-link")]
        public async Task<IActionResult> GenerateJoinLink(Guid appointmentId, [FromBody] GenerateLinkRequest req)
        {
            var url = await _svc.GenerateJoinLinkAsync(appointmentId, req.UserId, req.Role);
            return Ok(new { joinUrl = url });
        }

        // Public endpoint that the user visits. Validates internal join token then redirects to external provider.
        [HttpGet("join")]
        public async Task<IActionResult> Join([FromQuery] string token)
        {
            var principal = _tokenService.ValidateJoinToken(token);
            if (principal == null) return Unauthorized("Invalid or expired token");

            var appointmentId = Guid.Parse(principal.Claims.First(c => c.Type == "appointmentId").Value);
            var userId = Guid.Parse(principal.Claims.First(c => c.Type == "userId").Value);
            var role = principal.FindFirst(System.Security.Claims.ClaimTypes.Role)?.Value ?? "patient";

            var room = await _db.VideoRooms.FirstOrDefaultAsync(r => r.AppointmentId == appointmentId);
            if (room == null) return NotFound("Video room not found");

            var appointment = await _db.Appointments.FirstOrDefaultAsync(w => w.AppointmentId == appointmentId);
            if (appointment.Status == "Completed")
            {
                return BadRequest(new { message = "Consultation has already ended." });
            }



            // Waiting room check
            if (room.IsWaitingRoomEnabled && role == "patient")
            {
                var wr = await _db.WaitingRoomStatuses.FirstOrDefaultAsync(w => w.AppointmentId == appointmentId && w.PatientId == userId);
                if (wr == null)
                {
                    // create record
                    wr = new WaitingRoomStatus { WaitingRoomStatusId = Guid.NewGuid(), AppointmentId = appointmentId, PatientId = userId, Status = "Pending", UpdatedAt = DateTime.UtcNow };
                    _db.WaitingRoomStatuses.Add(wr);
                    await _db.SaveChangesAsync();
                }

                if (wr.Status != "Allowed")
                {
                    // return a small "Please wait" page (or json) rather than redirect
                    return Ok(new { status = "waiting", message = "Please wait until the doctor admits you." });
                }
            }

            // If allowed or no waiting room, redirect to external URL
            return Redirect(room.ExternalRoomUrl);
        }

        [HttpPatch("appointments/{appointmentId:guid}/allow-patient")]
        public async Task<IActionResult> AllowPatient(Guid appointmentId, [FromBody] AllowPatientRequest req)
        {
            // validate doctor is caller in production
            var ok = await _svc.AllowPatientAsync(appointmentId, req.DoctorId);
            if (!ok) return NotFound();
            // we could push a notification via SignalR / events so patient auto-redirects
            return NoContent();
        }

        [HttpPost("create")]
        public async Task<IActionResult> CreateAppointment([FromBody] CreateVideoAppointmentRequest req)
        {
            var result = await ((VideoServiceImpl)_svc).CreateFullVideoAppointmentAsync(
                req.DoctorId,
                req.PatientId,
                req.ScheduledStart,
                req.ScheduledEnd,
                req.WaitingRoom
            );

            return Ok(result);
        }

        [HttpGet("appointments/{appointmentId:guid}/waiting-status/{patientId:guid}")]
        public async Task<IActionResult> GetWaitingStatus(Guid appointmentId, Guid patientId)
        {
            var wr = await _svc.GetWaitingStatusAsync(appointmentId, patientId);
            if (wr == null) return NotFound();
            return Ok(new { wr.Status, wr.UpdatedAt });
        }

        [HttpPatch("{appointmentId:guid}/end")]
        public async Task<IActionResult> EndConsultation(Guid appointmentId, [FromBody] EndConsultationRequest req)
        {
            var ok = await ((VideoServiceImpl)_svc)
                .EndConsultationAsync(appointmentId, req.DoctorId);

            if (!ok)
                return NotFound("Appointment not found or unauthorized");

            return NoContent();
        }

        public record EndConsultationRequest(Guid DoctorId);


    }

    // Request DTOs
    public record CreateRoomRequest(Guid AppointmentId, bool WaitingRoom = true);
    public record GenerateLinkRequest(Guid UserId, string Role); // role: patient/doctor
    public record AllowPatientRequest(Guid DoctorId);
}
