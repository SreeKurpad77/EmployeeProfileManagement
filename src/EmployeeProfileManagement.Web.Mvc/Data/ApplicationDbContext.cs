using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using EmployeeProfileManagement.Core.Model;

namespace EmployeeProfileManagement.Web.Mvc.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }
        public DbSet<EmployeeProfileManagement.Core.Model.EmployeeProfile>? EmployeeProfile { get; set; }
    }
}