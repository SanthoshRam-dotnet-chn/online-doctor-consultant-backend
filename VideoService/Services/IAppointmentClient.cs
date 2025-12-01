using VideoService.DTOs;

namespace VideoService.Services
{
    public interface IAppointmentClient
    {
        Task<AppointmentDto?> GetAppointment(Guid appointmentId);
    }
}
