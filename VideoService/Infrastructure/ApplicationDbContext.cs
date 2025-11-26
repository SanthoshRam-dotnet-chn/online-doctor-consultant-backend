using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Reflection.Emit;
using VideoService.Domain.Entities;

namespace VideoService.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opts) : base(opts) { }

        public DbSet<Appointment> Appointments { get; set; }
        public DbSet<VideoRoom> VideoRooms { get; set; }
        public DbSet<WaitingRoomStatus> WaitingRoomStatuses { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Appointment>().HasKey(a => a.AppointmentId);
            modelBuilder.Entity<VideoRoom>().HasKey(v => v.VideoRoomId);
            modelBuilder.Entity<WaitingRoomStatus>().HasKey(w => w.WaitingRoomStatusId);
            base.OnModelCreating(modelBuilder);
        }
    }
}
