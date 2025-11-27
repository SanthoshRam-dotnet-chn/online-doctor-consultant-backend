namespace PatientService.Data
{
    using Microsoft.EntityFrameworkCore;
    using PatientService.Domain.Entities;
    using System.Collections.Generic;
    using System.Reflection.Emit;

    public class PatientDbContext : DbContext
    {
        public PatientDbContext(DbContextOptions<PatientDbContext> options)
            : base(options)
        {
        }

        public DbSet<Appointment> Appointments { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Appointment>(entity =>
            {
                entity.ToTable("Appointments");

                entity.HasKey(a => a.Id);

                entity.Property(a => a.Id)
                    .IsRequired();

                entity.Property(a => a.SlotId)
                    .IsRequired();

                entity.Property(a => a.PatientId)
                    .IsRequired();

                entity.Property(a => a.CreatedAt)
                    .IsRequired();
            });
        }
    }

}
