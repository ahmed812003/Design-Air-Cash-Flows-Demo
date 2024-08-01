using DesignAirDemo.Models.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DesignAirDemo.Data
{
    public class AppDbContext : IdentityDbContext<IdentityUser>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
            
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        public DbSet<Project> Projects { get; set; }

        public DbSet<Contractor> Contractors { get; set; }

        public DbSet<Supplier> Suppliers { get; set; }

        public DbSet<Client> Clients { get; set; }

        public DbSet<Operation> Operations { get; set; }

        public DbSet<OperationType> OperationTypes { get; set; }

        public DbSet<DashboardData> DashboardData { get; set; }

        public DbSet<ArchivedItem> ArchivedItems { get; set; }
    }
}
