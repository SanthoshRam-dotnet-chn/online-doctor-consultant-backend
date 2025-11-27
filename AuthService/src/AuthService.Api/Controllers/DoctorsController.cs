using AuthService.src.AuthService.Application.Exceptions;
using AuthService.src.AuthService.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AuthService.src.AuthService.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DoctorsController : ControllerBase
    {
        private readonly IAuthService _service;

        public DoctorsController(IAuthService service)
        {
            _service = service;
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> GetAllDoctors() => Ok(await _service.GetAllDoctorsAsync());

        //[Authorize]
        [HttpGet("{id:guid}")]
        public async Task<IActionResult> GetDoctorById(Guid id)
        {
            var doctor = await _service.GetDoctorByIdAsync(id);

            if (doctor == null)
             throw new UserNotFoundException("Doctor Not Found");

            return Ok(doctor);
        }



        [HttpGet("test")]
        public async Task<IActionResult> Test() => Ok("Doctor Service is working!");
    }
}
