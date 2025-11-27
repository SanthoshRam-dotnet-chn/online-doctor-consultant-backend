using PatientService.Domain.Dtos;

namespace PatientService.Services
{
    public class DoctorServiceClient
    {
        private readonly HttpClient _http;

        public DoctorServiceClient(HttpClient http)
        {
            _http = http;
        }

        public async Task<DoctorSlotDto?> GetSlotById(Guid slotId)
        {
            var response = await _http.GetAsync($"/api/AvailabilitySlot/slot/{slotId}");

            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<DoctorSlotDto>();
        }

        public async Task<bool> MarkSlotAsBooked(Guid slotId)
        {
            var response = await _http.PutAsync($"/api/AvailabilitySlot/{slotId}/book", null);
            return response.IsSuccessStatusCode;
        }
    }

}
