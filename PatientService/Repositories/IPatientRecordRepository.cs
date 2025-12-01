using PatientService.Domain.Entities;

namespace PatientService.Repositories
{
    public interface IPatientRecordRepository
    {
        Task<PatientRecord?> GetByPatientIdAsync(Guid patientId);
        Task<PatientRecord> CreateAsync(PatientRecord record);
        Task<PatientRecord> UpdateAsync(PatientRecord record);
        Task<Attachment> AddAttachmentAsync(Attachment attachment);
        Task<Attachment?> GetAttachmentByIdAsync(Guid attachmentId);
        Task SoftDeleteAttachmentAsync(Attachment attachment);
    }
}
