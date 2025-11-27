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
            var response = await _http.GetAsync($"/mock/c542fc19-eed5-4e14-a20d-9931f8e431c9");

            if (!response.IsSuccessStatusCode) return null;

            return await response.Content.ReadFromJsonAsync<DoctorSlotDto>();
        }

        public async Task<bool> MarkSlotAsBooked(Guid slotId)
        {
            var response = await _http.PutAsync($"/mock/e42e93f2-8802-4717-88ba-0d1cc2b035c7", null);
            return response.IsSuccessStatusCode;
        }
    }

}
