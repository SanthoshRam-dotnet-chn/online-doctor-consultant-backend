namespace NotificationService.Events
{
    public class MeetingCancelledEvent
    {
        public string PatientName { get; set; } = null!;
        public string PatientEmail { get; set; } = null!;
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; } = null!;
        public string Reason { get; set; } = null!;
    }
}
