using DatingApp_Api.Enitites;
using Microsoft.EntityFrameworkCore;

namespace DatingApp_Api.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions options) : base(options)
        {

        }
        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<UserLike> Likes { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<UserLike>()
            .HasKey(k => new { k.SourceUserId, k.LikedUserId });

            builder.Entity<UserLike>()
            .HasOne(s => s.SourceUser)
            .WithMany(l => l.LikedUser)
            .HasForeignKey(s => s.SourceUserId)
            .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<UserLike>()
           .HasOne(s => s.LikedUser)
           .WithMany(l => l.LikedByUser) //how has liked currently user
           .HasForeignKey(s => s.LikedUserId)
           .OnDelete(DeleteBehavior.NoAction);
        }
    }
}