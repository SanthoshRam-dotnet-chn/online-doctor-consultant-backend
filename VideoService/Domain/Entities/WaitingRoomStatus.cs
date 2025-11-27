namespace VideoService.Domain.Entities
{
    public class WaitingRoomStatus
    {
        public Guid WaitingRoomStatusId { get; set; }
        public Guid AppointmentId { get; set; }
        public Guid PatientId { get; set; }
        public string Status { get; set; } = "Pending"; // Pending | Allowed | Denied
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;
    }
}
