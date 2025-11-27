using DoctorService.Models;

namespace DoctorService.Interfaces
{
    public interface IAvailabilitySlotService
    {
        Task<IEnumerable<AvailabilitySlot>> GetSlotsByDoctorAsync(Guid doctorId);
        Task<IEnumerable<AvailabilitySlot>> GetSlotsByDoctorAndDateAsync(Guid doctorId, DateTime date);
        Task<AvailabilitySlot> CreateSlotAsync(AvailabilitySlot slot);
        Task DeleteSlotAsync(Guid id);
    }
}
