using DatingApp_Api.Enitites;
using Microsoft.EntityFrameworkCore;

namespace DatingApp_Api.Data
{
    public class DataContext:DbContext
    {
        public DataContext(DbContextOptions options):base(options)
        {
            
        }
        public DbSet<AppUser> AppUsers { get; set; }
    }
}