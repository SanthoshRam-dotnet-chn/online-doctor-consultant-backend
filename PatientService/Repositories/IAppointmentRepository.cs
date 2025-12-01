
using PatientService.Domain.Entities;

namespace PatientService.Repositories
{
    public interface IAppointmentRepository
    {
        Task<Appointment> Create(Appointment appointment);
        Task<Appointment?> GetById(Guid id);
        Task<IEnumerable<Appointment>> GetByPatientId(Guid patientId);
        Task<IEnumerable<Appointment>> GetByDoctorId(Guid doctorId);
    }

}
