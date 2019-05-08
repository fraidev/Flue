using IdentityService.Domain.State;
using Microsoft.EntityFrameworkCore;

namespace IdentityService.Infrastructure.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<UserState> Users { get; set; }
    }
}