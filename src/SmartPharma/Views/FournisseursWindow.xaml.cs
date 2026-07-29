using SmartPharma.Data;
using SmartPharma.Models;
using System.Linq;
using System.Windows;

namespace SmartPharma.Views
{
    public partial class FournisseursWindow : Window
    {
        public FournisseursWindow()
        {
            InitializeComponent();
            ChargerFournisseurs();
        }

        private void ChargerFournisseurs()
        {
            using var db = new SmartPharmaDbContext();

            dgFournisseurs.ItemsSource = db.Fournisseurs.ToList();
        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            var fenetre = new AjouterFournisseurWindow();

            fenetre.ShowDialog();
            ChargerFournisseurs();
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (dgFournisseurs.SelectedItem is Fournisseur fournisseurSelectionne)
            {
                var fenetre = new ModifierFournisseurWindow(fournisseurSelectionne);

                bool? resultat = fenetre.ShowDialog();

                if (resultat == true)
                {
                    ChargerFournisseurs();
                }
            }
            else
            {
                MessageBox.Show(
                    "Veuillez sélectionner un fournisseur à modifier.",
                    "Aucune sélection",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            if (dgFournisseurs.SelectedItem is not Fournisseur fournisseurSelectionne)
            {
                MessageBox.Show(
                    "Veuillez sélectionner un fournisseur à supprimer.",
                    "Aucune sélection",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            var confirmation = MessageBox.Show(
                $"Voulez-vous vraiment supprimer le fournisseur « {fournisseurSelectionne.Nom} » ?",
                "Confirmation",
                MessageBoxButton.YesNo,
                MessageBoxImage.Question);

            if (confirmation != MessageBoxResult.Yes)
            {
                return;
            }

            using var db = new SmartPharmaDbContext();

            var fournisseurDb = db.Fournisseurs.Find(fournisseurSelectionne.Id);

            if (fournisseurDb == null)
            {
                MessageBox.Show("Fournisseur introuvable.");
                return;
            }

            db.Fournisseurs.Remove(fournisseurDb);
            db.SaveChanges();

            MessageBox.Show("Fournisseur supprimé avec succès.");
            ChargerFournisseurs();
        }

        private void BtnRechercher_Click(object sender, RoutedEventArgs e)
        {
            using var db = new SmartPharmaDbContext();

            string recherche = txtRecherche.Text.Trim().ToLower();

            var resultats = db.Fournisseurs
                .Where(f =>
                    f.Nom.ToLower().Contains(recherche) ||
                    f.Entreprise.ToLower().Contains(recherche) ||
                    f.Telephone.ToLower().Contains(recherche) ||
                    f.Email.ToLower().Contains(recherche))
                .ToList();

            dgFournisseurs.ItemsSource = resultats;
        }

        private void BtnToutAfficher_Click(object sender, RoutedEventArgs e)
        {
            txtRecherche.Clear();
            ChargerFournisseurs();
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}