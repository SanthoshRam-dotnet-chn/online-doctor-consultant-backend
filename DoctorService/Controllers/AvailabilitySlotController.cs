using DoctorService.Interfaces;
using DoctorService.Models;
using Microsoft.AspNetCore.Mvc;

namespace DoctorService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AvailabilitySlotController : ControllerBase
    {
        private readonly IAvailabilitySlotService _service;

        public AvailabilitySlotController(IAvailabilitySlotService service)
        {
            _service = service;
        }

        [HttpGet("{doctorId}")]
        public async Task<IActionResult> GetSlotsByDoctor(Guid doctorId)
        {
            var slots = await _service.GetSlotsByDoctorAsync(doctorId);
            return Ok(slots);
        }

        [HttpGet("{doctorId}/{date}")]
        public async Task<IActionResult> GetSlotsByDoctorAndDate(Guid doctorId, DateTime date)
        {
            var slots = await _service.GetSlotsByDoctorAndDateAsync(doctorId, date);
            return Ok(slots);
        }

        [HttpPost]
        public async Task<IActionResult> CreateSlot([FromBody] AvailabilitySlot slot)
        {
            var created = await _service.CreateSlotAsync(slot);
            return Ok(created);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSlot(Guid id)
        {
            await _service.DeleteSlotAsync(id);
            return NoContent();
        }
    }
}
