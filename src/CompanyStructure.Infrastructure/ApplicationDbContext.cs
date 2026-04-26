using CompanyStructure.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace CompanyStructure.Infrastructure
{
    public class ApplicationDbContext : DbContext
    {
        public DbSet<Company> Companies { get; set; } = null!;
        public DbSet<Division> Divisions { get; set; } = null!;
        public DbSet<Project> Projects { get; set; } = null!;
        public DbSet<Department> Departments { get; set; } = null!;
        public DbSet<Employee> Employees { get; set; } = null!;

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
