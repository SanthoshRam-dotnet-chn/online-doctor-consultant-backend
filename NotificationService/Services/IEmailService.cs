namespace NotificationService.Services
{
    public interface IEmailService
    {
        Task SendEmail(string receptor, string subject, string body);
    }
}
