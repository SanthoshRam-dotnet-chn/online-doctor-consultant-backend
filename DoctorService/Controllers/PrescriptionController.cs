using DoctorService.Interfaces;
using DoctorService.Models;
using Microsoft.AspNetCore.Authorization;
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
        [Authorize]
        public async Task<IActionResult> GetPrescriptionById(Guid prescriptionId)
        {
            var prescription = await _service.GetPrescriptionByIdAsync(prescriptionId);
            return Ok(prescription);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreatePrescription([FromBody] Prescription prescription)
        {
            var created = await _service.CreatePrescriptionAsync(prescription);
            return Ok(created);
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test() => Ok("Prescription Service is working!");
    }
}
