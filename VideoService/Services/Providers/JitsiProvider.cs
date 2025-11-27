namespace VideoService.Services.Providers
{
    public class JitsiProvider : IVideoProvider
    {
        private readonly IConfiguration _cfg;
        public JitsiProvider(IConfiguration cfg) { _cfg = cfg; }

        public Task<(string roomName, string externalUrl)> CreateRoomAsync(Guid appointmentId, bool waitingRoom)
        {
            // Use a deterministic, non-guessable room name pattern
            var roomName = $"appt-{appointmentId:N}"; // e.g. appt-<guid without dashes>
            var baseUrl = _cfg["VideoService:JitsiBaseUrl"]?.TrimEnd('/') ?? "https://meet.jit.si";
            // You can add parameters (e.g. lobby/waiting settings if using self-hosted Jitsi with JWT)
            var externalUrl = $"{baseUrl}/{roomName}";
            return Task.FromResult((roomName, externalUrl));
        }
    }
}
