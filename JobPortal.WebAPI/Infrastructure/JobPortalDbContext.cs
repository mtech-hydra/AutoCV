using Microsoft.EntityFrameworkCore;
using JobPortal.WebAPI.Domain;

public class JobPortalDbContext : DbContext
{
    public JobPortalDbContext(DbContextOptions<JobPortalDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<CVProfile> CVProfiles => Set<CVProfile>();
    public DbSet<JobAd> JobAds => Set<JobAd>();
    public DbSet<CoverLetter> CoverLetters => Set<CoverLetter>();
    public DbSet<JobApplication> JobApplications => Set<JobApplication>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().HasQueryFilter(u => !u.IsDeleted);
        modelBuilder.Entity<CVProfile>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<JobAd>().HasQueryFilter(j => !j.IsDeleted);
        modelBuilder.Entity<CoverLetter>().HasQueryFilter(c => !c.IsDeleted);
        modelBuilder.Entity<JobApplication>().HasQueryFilter(a => !a.IsDeleted);
    }
}