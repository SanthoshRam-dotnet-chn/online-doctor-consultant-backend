namespace PatientService.Domain.Entities
{
    public class AppointmentResponse
    {
        public Guid AppointmentId { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Guid PatientId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
