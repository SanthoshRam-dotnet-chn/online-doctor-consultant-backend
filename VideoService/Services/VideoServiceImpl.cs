using VideoService.Domain.Entities;
using VideoService.Infrastructure;
using VideoService.Services.Providers;
using Microsoft.EntityFrameworkCore;
using VideoService.DTOs;

namespace VideoService.Services
{
    public interface IVideoService
    {
        Task<VideoRoom> CreateRoom(Guid appointmentId);
        Task<string> GenerateJoinLink(Guid appointmentId, Guid userId, string role);
        Task<bool> AllowPatient(Guid appointmentId);
    }

    public class VideoServiceImpl : IVideoService
    {
        private readonly ApplicationDbContext _db;
        private readonly IAppointmentClient _apptClient;
        private readonly IVideoProvider _provider;
        private readonly IJoinTokenService _tokenService;
        private readonly IConfiguration _cfg;

        public VideoServiceImpl(
            ApplicationDbContext db,
            IAppointmentClient apptClient,
            IVideoProvider provider,
            IJoinTokenService tokenService,
            IConfiguration cfg)
        {
            _db = db;
            _apptClient = apptClient;
            _provider = provider;
            _tokenService = tokenService;
            _cfg = cfg;
        }

        public async Task<VideoRoom> CreateRoom(Guid appointmentId)
        {
            var appt = await _apptClient.GetAppointment(appointmentId);           
            if (appt == null)
                throw new Exception("Appointment not found in PatientService");

            var existing = await _db.VideoRooms.FirstOrDefaultAsync(v => v.AppointmentId == appointmentId);
            if (existing != null) return existing;

            var (roomName, url) = await _provider.CreateRoom(appointmentId, true);

            var room = new VideoRoom
            {
                VideoRoomId = Guid.NewGuid(),
                AppointmentId = appointmentId,
                RoomName = roomName,
                ExternalRoomUrl = url,
                Provider = "JITSI",
                IsWaitingRoomEnabled = true
            };

            _db.VideoRooms.Add(room);
            await _db.SaveChangesAsync();

            // create WaitingRoomStatus record
            _db.WaitingRoomStatuses.Add(new WaitingRoomStatus
            {
                WaitingRoomStatusId = Guid.NewGuid(),
                AppointmentId = appointmentId,
                PatientId = appt.PatientId,
                Status = "Pending"
            });
            await _db.SaveChangesAsync();

            return room;
        }

        public async Task<string> GenerateJoinLink(Guid appointmentId, Guid userId, string role)
        {
            var appt = await _apptClient.GetAppointment(appointmentId);
            if (appt == null) throw new Exception("Appointment not found");

            if (role == "patient" && appt.PatientId != userId)
                throw new UnauthorizedAccessException("Not your appointment");

            if (role == "doctor" && appt.DoctorId != userId)
                throw new UnauthorizedAccessException("Not your appointment");

            var token = _tokenService.GenerateJoinToken(appointmentId, userId, role);
            return $"{_cfg["AppBaseUrl"]}/video/join?token={token}";
        }

        public async Task<bool> AllowPatient(Guid appointmentId)
        {
            var wr = await _db.WaitingRoomStatuses
                .FirstOrDefaultAsync(w => w.AppointmentId == appointmentId);

            if (wr == null) return false;

            wr.Status = "Allowed";
            wr.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }
    }
}
