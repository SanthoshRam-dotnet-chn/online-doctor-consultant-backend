using DoctorService.Exceptions;
using DoctorService.Interfaces;
using DoctorService.Models;

namespace DoctorService.Services
{
    public class AvailabilitySlotService : IAvailabilitySlotService
    {
        private readonly IAvailabilitySlotRepository _repository;

        public AvailabilitySlotService(IAvailabilitySlotRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<AvailabilitySlot>> GetAllSlotsAsync()
        {
            return await _repository.GetAllSlotsAsync();
        }

        public async Task<AvailabilitySlot> CreateSlotAsync(AvailabilitySlot slot)
        {
            bool exists = await _repository.SlotExistsAsync(slot.DoctorId, slot.StartTime, slot.EndTime);
            if (exists)
                throw new SlotAlreadyExistsException("The availability slot overlaps with an existing slot.");
            slot.Id = Guid.NewGuid();
            slot.IsAvailable = true;

            await _repository.AddSlotAsync(slot);
            return slot;
        }

        public async Task DeleteSlotAsync(Guid id)
        {
            var existingSlot = await _repository.GetSlotByIdAsync(id);
            if (existingSlot == null)
                throw new SlotNotFoundException("Slot not found.");

            await _repository.DeleteSlotAsync(id);
        }

        public async Task<IEnumerable<AvailabilitySlot>> GetSlotsByDoctorAsync(Guid doctorId)
        {
            var slots = await _repository.GetSlotsByDoctorAsync(doctorId);
            return slots.Where(s => s.StartTime >= DateTime.UtcNow).OrderBy(s => s.StartTime);
        }

        public async Task<IEnumerable<AvailabilitySlot>> GetSlotsByDoctorAndDateAsync(Guid doctorId, DateTime date)
        {
            var slots = await _repository.GetSlotsByDoctorAndDateAsync(doctorId, date);
            return slots.OrderBy(s => s.StartTime);
        }

        public async Task MarkSlotAsBookedAsync(Guid id)
        {
            var slot = await _repository.GetSlotByIdAsync(id);
            if (slot == null)
                throw new SlotNotFoundException("Slot not found.");

            if (!slot.IsAvailable)
                throw new Exception("Slot is already booked.");

            await _repository.MarkAsBookedAsync(id);
        }

        public async Task<AvailabilitySlot> GetSlotByIdAsync(Guid id)
        {
            var slot = await _repository.GetSlotByIdAsync(id);
            if (slot == null)
                throw new SlotNotFoundException("Slot not found.");

            return slot;
        }


    }
}
