using Microsoft.AspNetCore.Mvc;
using PatientService.Domain.Dtos;
using PatientService.Repositories;
using PatientService.Services;

namespace PatientService.Controllers
{
    [ApiController]
    [Route("api/patient/records")]
    public class PatientRecordsController : ControllerBase
    {
        private readonly IPatientRecordService _service;
        private readonly IWebHostEnvironment _env;
        private readonly IConfiguration _config;

        public PatientRecordsController(IPatientRecordService service, IWebHostEnvironment env, IConfiguration config)
        {
            _service = service;
            _env = env;
            _config = config;
        }

        // Create or update doctor's notes for a patient (one record per patient)
        [HttpPost]
        public async Task<IActionResult> CreateOrUpdate([FromBody] CreateOrUpdateRecordRequest request)
        {
            var result = await _service.CreateOrUpdateRecordAsync(request);
            return Ok(result);
        }

        // Get patient record
        [HttpGet("{patientId:guid}")]
        public async Task<IActionResult> GetRecord(Guid patientId)
        {
            var record = await _service.GetRecordByPatientIdAsync(patientId);
            if (record == null)
                return NotFound(new { success = false, message = "Record not found." });

            return Ok(record);
        }

        // Upload an attachment (multipart/form-data)
        // form-data: file (file)
        [HttpPost("{patientId:guid}/attachments")]
        [RequestSizeLimit(50_000_000)] // 50 MB limit - adjust as needed
        public async Task<IActionResult> UploadAttachment(Guid patientId, IFormFile file)
        {
            if (file == null) return BadRequest(new { success = false, message = "No file uploaded." });

            // get uploads path from config or fallback
            var configured = _config["Uploads:PatientRecords"];
            string uploadsRoot;
            if (!string.IsNullOrWhiteSpace(configured))
            {
                uploadsRoot = configured!;
                // if configured path is relative, make it absolute under content root
                if (!Path.IsPathRooted(uploadsRoot))
                    uploadsRoot = Path.Combine(_env.ContentRootPath, uploadsRoot);
            }
            else
            {
                uploadsRoot = Path.Combine(_env.ContentRootPath, "uploads", "patient-records");
            }

            var attachment = await _service.AddAttachmentAsync(patientId, file, uploadsRoot);
            return Ok(attachment);
        }

        // Download attachment
        [HttpGet("{patientId:guid}/attachments/{attachmentId:guid}/download")]
        public async Task<IActionResult> Download(Guid patientId, Guid attachmentId)
        {
            // fetch via repository to get stored filename
            // small direct access - you can add to service if you prefer
            var record = await _service.GetRecordByPatientIdAsync(patientId);
            if (record == null) return NotFound();

            var attach = record.Attachments.FirstOrDefault(a => a.Id == attachmentId);
            if (attach == null || attach.IsDeleted) return NotFound();

            var configured = _config["Uploads:PatientRecords"];
            string uploadsRoot;
            if (!string.IsNullOrWhiteSpace(configured))
            {
                uploadsRoot = configured!;
                if (!Path.IsPathRooted(uploadsRoot))
                    uploadsRoot = Path.Combine(_env.ContentRootPath, uploadsRoot);
            }
            else
            {
                uploadsRoot = Path.Combine(_env.ContentRootPath, "uploads", "patient-records");
            }

            // get stored filename from DB (requires repository access). We'll use repository here quickly:
            // (if you want to avoid repository in controller, move this logic to service)
            var repo = HttpContext.RequestServices.GetRequiredService<IPatientRecordRepository>();
            var attachmentEntity = await repo.GetAttachmentByIdAsync(attachmentId);
            if (attachmentEntity == null || attachmentEntity.IsDeleted) return NotFound();

            var filePath = Path.Combine(uploadsRoot, attachmentEntity.StoredFileName);
            if (!System.IO.File.Exists(filePath)) return NotFound(new { success = false, message = "File not found on disk." });

            var stream = System.IO.File.OpenRead(filePath);
            return File(stream, attachmentEntity.ContentType, attachmentEntity.FileName);
        }

        // Soft-delete attachment
        [HttpDelete("{patientId:guid}/attachments/{attachmentId:guid}")]
        public async Task<IActionResult> SoftDeleteAttachment(Guid patientId, Guid attachmentId)
        {
            try
            {
                await _service.SoftDeleteAttachmentAsync(patientId, attachmentId);
                return Ok(new { success = true });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }
    }
}
