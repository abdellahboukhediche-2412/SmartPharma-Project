using Microsoft.EntityFrameworkCore;
using SmartPharma.Models;

namespace SmartPharma.Data
{
    public class SmartPharmaDbContext : DbContext
    {
        public DbSet<Medicament> Medicaments { get; set; }
        public DbSet<Vente> Ventes { get; set; }
        public DbSet<Client> Clients { get; set; }
        public DbSet<Fournisseur> Fournisseurs { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(
                @"Server=(localdb)\MSSQLLocalDB;Database=SmartPharmaDB;Trusted_Connection=True;TrustServerCertificate=True");
        }
    }
}