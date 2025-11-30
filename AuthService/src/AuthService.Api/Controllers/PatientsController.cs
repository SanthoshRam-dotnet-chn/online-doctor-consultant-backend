using AuthService.src.AuthService.Application.Exceptions;
using AuthService.src.AuthService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.src.AuthService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PatientsController : ControllerBase
    {

        private readonly IAuthService _service;

        public PatientsController(IAuthService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllPatients() =>
            Ok(await _service.GetAllPatientsAsync());

        [Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetPatientById(Guid id)
        {
            var patient = await _service.GetPatientByIdAsync(id);

            if (patient == null)
                throw new UserNotFoundException("Patient Not Found");

            return Ok(patient);
        }


        [HttpGet("test")]
        public async Task<IActionResult> Test() => Ok("Patient Service is working!");
    }
}
