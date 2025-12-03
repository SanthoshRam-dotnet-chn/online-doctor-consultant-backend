namespace PatientService.Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid SlotId { get; set; }
        public Guid DoctorId { get; set; }
        public Guid PatientId { get; set; }
        public string Reason { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public string status { get; set; } = "scheduled";
    }

}
