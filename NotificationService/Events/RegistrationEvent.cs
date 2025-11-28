namespace NotificationService.Events
{
    public class RegistrationEvent
    {
        public string Name { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime RegistrationDate { get; set; } = DateTime.Now;
    }

}
