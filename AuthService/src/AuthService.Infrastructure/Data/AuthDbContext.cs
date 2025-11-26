using Microsoft.EntityFrameworkCore;
using AuthService.src.AuthService.Domain.Entities;

namespace AuthService.src.AuthService.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
