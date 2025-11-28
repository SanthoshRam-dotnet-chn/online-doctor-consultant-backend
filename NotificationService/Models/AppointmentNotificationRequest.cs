namespace NotificationService.Models
{
    public class AppointmentNotificationRequest
    {
        public string PatientEmail { get; set; } = null!;
        public string MeetingLink { get; set; } = null!;
        public string Purpose { get; set; } = null!;
        public DateTime AppointmentTime { get; set; }
        public string DoctorName { get; set; } = null!;
    }
}
