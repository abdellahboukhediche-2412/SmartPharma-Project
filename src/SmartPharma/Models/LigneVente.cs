namespace SmartPharma.Models
{
    public class LigneVente
    {
        public int Id { get; set; }

        public int VenteId { get; set; }
        public Vente Vente { get; set; } = null!;

        public int MedicamentId { get; set; }
        public Medicament Medicament { get; set; } = null!;

        public int Quantite { get; set; }

        public decimal PrixUnitaire { get; set; }

        public decimal SousTotal { get; set; }
    }
}