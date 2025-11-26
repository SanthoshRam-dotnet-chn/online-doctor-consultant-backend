using Microsoft.EntityFrameworkCore;
using UserAuthService.src.AuthService.Domain.Entities;

namespace UserAuthService.src.AuthService.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}
