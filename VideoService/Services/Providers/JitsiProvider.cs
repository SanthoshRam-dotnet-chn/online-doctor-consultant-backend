namespace VideoService.Services.Providers
{
    public class JitsiProvider : IVideoProvider
    {
        private readonly IConfiguration _cfg;

        public JitsiProvider(IConfiguration cfg)
        {
            _cfg = cfg;
        }

        public Task<(string roomName, string externalUrl)> CreateRoom(Guid apptId, bool waitingRoomEnabled)
        {
            var baseUrl = _cfg["VideoService:JitsiBaseUrl"].TrimEnd('/');
            var roomName = $"appt-{apptId:N}";
            var url = $"{baseUrl}/{roomName}";

            return Task.FromResult((roomName, url));
        }
    }
}
