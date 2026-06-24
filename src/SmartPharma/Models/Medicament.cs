namespace SmartPharma.Models
{
    public class Medicament
    {
        public int Id { get; set; }

        public string Nom { get; set; } = string.Empty;

        public decimal Prix { get; set; }

        public int QuantiteStock { get; set; }

        public DateTime DateExpiration { get; set; }
    }
}
