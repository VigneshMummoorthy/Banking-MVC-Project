using System.Data.Entity;
using iStart.Models;

namespace iStart.Data
{
    public class UserDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
    }
}
