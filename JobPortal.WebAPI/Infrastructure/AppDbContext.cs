using Microsoft.EntityFrameworkCore;
using JobPortal.WebAPI.Domain;

namespace JobPortal.WebAPI.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<CVProfile> CVProfiles { get; set; }
        public DbSet<JobAd> JobAds { get; set; }
        public DbSet<CoverLetter> CoverLetters { get; set; }
        public DbSet<JobApplication> JobApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}