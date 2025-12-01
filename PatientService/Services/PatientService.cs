using PatientService.Domain.Entities;
using PatientService.Repositories;

namespace PatientService.Services
{
    public class PatientService : IPatientService
    {
        private readonly IAppointmentRepository _repo;
        private readonly DoctorServiceClient _doctorClient;

        public PatientService(IAppointmentRepository repo, DoctorServiceClient doctorClient)
        {
            _repo = repo;
            _doctorClient = doctorClient;
        }

        public async Task<AppointmentResponse> BookAppointment(BookAppointmentRequest request)
        {
            // 1. Get slot info
            var slot = await _doctorClient.GetSlotById(request.SlotId);

            if (slot == null)
                throw new Exception("Slot does not exist.");

            if (!slot.IsAvailable)
                throw new Exception("Slot is not available.");

            // 2. Mark slot as booked
            var booked = await _doctorClient.MarkSlotAsBooked(request.SlotId);
            if (!booked)
                throw new Exception("Failed to mark slot as booked.");

            // 3. Create appointment
            var appointment = new Appointment
            {
                Id = Guid.NewGuid(),
                SlotId = request.SlotId,
                PatientId = request.PatientId,
                CreatedAt = DateTime.UtcNow,
                DoctorId = slot.DoctorId
            };

            await _repo.Create(appointment);

            // 4. Return result
            return new AppointmentResponse
            {
                AppointmentId = appointment.Id,
                DoctorId = slot.DoctorId,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime,
                PatientId = appointment.PatientId,
                CreatedAt = appointment.CreatedAt
            };
        }

        public async Task<AppointmentResponse?> GetAppointment(Guid appointmentId)
        {
            var appointment = await _repo.GetById(appointmentId);
            if (appointment == null)
                return null;

            var slot = await _doctorClient.GetSlotById(appointment.SlotId);
            if (slot == null)
                return null;

            return new AppointmentResponse
            {
                AppointmentId = appointment.Id,
                DoctorId = slot.DoctorId,
                StartTime = slot.StartTime,
                EndTime = slot.EndTime,
                PatientId = appointment.PatientId,
                CreatedAt = appointment.CreatedAt
            };
        }

        public async Task<IEnumerable<AppointmentResponse>> GetAppointmentsForPatient(Guid patientId)
        {
            var list = await _repo.GetByPatientId(patientId);

            var responses = new List<AppointmentResponse>();

            foreach (var appointment in list)
            {
                var slot = await _doctorClient.GetSlotById(appointment.SlotId);
                if (slot == null) continue;

                responses.Add(new AppointmentResponse
                {
                    AppointmentId = appointment.Id,
                    DoctorId = slot.DoctorId,
                    StartTime = slot.StartTime,
                    EndTime = slot.EndTime,
                    PatientId = appointment.PatientId,
                    CreatedAt = appointment.CreatedAt
                });
            }

            return responses;
        }

        public async Task<IEnumerable<AppointmentResponse>> GetAppointmentsForDoctor(Guid doctorId)
        {
            var list = await _repo.GetByDoctorId(doctorId);

            var responses = new List<AppointmentResponse>();

            foreach (var appointment in list)
            {
                var slot = await _doctorClient.GetSlotById(appointment.SlotId);
                if (slot == null) continue;

                responses.Add(new AppointmentResponse
                {
                    AppointmentId = appointment.Id,
                    DoctorId = slot.DoctorId,
                    StartTime = slot.StartTime,
                    EndTime = slot.EndTime,
                    PatientId = appointment.PatientId,
                    CreatedAt = appointment.CreatedAt
                });
            }

            return responses;
        }

    }

}
