using Microsoft.EntityFrameworkCore;
using VideoService.Domain.Entities;
using VideoService.Infrastructure;
using VideoService.Services;
using VideoService.Services.Providers;

namespace VideoService.Application
{
    public interface IVideoService
    {
        Task<VideoRoom> CreateVideoRoomForAppointmentAsync(Guid appointmentId, bool waitingRoom);
        Task<string> GenerateJoinLinkAsync(Guid appointmentId, Guid userId, string role);
        Task<bool> AllowPatientAsync(Guid appointmentId, Guid doctorId);
        Task<WaitingRoomStatus?> GetWaitingStatusAsync(Guid appointmentId, Guid patientId);
        Task<object> CreateFullVideoAppointmentAsync(
            Guid doctorId,
            Guid patientId,
            DateTime start,
            DateTime end,
            bool waitingRoom);
        Task<bool> EndConsultationAsync(Guid appointmentId, Guid doctorId);


     }

    public class VideoServiceImpl : IVideoService
    {
        private readonly ApplicationDbContext _db;
        private readonly IVideoProvider _provider;
        private readonly IJoinTokenService _tokenService;
        private readonly IConfiguration _cfg;

        public VideoServiceImpl(ApplicationDbContext db, IVideoProvider provider, IJoinTokenService tokenService, IConfiguration cfg)
        {
            _db = db;
            _provider = provider;
            _tokenService = tokenService;
            _cfg = cfg;
        }

        public async Task<VideoRoom> CreateVideoRoomForAppointmentAsync(Guid appointmentId, bool waitingRoom)
        {
            var existing = await _db.VideoRooms.FirstOrDefaultAsync(v => v.AppointmentId == appointmentId);
            if (existing != null) return existing;

            var (roomName, externalUrl) = await _provider.CreateRoomAsync(appointmentId, waitingRoom);

            var room = new VideoRoom
            {
                VideoRoomId = Guid.NewGuid(),
                AppointmentId = appointmentId,
                Provider = "JITSI",
                RoomName = roomName,
                ExternalRoomUrl = externalUrl,
                IsWaitingRoomEnabled = waitingRoom
            };

            _db.VideoRooms.Add(room);
            await _db.SaveChangesAsync();
            return room;
        }

        public Task<string> GenerateJoinLinkAsync(Guid appointmentId, Guid userId, string role)
        {
            // Generates short-lived token (not the external provider token)
            var token = _tokenService.GenerateJoinToken(appointmentId, userId, role);
            // Join endpoint will validate and redirect
            var baseurl = _cfg["AppBaseUrl"]?.TrimEnd('/') ?? "http://localhost:5002";
            var joinUrl = $"{baseurl}/video/join?token={token}";
            return Task.FromResult(joinUrl);
        }

        public async Task<bool> AllowPatientAsync(Guid appointmentId, Guid doctorId)
        {
            // In real system verify doctorId belongs to appointment's doctor
            var wr = await _db.WaitingRoomStatuses.FirstOrDefaultAsync(w => w.AppointmentId == appointmentId);
            if (wr == null) return false;
            wr.Status = "Allowed";
            wr.UpdatedAt = DateTime.UtcNow;
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task<object> CreateFullVideoAppointmentAsync(
            Guid doctorId,
            Guid patientId,
            DateTime start,
            DateTime end,
            bool waitingRoom)
        {
            // 1. Create Appointment
            var appointment = new Appointment
            {
                AppointmentId = Guid.NewGuid(),
                DoctorId = doctorId,
                PatientId = patientId,
                ScheduledStart = start,
                ScheduledEnd = end,
                Status = "Scheduled"
            };

            _db.Appointments.Add(appointment);
            await _db.SaveChangesAsync();

            // 2. Create Video Room
            var room = await CreateVideoRoomForAppointmentAsync(appointment.AppointmentId, waitingRoom);

            // 3. Create Waiting Room Entry
            if (waitingRoom)
            {
                var wr = new WaitingRoomStatus
                {
                    WaitingRoomStatusId = Guid.NewGuid(),
                    AppointmentId = appointment.AppointmentId,
                    PatientId = patientId,
                    Status = "Pending",
                    UpdatedAt = DateTime.UtcNow
                };

                _db.WaitingRoomStatuses.Add(wr);
                await _db.SaveChangesAsync();
            }

            // 4. Generate doctor link
            var doctorJoin = await GenerateJoinLinkAsync(
                appointment.AppointmentId,
                doctorId,
                "doctor"
            );

            // 5. Generate patient link
            var patientJoin = await GenerateJoinLinkAsync(
                appointment.AppointmentId,
                patientId,
                "patient"
            );

            return new
            {
                appointmentId = appointment.AppointmentId,
                videoRoomUrl = room.ExternalRoomUrl,
                doctorJoinLink = doctorJoin,
                patientJoinLink = patientJoin
            };
        }

        public async Task<bool> EndConsultationAsync(Guid appointmentId, Guid doctorId)
        {
            var appt = await _db.Appointments.FirstOrDefaultAsync(a => a.AppointmentId == appointmentId);
            if (appt == null) return false;

            // Optional: verify doctor ending meeting is the same doctor
            if (appt.DoctorId != doctorId)
                return false;

            appt.Status = "Completed";
            appt.EndedAt = DateTime.UtcNow;

            // OPTIONAL: clean waiting room
            var wr = await _db.WaitingRoomStatuses
                .Where(w => w.AppointmentId == appointmentId)
                .ToListAsync();

            if (wr.Any())
                _db.WaitingRoomStatuses.RemoveRange(wr);

            // OPTIONAL: remove video room entry if you want temporary rooms
            // var room = await _db.VideoRooms.FirstOrDefaultAsync(v => v.AppointmentId == appointmentId);
            // if (room != null) _db.VideoRooms.Remove(room);

            await _db.SaveChangesAsync();
            return true;
        }


        public Task<WaitingRoomStatus?> GetWaitingStatusAsync(Guid appointmentId, Guid patientId)
        {
            return _db.WaitingRoomStatuses.FirstOrDefaultAsync(w => w.AppointmentId == appointmentId && w.PatientId == patientId);
        }
    }
}
