namespace VideoService.Domain.Entities
{
    public class VideoRoom
    {
        public Guid VideoRoomId { get; set; }
        public Guid AppointmentId { get; set; }
        public string Provider { get; set; } = "JITSI";
        public string RoomName { get; set; } = "";
        public string ExternalRoomUrl { get; set; }
        public bool IsWaitingRoomEnabled { get; set; } = true;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
