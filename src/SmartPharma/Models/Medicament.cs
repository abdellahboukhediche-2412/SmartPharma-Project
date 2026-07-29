namespace SmartPharma.Models
{
    public class Medicament
    {
        public int Id { get; set; }

        public string Nom { get; set; } = string.Empty;

        public decimal Prix { get; set; }

        public int QuantiteStock { get; set; }

        public int QuantiteParBoite { get; set; }

        public string Forme { get; set; } = string.Empty;

        public DateTime DateExpiration { get; set; }
    }
}