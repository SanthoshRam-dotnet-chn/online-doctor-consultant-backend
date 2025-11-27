using Microsoft.EntityFrameworkCore;
using PatientService.Domain.Entities;

namespace PatientService.Data
{
public class PatientDbContext : DbContext
{
    public PatientDbContext(DbContextOptions<PatientDbContext> options)
        : base(options)
    {
    }

    public DbSet<Appointment> Appointments { get; set; } 
    public DbSet<PatientRecord> PatientRecords { get; set; } 
    public DbSet<Attachment> Attachments { get; set; } 

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Appointment>(entity =>
        {
            entity.ToTable("Appointments");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Id).IsRequired();
            entity.Property(a => a.SlotId).IsRequired();
            entity.Property(a => a.PatientId).IsRequired();
            entity.Property(a => a.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<PatientRecord>(entity =>
        {
            entity.ToTable("PatientRecords");
            entity.HasKey(r => r.Id);
            entity.HasIndex(r => r.PatientId).IsUnique(); // one record per patient
            entity.Property(r => r.PatientId).IsRequired();
            entity.Property(r => r.DoctorNotes).HasMaxLength(5000);
            entity.Property(r => r.CreatedAt).IsRequired();
            entity.Property(r => r.UpdatedAt);
        });

        modelBuilder.Entity<Attachment>(entity =>
        {
            entity.ToTable("Attachments");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.FileName).IsRequired();
            entity.Property(a => a.StoredFileName).IsRequired();
            entity.Property(a => a.ContentType).IsRequired();
            entity.Property(a => a.Size).IsRequired();
            entity.Property(a => a.CreatedAt).IsRequired();
            entity.Property(a => a.IsDeleted).HasDefaultValue(false);
            entity.HasOne(a => a.PatientRecord)
                  .WithMany(r => r.Attachments)
                  .HasForeignKey(a => a.PatientRecordId)
                  .OnDelete(DeleteBehavior.Cascade);
        });
    }
}

}

