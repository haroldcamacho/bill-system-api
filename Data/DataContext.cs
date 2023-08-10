using Microsoft.EntityFrameworkCore;
using BasicBilling.API.Models;

namespace BasicBilling.API.Data
{
    public class DataContext : DbContext
    {
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<Bill> Bills { get; set; }
        public DbSet<Client> Clients { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Bill>().ToTable("Bills");
            modelBuilder.Entity<Client>().ToTable("Clients");

            // Configure foreign key relationship if needed
            modelBuilder.Entity<Bill>()
                .HasOne(b => b.Client)
                .WithMany(c => c.Bills)
                .HasForeignKey(b => b.ClientId);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite("Data Source=BasicBilling.db");
            }
        }
    }
}
