using Microsoft.EntityFrameworkCore;
using VideoService.Domain.Entities;

namespace VideoService.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> opt) : base(opt) { }

        public DbSet<VideoRoom> VideoRooms { get; set; }
        public DbSet<WaitingRoomStatus> WaitingRoomStatuses { get; set; }
    }
}
