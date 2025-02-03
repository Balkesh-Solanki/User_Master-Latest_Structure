using DataLayer.Entities;
using Microsoft.EntityFrameworkCore;

namespace DataLayer.DbScript
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }
        public DbSet<UserMst> Users { get; set; }
        public DbSet<TokenMst> TokenMst { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserMst>()
                .ToTable("UserMst"); 

            modelBuilder.Entity<UserMst>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<UserMst>()
                .HasIndex(u => u.Password)
                .IsUnique();

            modelBuilder.Entity<UserMst>()
                .HasIndex(u => u.Mobile)
                .IsUnique();

            modelBuilder.Entity<TokenMst>()
                .ToTable("TokenMst");

            modelBuilder.Entity<TokenMst>()
                .HasOne(t => t.User)
                .WithMany()
                .HasForeignKey(t => t.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
