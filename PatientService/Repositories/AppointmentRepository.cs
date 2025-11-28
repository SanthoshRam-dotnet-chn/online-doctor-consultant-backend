using Microsoft.EntityFrameworkCore;
using PatientService.Data;
using PatientService.Domain.Entities;

namespace PatientService.Repositories
{
    public class AppointmentRepository : IAppointmentRepository
    {
        private readonly PatientDbContext _context;

        public AppointmentRepository(PatientDbContext context)
        {
            _context = context;
        }

        public async Task<Appointment> Create(Appointment appointment)
        {
            _context.Appointments.Add(appointment);
            await _context.SaveChangesAsync();
            return appointment;
        }

        public async Task<Appointment?> GetById(Guid id)
        {
            return await _context.Appointments
                .FirstOrDefaultAsync(a => a.Id == id);
        }

        public async Task<IEnumerable<Appointment>> GetByPatientId(Guid patientId)
        {
            return await _context.Appointments
                .Where(a => a.PatientId == patientId)
                .ToListAsync();
        }
    }

}
