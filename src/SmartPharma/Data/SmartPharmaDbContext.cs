using Microsoft.EntityFrameworkCore;
using SmartPharma.Models;

namespace SmartPharma.Data
{
    public class SmartPharmaDbContext : DbContext
    {
        public DbSet<Medicament> Medicaments { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\MSSQLLocalDB;Database=SmartPharmaDB;Trusted_Connection=True;TrustServerCertificate=True");
        }
    public DbSet<Vente> Ventes { get; set; }
    }
}

