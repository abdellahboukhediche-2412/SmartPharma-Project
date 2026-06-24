using SmartPharma.Data;
using SmartPharma.Models;
using System.Linq;
using System.Windows;

namespace SmartPharma.Views
{
    public partial class MedicamentsWindow : Window
    {
        public MedicamentsWindow()
        {
            InitializeComponent();

            ChargerMedicaments();
        }

        private void ChargerMedicaments()
        {
            using var db = new SmartPharmaDbContext();

            dgMedicaments.ItemsSource = db.Medicaments.ToList();
        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            var fenetre = new AjouterMedicamentWindow();

            fenetre.ShowDialog();

            ChargerMedicaments();
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (dgMedicaments.SelectedItem is Medicament medicamentSelectionne)
            {
                var fenetre = new ModifierMedicamentWindow(medicamentSelectionne);

                fenetre.ShowDialog();

                ChargerMedicaments();
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un médicament à modifier.");
            }
        }

        private void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            if (dgMedicaments.SelectedItem is Medicament medicamentSelectionne)
            {
                var confirmation = MessageBox.Show(
                    "Voulez-vous vraiment supprimer ce médicament ?",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (confirmation == MessageBoxResult.Yes)
                {
                    using var db = new SmartPharmaDbContext();

                    var medicamentDb = db.Medicaments.Find(medicamentSelectionne.Id);

                    if (medicamentDb != null)
                    {
                        db.Medicaments.Remove(medicamentDb);

                        db.SaveChanges();

                        MessageBox.Show("Médicament supprimé avec succès.");

                        ChargerMedicaments();
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un médicament à supprimer.");
            }
        }

        private void BtnRechercher_Click(object sender, RoutedEventArgs e)
        {
            using var db = new SmartPharmaDbContext();

            string recherche = txtRecherche.Text.Trim().ToLower();

            var resultats = db.Medicaments
                              .Where(m => m.Nom.ToLower().Contains(recherche))
                              .ToList();

            dgMedicaments.ItemsSource = resultats;
        }

        private void BtnToutAfficher_Click(object sender, RoutedEventArgs e)
        {
            txtRecherche.Clear();

            ChargerMedicaments();
        }
        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}