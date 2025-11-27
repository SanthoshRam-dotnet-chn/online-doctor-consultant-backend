using System;
using System.Collections.Generic;
using System.Net.Mail;

namespace PatientService.Domain.Entities
{
    public class PatientRecord
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public Guid PatientId { get; set; }              // unique per patient (one record per patient)
        public string? DoctorNotes { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }
}
