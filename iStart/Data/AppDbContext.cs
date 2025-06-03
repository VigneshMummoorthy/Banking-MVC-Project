using iStart.Models;
using Microsoft.EntityFrameworkCore;

namespace iStart.Data
{
    public class AppDbContext : DbContext
    {
        //public AppDbContext(DbContextOptions<AppDbContext> options)
        //    : base(options)
        //{
        //}

        public DbSet<Employee> Employees { get; set; }
    }
}
