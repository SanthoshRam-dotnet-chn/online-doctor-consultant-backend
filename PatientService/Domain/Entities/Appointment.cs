namespace PatientService.Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid SlotId { get; set; }
        public Guid PatientId { get; set; }
        public DateTime CreatedAt { get; set; }
    }

}
