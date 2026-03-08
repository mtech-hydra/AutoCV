using Microsoft.EntityFrameworkCore;
using JobPortal.WebAPI.Domain;

namespace JobPortal.WebAPI.Infrastructure
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }

        // ToDo: Virtual for testing purposes, consider using interfaces and repositories for better testability
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<CVProfile> CVProfiles { get; set; }
        public virtual DbSet<JobAd> JobAds { get; set; }
        public virtual DbSet<CoverLetter> CoverLetters { get; set; }
        public virtual DbSet<JobApplication> JobApplications { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            base.OnModelCreating(modelBuilder);
        }
    }
}