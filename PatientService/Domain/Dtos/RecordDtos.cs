namespace PatientService.Domain.Dtos
{
    public class PatientRecordResponse
    {
        public Guid Id { get; set; }
        public Guid PatientId { get; set; }
        public string? DoctorNotes { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public IEnumerable<AttachmentResponse> Attachments { get; set; } = Array.Empty<AttachmentResponse>();
    }

    public class AttachmentResponse
    {
        public Guid Id { get; set; }
        public string FileName { get; set; } = null!;
        public string DownloadUrl { get; set; } = null!; 
        public string ContentType { get; set; } = null!;
        public long Size { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }

    public class CreateOrUpdateRecordRequest
    {
        public Guid PatientId { get; set; }
        public string? DoctorNotes { get; set; }
    }
}
