using System.Net;
using System.Text.Json;
using AuthService.src.AuthService.Application.Exceptions;

namespace AuthService.src.AuthService.Api.Middleware
{
    public class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        public GlobalExceptionMiddleware(RequestDelegate next, ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Unhandled exception occurred");

                context.Response.ContentType = "application/json";
                context.Response.StatusCode = ex switch
                {
                    EmailAlreadyExistsException => StatusCodes.Status409Conflict,
                    InvalidCredentialsException => StatusCodes.Status401Unauthorized,
                    UserNotFoundException => StatusCodes.Status404NotFound,
                    _ => StatusCodes.Status400BadRequest
                };

                var response = new
                {
                    success = false,
                    message = ex.Message
                };

                await context.Response.WriteAsync(JsonSerializer.Serialize(response));
            }
        }
    }
}
