using PatientService.Domain.Entities;

namespace PatientService.Services
{
    public interface IPatientService
    {
        Task<AppointmentResponse> BookAppointment(BookAppointmentRequest request);
        Task<AppointmentResponse?> GetAppointment(Guid appointmentId);
        Task<IEnumerable<AppointmentResponse>> GetAppointmentsForPatient(Guid patientId);
        Task<IEnumerable<AppointmentResponse>> GetAppointmentsForDoctor(Guid doctorId);
    }


}
