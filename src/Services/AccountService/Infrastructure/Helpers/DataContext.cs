using AccountService.Domain.Write.State;
using Microsoft.EntityFrameworkCore;

namespace AccountService.Infrastructure.Helpers
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
    }
}