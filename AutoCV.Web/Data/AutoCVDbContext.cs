using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;

namespace AutoCV.Web.Data
{
    public class AutoCvDbContext : DbContext
    {
        public AutoCvDbContext(DbContextOptions<AutoCvDbContext> options)
            : base(options)
        {
        }

        public DbSet<JobAdEntity> Errand => Set<JobAdEntity>();
    }
}
