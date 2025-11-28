using DoctorService.Models;
using Microsoft.EntityFrameworkCore;

namespace DoctorService.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<AvailabilitySlot> AvailabilitySlots { get; set; }
        public DbSet<Prescription> Prescriptions { get; set; }
    }
}
