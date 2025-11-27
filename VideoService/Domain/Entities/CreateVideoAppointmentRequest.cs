namespace VideoService.Domain.Entities
{
    public class CreateVideoAppointmentRequest
    {

        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public DateTime ScheduledStart { get; set; }
        public DateTime ScheduledEnd { get; set; }
        public bool WaitingRoom { get; set; } = true;
    }
}