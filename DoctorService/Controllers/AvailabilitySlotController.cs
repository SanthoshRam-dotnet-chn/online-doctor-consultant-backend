using DoctorService.Exceptions;
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

        [HttpPatch("book/{id}")]
        public async Task<IActionResult> MarkAsBooked(Guid id)
        {
            try
            {
                await _service.MarkSlotAsBookedAsync(id);
                return Ok(new { success = true, message = "Slot marked as booked." });
            }
            catch (SlotNotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
            catch (Exception ex)
            {
                return BadRequest(new { success = false, message = ex.Message });
            }
        }

        [HttpGet("slot/{id}")]
        public async Task<IActionResult> GetSlotById(Guid id)
        {
            try
            {
                var slot = await _service.GetSlotByIdAsync(id);
                return Ok(slot);
            }
            catch (SlotNotFoundException ex)
            {
                return NotFound(new { success = false, message = ex.Message });
            }
        }

    }
}
