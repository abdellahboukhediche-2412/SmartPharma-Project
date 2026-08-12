using SmartPharma.Data;
using SmartPharma.Models;
using System.Linq;
using System.Windows;

namespace SmartPharma.Views
{
    public partial class UtilisateursWindow : Window
    {
        public UtilisateursWindow()
        {
            InitializeComponent();

            ChargerUtilisateurs();
        }

        private void ChargerUtilisateurs()
        {
            using var db = new SmartPharmaDbContext();

            dgUtilisateurs.ItemsSource =
                db.Utilisateurs.ToList();
        }

        private void BtnAjouter_Click(
            object sender,
            RoutedEventArgs e)
        {
            var fenetre =
                new AjouterUtilisateurWindow();

            fenetre.ShowDialog();

            ChargerUtilisateurs();
        }

        private void BtnModifier_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (dgUtilisateurs.SelectedItem
                is Utilisateur utilisateurSelectionne)
            {
                var fenetre =
                    new ModifierUtilisateurWindow(
                        utilisateurSelectionne);

                bool? resultat =
                    fenetre.ShowDialog();

                if (resultat == true)
                {
                    ChargerUtilisateurs();
                }
            }
            else
            {
                MessageBox.Show(
                    "Veuillez sélectionner un utilisateur.",
                    "Aucune sélection",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void BtnSupprimer_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (dgUtilisateurs.SelectedItem
                is not Utilisateur utilisateurSelectionne)
            {
                MessageBox.Show(
                    "Veuillez sélectionner un utilisateur.",
                    "Aucune sélection",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            var confirmation = MessageBox.Show(
                $"Voulez-vous vraiment supprimer le compte « {utilisateurSelectionne.NomUtilisateur} » ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirmation != MessageBoxResult.Yes)
            {
                return;
            }

            using var db =
                new SmartPharmaDbContext();

            var utilisateurDb =
                db.Utilisateurs.Find(
                    utilisateurSelectionne.Id);

            if (utilisateurDb == null)
            {
                MessageBox.Show(
                    "Utilisateur introuvable.");

                return;
            }

            db.Utilisateurs.Remove(
                utilisateurDb);

            db.SaveChanges();

            MessageBox.Show(
                "Utilisateur supprimé avec succès.");

            ChargerUtilisateurs();
        }

        private void BtnRechercher_Click(
            object sender,
            RoutedEventArgs e)
        {
            using var db =
                new SmartPharmaDbContext();

            string recherche =
                txtRecherche.Text.Trim().ToLower();

            var resultats =
                db.Utilisateurs
                    .Where(u =>
                        u.Nom.ToLower()
                            .Contains(recherche) ||

                        u.Prenom.ToLower()
                            .Contains(recherche) ||

                        u.NomUtilisateur.ToLower()
                            .Contains(recherche))
                    .ToList();

            dgUtilisateurs.ItemsSource =
                resultats;
        }

        private void BtnToutAfficher_Click(
            object sender,
            RoutedEventArgs e)
        {
            txtRecherche.Clear();

            ChargerUtilisateurs();
        }

        private void BtnRetour_Click(
            object sender,
            RoutedEventArgs e)
        {
            Close();
        }
    }
}