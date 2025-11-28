using Microsoft.EntityFrameworkCore;
using PatientService.Data;
using PatientService.Domain.Entities;

namespace PatientService.Repositories
{
    public class PatientRecordRepository : IPatientRecordRepository
    {
        private readonly PatientDbContext _context;

        public PatientRecordRepository(PatientDbContext context)
        {
            _context = context;
        }

        public async Task<PatientRecord?> GetByPatientIdAsync(Guid patientId)
        {
            return await _context.PatientRecords
                .Include(r => r.Attachments)
                .FirstOrDefaultAsync(r => r.PatientId == patientId);
        }

        public async Task<PatientRecord> CreateAsync(PatientRecord record)
        {
            _context.PatientRecords.Add(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<PatientRecord> UpdateAsync(PatientRecord record)
        {
            _context.PatientRecords.Update(record);
            await _context.SaveChangesAsync();
            return record;
        }

        public async Task<Attachment> AddAttachmentAsync(Attachment attachment)
        {
            _context.Attachments.Add(attachment);
            await _context.SaveChangesAsync();
            return attachment;
        }

        public async Task<Attachment?> GetAttachmentByIdAsync(Guid attachmentId)
        {
            return await _context.Attachments.FirstOrDefaultAsync(a => a.Id == attachmentId);
        }

        public async Task SoftDeleteAttachmentAsync(Attachment attachment)
        {
            attachment.IsDeleted = true;
            attachment.DeletedAt = DateTime.UtcNow;
            _context.Attachments.Update(attachment);
            await _context.SaveChangesAsync();
        }
    }
}