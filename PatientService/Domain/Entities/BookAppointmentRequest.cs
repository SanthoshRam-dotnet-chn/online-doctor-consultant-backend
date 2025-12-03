namespace PatientService.Domain.Entities
{
    public class BookAppointmentRequest
    {
        public Guid SlotId { get; set; }
        public Guid PatientId { get; set; }

        public string Reason { get; set; } = string.Empty;
    }

}
