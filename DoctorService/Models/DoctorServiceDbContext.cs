using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace DoctorService.Models;

public partial class DoctorServiceDbContext : DbContext
{
    public DoctorServiceDbContext()
    {
    }

    public DoctorServiceDbContext(DbContextOptions<DoctorServiceDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<AvailabilitySlot> AvailabilitySlots { get; set; }

    public virtual DbSet<Prescription> Prescriptions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see https://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Server=ENCAPJCHNLT0138;Database=DoctorService_Db;Trusted_Connection=True;TrustServerCertificate=True;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<AvailabilitySlot>(entity =>
        {
            entity.Property(e => e.Id).ValueGeneratedNever();
        });

        modelBuilder.Entity<Prescription>(entity =>
        {
            entity.Property(e => e.PrescriptionId).ValueGeneratedNever();
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
