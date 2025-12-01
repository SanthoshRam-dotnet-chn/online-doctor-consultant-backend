namespace NotificationService.Events
{
    public class MeetingConfirmedEvent
    {
        public string PatientName { get; set; } = null!;
        public string PatientEmail { get; set; } = null!;
        public string MeetingLink { get; set; } = null!;
        public string Purpose { get; set; } = null!;
        public DateTime AppointmentDate { get; set; }
        public TimeSpan StartTime { get; set; }
        public TimeSpan EndTime { get; set; }
    }

}
