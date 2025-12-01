using PatientService.Domain.Dtos;
using PatientService.Domain.Entities;
using PatientService.Repositories;

namespace PatientService.Services
{
    public class PatientRecordService : IPatientRecordService
    {
        private readonly IPatientRecordRepository _repo;
        private readonly ILogger<PatientRecordService> _logger;

        public PatientRecordService(IPatientRecordRepository repo, ILogger<PatientRecordService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        public async Task<PatientRecordResponse?> GetRecordByPatientIdAsync(Guid patientId)
        {
            var record = await _repo.GetByPatientIdAsync(patientId);
            if (record == null) return null;

            return MapToResponse(record);
        }

        public async Task<PatientRecordResponse> CreateOrUpdateRecordAsync(CreateOrUpdateRecordRequest request)
        {
            var existing = await _repo.GetByPatientIdAsync(request.PatientId);

            if (existing == null)
            {
                var newRecord = new PatientRecord
                {
                    PatientId = request.PatientId,
                    DoctorNotes = request.DoctorNotes,
                    CreatedAt = DateTime.UtcNow
                };

                var created = await _repo.CreateAsync(newRecord);
                return MapToResponse(created);
            }

            existing.DoctorNotes = request.DoctorNotes;
            existing.UpdatedAt = DateTime.UtcNow;

            var updated = await _repo.UpdateAsync(existing);
            return MapToResponse(updated);
        }

        // uploadsRoot is a directory path on disk where files will be stored (absolute or relative to content root)
        public async Task<AttachmentResponse> AddAttachmentAsync(Guid patientId, IFormFile file, string uploadsRoot)
        {
            if (file == null || file.Length == 0)
                throw new ArgumentException("File is empty.");

            var record = await _repo.GetByPatientIdAsync(patientId);

            if (record == null)
            {
                // create an empty record if not exists (since one record per patient)
                record = new PatientRecord
                {
                    PatientId = patientId,
                    CreatedAt = DateTime.UtcNow
                };
                record = await _repo.CreateAsync(record);
            }

            // ensure directory exists
            Directory.CreateDirectory(uploadsRoot);

            var originalFileName = Path.GetFileName(file.FileName);
            var storedFileName = $"{Guid.NewGuid()}{Path.GetExtension(originalFileName)}";
            var filePath = Path.Combine(uploadsRoot, storedFileName);

            // save file to disk
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var attachment = new Attachment
            {
                PatientRecordId = record.Id,
                FileName = originalFileName,
                StoredFileName = storedFileName,
                ContentType = file.ContentType ?? "application/octet-stream",
                Size = file.Length,
                CreatedAt = DateTime.UtcNow
            };

            var created = await _repo.AddAttachmentAsync(attachment);

            return new AttachmentResponse
            {
                Id = created.Id,
                FileName = created.FileName,
                ContentType = created.ContentType,
                Size = created.Size,
                CreatedAt = created.CreatedAt,
                IsDeleted = created.IsDeleted,
                DownloadUrl = $"/api/patient/records/{patientId}/attachments/{created.Id}/download"
            };
        }

        public async Task SoftDeleteAttachmentAsync(Guid patientId, Guid attachmentId)
        {
            var attachment = await _repo.GetAttachmentByIdAsync(attachmentId);
            if (attachment == null)
                throw new InvalidOperationException("Attachment not found.");

            // ensure it belongs to the patient's record
            var record = await _repo.GetByPatientIdAsync(patientId);
            if (record == null || attachment.PatientRecordId != record.Id)
                throw new InvalidOperationException("Attachment does not belong to this patient's record.");

            await _repo.SoftDeleteAttachmentAsync(attachment);
        }

        private PatientRecordResponse MapToResponse(PatientRecord r)
        {
            return new PatientRecordResponse
            {
                Id = r.Id,
                PatientId = r.PatientId,
                DoctorNotes = r.DoctorNotes,
                CreatedAt = r.CreatedAt,
                UpdatedAt = r.UpdatedAt,
                Attachments = r.Attachments?
                    .Where(a => !a.IsDeleted) // don't show soft-deleted attachments
                    .Select(a => new AttachmentResponse
                    {
                        Id = a.Id,
                        FileName = a.FileName,
                        ContentType = a.ContentType,
                        Size = a.Size,
                        CreatedAt = a.CreatedAt,
                        IsDeleted = a.IsDeleted,
                        DownloadUrl = $"/api/patient/records/{r.PatientId}/attachments/{a.Id}/download"
                    })
                    .ToList()
                    ?? new List<AttachmentResponse>()
            };
        }
    }
}
