namespace SmartPharma.Models
{
    public class Utilisateur
    {
        public int Id { get; set; }

        public string Nom { get; set; } = string.Empty;

        public string Prenom { get; set; } = string.Empty;

        public string NomUtilisateur { get; set; } = string.Empty;

        public string MotDePasse { get; set; } = string.Empty;

        public bool Actif { get; set; } = true;
    }
}