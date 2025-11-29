namespace NotificationService.Events
{
    public class MeetingEndedEvent
    {
        public string PatientName { get; set; } = null!;
        public string PatientEmail { get; set; } = null!;
        public DateTime AppointmentDate { get; set; }
        public string AppointmentTime { get; set; } = null!;
        public string? FeedbackLink { get; set; }
    }

}
