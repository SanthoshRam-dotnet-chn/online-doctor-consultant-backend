namespace VideoService.Domain.Entities
{
    public class Appointment
    {
        public Guid AppointmentId { get; set; }
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
        public string Status { get; set; } = "Scheduled";
        public DateTime? EndedAt { get; set; }
    }
}
