using DoctorService.Models;

namespace DoctorService.Interfaces
{
    public interface IAvailabilitySlotRepository
    {
        Task<IEnumerable<AvailabilitySlot>> GetSlotsByDoctorAsync(Guid doctorId);
        Task<IEnumerable<AvailabilitySlot>> GetSlotsByDoctorAndDateAsync(Guid doctorId, DateTime date);
        Task AddSlotAsync(AvailabilitySlot slot);
        Task DeleteSlotAsync(Guid id);
        Task<AvailabilitySlot> GetSlotByIdAsync(Guid id);
        Task<bool> SlotExistsAsync(Guid doctorId, DateTime startTime, DateTime endTime);
        Task<IEnumerable<AvailabilitySlot>> GetAllSlotsAsync();
        Task<bool> MarkAsBookedAsync(Guid id);
    }
}
