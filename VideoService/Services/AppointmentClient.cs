using System.Text.Json;
using VideoService.DTOs;

namespace VideoService.Services
{
    public class AppointmentClient : IAppointmentClient
    {
        private readonly HttpClient _http;

        public AppointmentClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<AppointmentDto?> GetAppointment(Guid appointmentId)
        {
            var res = await _http.GetAsync($"/api/patient/appointments/{appointmentId}");

            if (!res.IsSuccessStatusCode)
                return null;

            var json = await res.Content.ReadAsStringAsync();
            var dto = JsonSerializer.Deserialize<AppointmentDto>(
                json,
                new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                }
            );

            return dto;
        }
    }
}
