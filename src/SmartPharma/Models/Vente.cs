namespace SmartPharma.Models
{
    public class Vente
    {
        public int Id { get; set; }

        public DateTime DateVente { get; set; } = DateTime.Now;

        public decimal MontantTotal { get; set; }

        public int MedicamentId { get; set; }

        public Medicament Medicament { get; set; } = null!;

        public int QuantiteVendue { get; set; }
    }
}