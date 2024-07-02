using Microsoft.EntityFrameworkCore;
using CRM_App.Models;

namespace CRM_App.Services
{
    public class AppDbContext : DbContext
    {
        private readonly string _databasePath;
        public AppDbContext(string databasePath)
        {
            _databasePath = databasePath;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite($"Data Source={_databasePath}");
        }

        public DbSet<Comment> Comments { get; set; }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Request> Requests { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<Status> Statuses { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //tabela Statuses va avea decat valorile de mai jos
            modelBuilder.Entity<Status>().HasData(
                new Status { StatusID = 1, StatusName = "in desfasurare" },
                new Status { StatusID = 2, StatusName = "finalizat" },
                new Status { StatusID = 3, StatusName = "anulat" }
            );
        }
    }
}
