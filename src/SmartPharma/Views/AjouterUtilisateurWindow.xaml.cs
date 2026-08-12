using SmartPharma.Data;
using SmartPharma.Models;
using System.Linq;
using System.Windows;

namespace SmartPharma.Views
{
    public partial class AjouterUtilisateurWindow : Window
    {
        public AjouterUtilisateurWindow()
        {
            InitializeComponent();
        }

        private void BtnEnregistrer_Click(
            object sender,
            RoutedEventArgs e)
        {
            string nom = txtNom.Text.Trim();
            string prenom = txtPrenom.Text.Trim();
            string nomUtilisateur = txtNomUtilisateur.Text.Trim();
            string motDePasse = txtMotDePasse.Password;
            string confirmation = txtConfirmation.Password;

            // Validation des champs obligatoires
            if (string.IsNullOrWhiteSpace(nom) ||
                string.IsNullOrWhiteSpace(prenom) ||
                string.IsNullOrWhiteSpace(nomUtilisateur) ||
                string.IsNullOrWhiteSpace(motDePasse))
            {
                MessageBox.Show(
                    "Veuillez remplir tous les champs obligatoires.",
                    "Champs obligatoires",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            // Longueur du nom utilisateur
            if (nomUtilisateur.Length < 3)
            {
                MessageBox.Show(
                    "Le nom d'utilisateur doit contenir au moins 3 caractères.",
                    "Nom d'utilisateur invalide",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            // Longueur du mot de passe
            if (motDePasse.Length < 4)
            {
                MessageBox.Show(
                    "Le mot de passe doit contenir au moins 4 caractères.",
                    "Mot de passe invalide",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            // Confirmation
            if (motDePasse != confirmation)
            {
                MessageBox.Show(
                    "Les deux mots de passe ne correspondent pas.",
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            using var db = new SmartPharmaDbContext();

            // Vérifier si le nom utilisateur existe déjà
            bool existe = db.Utilisateurs
                .Any(u => u.NomUtilisateur == nomUtilisateur);

            if (existe)
            {
                MessageBox.Show(
                    "Ce nom d'utilisateur existe déjà.",
                    "Compte existant",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            var utilisateur = new Utilisateur
            {
                Nom = nom,
                Prenom = prenom,
                NomUtilisateur = nomUtilisateur,
                MotDePasse = motDePasse,
                Actif = chkActif.IsChecked == true
            };

            db.Utilisateurs.Add(utilisateur);
            db.SaveChanges();

            MessageBox.Show(
                "Utilisateur ajouté avec succès.",
                "SmartPharma",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            DialogResult = true;
            Close();
        }

        private void BtnAnnuler_Click(
            object sender,
            RoutedEventArgs e)
        {
            Close();
        }
    }
}