using System.Net;
using System.Net.Mail;
using System.Text.Json;

namespace NotificationService.Middleware
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
                _logger.LogError(ex, $"Unhandled exception occurred: {ex.Message}");
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            HttpStatusCode status;
            string message;

            switch (exception)
            {
                case SmtpException:
                    status = HttpStatusCode.BadGateway;
                    message = "Email service is temporarily unavailable. Please try again later.";
                    break;
                case FileNotFoundException:
                    status = HttpStatusCode.NotFound;
                    message = "Email template not found.";
                    break;
                case FormatException:
                    status = HttpStatusCode.BadRequest;
                    message = "Invalid email address.";
                    break;
                case ArgumentNullException:
                    status = HttpStatusCode.BadRequest;
                    message = "Missing required data.";
                    break;
                default:
                    status = HttpStatusCode.InternalServerError;
                    message = "An unexpected error occurred. Please contact support.";
                    break;
            }

            var response = new
            {
                StatusCode = (int)status,
                Error = message,
                Details = exception.Message
            };

            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)status;

            var json = JsonSerializer.Serialize(response);
            return context.Response.WriteAsync(json);
        }
    }
}
