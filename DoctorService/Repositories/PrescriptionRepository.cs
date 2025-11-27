using DoctorService.Data;
using DoctorService.Interfaces;
using DoctorService.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorService.Repositories
{
    public class PrescriptionRepository : IPrescriptionRepository
    {
        private readonly AppDbContext _context;

        public PrescriptionRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddPrescriptionAsync(Prescription prescription)
        {
            await _context.Prescriptions.AddAsync(prescription);
            await _context.SaveChangesAsync();
        }

        public async Task<Prescription> GetPrescriptionByIdAsync(Guid prescriptionId)
        {
            return await _context.Prescriptions.FindAsync(prescriptionId);
        }
    }
}
