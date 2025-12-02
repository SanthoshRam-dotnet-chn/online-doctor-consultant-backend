using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using PatientService.Domain.Entities;
using PatientService.Services;

namespace PatientService.Controllers
{
    [ApiController]
    [Route("api/patient")]
    public class PatientController : ControllerBase
    {
        private readonly IPatientService _service;

        public PatientController(IPatientService service)
        {
            _service = service;
        }

        [HttpPost("appointments")]
        //[Authorize]
        public async Task<IActionResult> BookAppointment([FromBody] BookAppointmentRequest request)
        {
            try
            {
                var result = await _service.BookAppointment(request);
                return Ok(result);
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message
                });
            }
        }

        [HttpGet("appointments/{appointmentId}")]
        [Authorize]
        public async Task<IActionResult> GetAppointment(Guid appointmentId)
        {
            var result = await _service.GetAppointment(appointmentId);
            if (result == null)
            {
                return NotFound(new
                {
                    success = false,
                    message = "Appointment not found."
                });
            }

            return Ok(result);
        }

        [HttpGet("appointments/patient/{patientId}")]
        [Authorize]
        public async Task<IActionResult> GetAppointmentsForPatient(Guid patientId)
        {
            var result = await _service.GetAppointmentsForPatient(patientId);
            return Ok(result);
        }

        [HttpGet("appointments/doctor/{doctorId}")]
        public async Task<IActionResult> GetAppointmentsForDoctor(Guid doctorId)
        {
            var result = await _service.GetAppointmentsForDoctor(doctorId);
            return Ok(result);
        }

        [HttpGet("test")]
        public async Task<IActionResult> Test() => Ok("Patient Service is working!");
    }

}
