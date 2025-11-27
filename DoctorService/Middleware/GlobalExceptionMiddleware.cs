using DoctorService.Exceptions;
using System.Net;
using System.Text.Json;

namespace DoctorService.Middleware
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
                await HandleExceptionAsync(context, ex);
            }
        }

        private static Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode statusCode;
            string message;

            switch (exception)
            {
                case BadRequestException badRequestEx:
                    statusCode = HttpStatusCode.BadRequest;
                    message = badRequestEx.Message;
                    break;

                case PrescriptionNotFoundException prescriptionNotFoundEx:
                    statusCode = HttpStatusCode.NotFound;
                    message = prescriptionNotFoundEx.Message;
                    break;

                case SlotAlreadyExistsException slotExistsEx:
                    statusCode = HttpStatusCode.Conflict;
                    message = slotExistsEx.Message;
                    break;

                case SlotNotFoundException slotNotFoundEx:
                    statusCode = HttpStatusCode.NotFound;
                    message = slotNotFoundEx.Message;
                    break;

                default:
                    statusCode = HttpStatusCode.InternalServerError;
                    message = "An unexpected error occurred. Please try again later.";
                    break;
            }

            var result = JsonSerializer.Serialize(new { error = message });
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)statusCode;

            return context.Response.WriteAsync(result);
        }
    }
}
