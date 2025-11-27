namespace VideoService.Services.Providers
{
    public interface IVideoProvider
    {
        Task<(string roomName, string externalUrl)> CreateRoom(Guid apptId, bool waitingRoomEnabled);
    }

}
