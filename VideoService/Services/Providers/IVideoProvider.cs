namespace VideoService.Services.Providers
{
    public interface IVideoProvider
    {
        Task<(string roomName, string externalUrl)> CreateRoomAsync(Guid appointmentId, bool waitingRoom);
        // TODO: Optionally: Task CloseRoomAsync(string roomName);
    }
}
