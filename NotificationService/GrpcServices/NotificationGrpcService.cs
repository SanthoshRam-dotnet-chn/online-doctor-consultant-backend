using Grpc.Core;
using NotificationService.Events;
using NotificationService.Protos;
using NotificationService.Services;

namespace NotificationService.GrpcServices
{
    public class NotificationGrpcService : NotificationGrpc.NotificationGrpcBase
    {
        private readonly RegistrationNotificationService _registrationNotifier;

        public NotificationGrpcService(RegistrationNotificationService registrationNotifier)
        {
            _registrationNotifier = registrationNotifier;
        }

        public override async Task<RegistrationResponse> SendRegistrationNotification(
            RegistrationRequest request,
            ServerCallContext context
        )
        {
            try
            {
                var evt = new RegistrationEvent
                {
                    Name = request.Name,
                    Email = request.Email,
                    RegistrationDate = DateTime.Now,
                };

                await _registrationNotifier.HandleAsync(evt);

                return new RegistrationResponse
                {
                    Success = true,
                    Message = "Registration notification sent.",
                };
            }
            catch (Exception ex)
            {
                return new RegistrationResponse
                {
                    Success = false,
                    Message = $"Failed: {ex.Message}",
                };
            }
        }
    }
}
