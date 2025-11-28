using DoctorService.Interfaces;
using DoctorService.Models;
using Microsoft.AspNetCore.Mvc;

namespace DoctorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PrescriptionController : ControllerBase
    {
        private readonly IPrescriptionService _service;

        public PrescriptionController(IPrescriptionService service)
        {
            _service = service;
        }

        [HttpGet("{prescriptionId}")]
        public async Task<IActionResult> GetPrescriptionById(Guid prescriptionId)
        {
            var prescription = await _service.GetPrescriptionByIdAsync(prescriptionId);
            return Ok(prescription);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePrescription([FromBody] Prescription prescription)
        {
            var created = await _service.CreatePrescriptionAsync(prescription);
            return Ok(created);
        }
    }
}
