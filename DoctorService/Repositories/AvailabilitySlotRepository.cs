using DoctorService.Data;
using DoctorService.Interfaces;
using DoctorService.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorService.Repositories
{
    public class AvailabilitySlotRepository : IAvailabilitySlotRepository
    {
        private readonly AppDbContext _context;

        public AvailabilitySlotRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddSlotAsync(AvailabilitySlot slot)
        {
            await _context.AvailabilitySlots.AddAsync(slot);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteSlotAsync(Guid id)
        {
            var slot = await _context.AvailabilitySlots.FindAsync(id);
            if (slot != null)
            {
                _context.AvailabilitySlots.Remove(slot);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<IEnumerable<AvailabilitySlot>> GetSlotsByDoctorAsync(Guid doctorId)
        {
            return await _context.AvailabilitySlots
                .Where(s => s.DoctorId == doctorId)
                .ToListAsync();
        }

        public async Task<IEnumerable<AvailabilitySlot>> GetSlotsByDoctorAndDateAsync(Guid doctorId, DateTime date)
        {
            return await _context.AvailabilitySlots
                .Where(s => s.DoctorId == doctorId && s.StartTime.Date == date.Date)
                .ToListAsync();
        }

        public async Task<AvailabilitySlot> GetSlotByIdAsync(Guid id)
        {
            return await _context.AvailabilitySlots.FindAsync(id);
        }

        public async Task<bool> SlotExistsAsync(Guid doctorId, DateTime startTime, DateTime endTime)
        {
            return await _context.AvailabilitySlots.AnyAsync(s =>
                s.DoctorId == doctorId &&
                ((startTime >= s.StartTime && startTime < s.EndTime) ||
                 (endTime > s.StartTime && endTime <= s.EndTime) ||
                 (startTime <= s.StartTime && endTime >= s.EndTime))
            );
        }
    }
}
