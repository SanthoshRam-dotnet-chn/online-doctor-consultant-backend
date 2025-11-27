using System;

namespace PatientService.Domain.Entities
{
    public class Attachment
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PatientRecordId { get; set; }
        public string FileName { get; set; } = null!;
        public string StoredFileName { get; set; } = null!; // physical filename on disk
        public string ContentType { get; set; } = null!;
        public long Size { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        // soft delete
        public bool IsDeleted { get; set; } = false;
        public DateTime? DeletedAt { get; set; }

        // navigation
        public PatientRecord? PatientRecord { get; set; }
    }
}
