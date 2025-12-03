using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using VideoService.Infrastructure;
using VideoService.Services;

namespace VideoService.Controllers
{
    [ApiController]
    [Route("video")]
    public class VideoController : ControllerBase
    {
        private readonly IVideoService _videoService;
        private readonly IJoinTokenService _tokenService;
        private readonly ApplicationDbContext _db;

        public VideoController(
            IVideoService videoService,
            IJoinTokenService tokenService,
            ApplicationDbContext db)
        {
            _videoService = videoService;
            _tokenService = tokenService;
            _db = db;
        }

        [HttpPost("create")]
        [Authorize]
        public async Task<IActionResult> CreateRoom([FromBody] Guid appointmentId)
        {
            var room = await _videoService.CreateRoom(appointmentId);
            return Ok(room);
        }

        [HttpPost("generate-link")]
        [Authorize]
        public async Task<IActionResult> GenerateLink([FromBody] GenerateLinkRequest req)
        {
            var link = await _videoService.GenerateJoinLink(req.AppointmentId, req.UserId, req.Role);
            return Ok(new { link });
        }

        [HttpPatch("allow/{appointmentId}")]
        [Authorize]
        public async Task<IActionResult> Allow(Guid appointmentId)
        {
            var ok = await _videoService.AllowPatient(appointmentId);
            return ok ? Ok() : NotFound();
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test() => Ok("Video Service is working!");

       [HttpGet("join")]
[Authorize]
public async Task<IActionResult> Join([FromQuery] string token)
{
    var principal = _tokenService.Validate(token);
    if (principal == null)
        return Unauthorized("Invalid token");

    var apptId = Guid.Parse(principal.FindFirst("appointmentId")!.Value);
    var role = principal.FindFirst(ClaimTypes.Role)!.Value;
    var userId = Guid.Parse(principal.FindFirst("userId")!.Value);

    var room = await _db.VideoRooms.FirstOrDefaultAsync(v => v.AppointmentId == apptId);
    if (room == null)
        return NotFound("Room not found");

    if (room.IsWaitingRoomEnabled && role == "patient")
    {
        var wr = await _db.WaitingRoomStatuses
            .FirstOrDefaultAsync(w => w.AppointmentId == apptId && w.PatientId == userId);

        if (wr == null || wr.Status != "Allowed")
        {
            return Ok(new 
            { 
                status = "waiting", 
                message = "Doctor will admit you shortly" 
            });
        }
    }

    return Ok(new 
    {
        status = "allowed",
        meetingUrl = room.ExternalRoomUrl
    });
}
    }

    public record GenerateLinkRequest(Guid AppointmentId, Guid UserId, string Role);
}
