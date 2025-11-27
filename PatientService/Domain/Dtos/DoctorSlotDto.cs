namespace PatientService.Domain.Dtos
{
    public class DoctorSlotDto
    {
        public Guid Id { get; set; }
        public Guid DoctorId { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public string Status { get; set; }
    }

}
