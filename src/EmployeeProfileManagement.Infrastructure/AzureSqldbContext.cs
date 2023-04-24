using EmployeeProfileManagement.Core.Model;
using Microsoft.EntityFrameworkCore;

namespace EmployeeProfileManagement.Infrastructure
{
    public class AzureSqldbContext : DbContext
    {
        public AzureSqldbContext(DbContextOptions<AzureSqldbContext> options) : base(options) { }

        public DbSet<EmployeeProfile> EmployeeProfiles { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            modelBuilder.Entity<EmployeeProfile>().HasData(
            new EmployeeProfile
            {
                Id = 1,
                Name = "Test",
                DateofBirth = DateTime.Now,
                Designation = "Manager",
                HireDate = DateTime.Now,
                PhotoUrl = ""
            });
        }
    }
}