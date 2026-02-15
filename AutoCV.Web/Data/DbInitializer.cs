using Microsoft.EntityFrameworkCore;

namespace AutoCV.Web.Data
{
    public static class DbInitializer
    {
        public static void Initialize(AutoCvDbContext context)
        {
            // Ensure database is created
            context.Database.EnsureCreated();
        }
    }
}
