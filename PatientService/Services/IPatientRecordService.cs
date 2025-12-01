using PatientService.Domain.Dtos;

namespace PatientService.Services
{
    public interface IPatientRecordService
    {
        Task<PatientRecordResponse?> GetRecordByPatientIdAsync(Guid patientId);
        Task<PatientRecordResponse> CreateOrUpdateRecordAsync(CreateOrUpdateRecordRequest request);
        Task<AttachmentResponse> AddAttachmentAsync(Guid patientId, IFormFile file, string uploadsRoot);
        Task SoftDeleteAttachmentAsync(Guid patientId, Guid attachmentId);
    }
}
