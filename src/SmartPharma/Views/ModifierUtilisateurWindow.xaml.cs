using SmartPharma.Data;
using SmartPharma.Models;
using System.Linq;
using System.Windows;

namespace SmartPharma.Views
{
    public partial class ModifierUtilisateurWindow : Window
    {
        private readonly int _utilisateurId;

        public ModifierUtilisateurWindow(Utilisateur utilisateur)
        {
            InitializeComponent();

            _utilisateurId = utilisateur.Id;

            txtNom.Text = utilisateur.Nom;
            txtPrenom.Text = utilisateur.Prenom;
            txtNomUtilisateur.Text = utilisateur.NomUtilisateur;
            chkActif.IsChecked = utilisateur.Actif;
        }

        private void BtnEnregistrer_Click(
            object sender,
            RoutedEventArgs e)
        {
            string nom = txtNom.Text.Trim();
            string prenom = txtPrenom.Text.Trim();
            string nomUtilisateur = txtNomUtilisateur.Text.Trim();
            string nouveauMotDePasse = txtMotDePasse.Password;
            string confirmation = txtConfirmation.Password;

            // Champs obligatoires
            if (string.IsNullOrWhiteSpace(nom) ||
                string.IsNullOrWhiteSpace(prenom) ||
                string.IsNullOrWhiteSpace(nomUtilisateur))
            {
                MessageBox.Show(
                    "Veuillez remplir tous les champs obligatoires.",
                    "Champs obligatoires",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            // Nom utilisateur minimum
            if (nomUtilisateur.Length < 3)
            {
                MessageBox.Show(
                    "Le nom d'utilisateur doit contenir au moins 3 caractères.",
                    "Nom d'utilisateur invalide",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            // Si l'utilisateur veut changer son mot de passe
            if (!string.IsNullOrWhiteSpace(nouveauMotDePasse))
            {
                if (nouveauMotDePasse.Length < 4)
                {
                    MessageBox.Show(
                        "Le mot de passe doit contenir au moins 4 caractères.",
                        "Mot de passe invalide",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }

                if (nouveauMotDePasse != confirmation)
                {
                    MessageBox.Show(
                        "Les deux mots de passe ne correspondent pas.",
                        "Erreur",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning);

                    return;
                }
            }

            using var db = new SmartPharmaDbContext();

            var utilisateurDb =
                db.Utilisateurs.Find(_utilisateurId);

            if (utilisateurDb == null)
            {
                MessageBox.Show(
                    "Utilisateur introuvable.",
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            // Vérifier si un autre utilisateur possède déjà ce nom
            bool existe = db.Utilisateurs
                .Any(u =>
                    u.NomUtilisateur == nomUtilisateur &&
                    u.Id != _utilisateurId);

            if (existe)
            {
                MessageBox.Show(
                    "Ce nom d'utilisateur est déjà utilisé par un autre compte.",
                    "Nom d'utilisateur existant",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            // Mise à jour
            utilisateurDb.Nom = nom;
            utilisateurDb.Prenom = prenom;
            utilisateurDb.NomUtilisateur = nomUtilisateur;
            utilisateurDb.Actif = chkActif.IsChecked == true;

            // Modifier le mot de passe seulement si un nouveau est saisi
            if (!string.IsNullOrWhiteSpace(nouveauMotDePasse))
            {
                utilisateurDb.MotDePasse =
                    nouveauMotDePasse;
            }

            db.SaveChanges();

            MessageBox.Show(
                "Utilisateur modifié avec succès.",
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