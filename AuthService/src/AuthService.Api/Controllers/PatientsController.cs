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

        [HttpGet("test")]
        public async Task<IActionResult> Test() => Ok("Patient Service is working!");
    }
}
