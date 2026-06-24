using SmartPharma.Data;
using SmartPharma.Models;
using System.Linq;
using System.Windows;

namespace SmartPharma.Views
{
    public partial class VentesWindow : Window
    {
        private decimal total = 0;

        public VentesWindow()
        {
            InitializeComponent();

            ChargerMedicaments();
            ChargerVentes();
        }

        private void ChargerMedicaments()
        {
            using var db = new SmartPharmaDbContext();

            cmbMedicaments.ItemsSource = db.Medicaments.ToList();
        }

        private void ChargerVentes()
        {
            using var db = new SmartPharmaDbContext();

            var ventes = db.Ventes
                .Select(v => new
                {
                    v.Id,
                    v.DateVente,
                    v.QuantiteVendue,
                    v.MontantTotal,
                    Medicament = v.Medicament.Nom
                })
                .ToList();

            dgVentes.ItemsSource = ventes;
        }

        private void BtnCalculer_Click(object sender, RoutedEventArgs e)
        {
            if (cmbMedicaments.SelectedItem is Medicament medicament &&
                int.TryParse(txtQuantite.Text, out int quantite))
            {
                total = medicament.Prix * quantite;
                txtTotal.Text = $"Total : {total} $";
            }
            else
            {
                MessageBox.Show("Veuillez choisir un médicament et entrer une quantité valide.");
            }
        }

        private void BtnEnregistrerVente_Click(object sender, RoutedEventArgs e)
        {
            if (cmbMedicaments.SelectedItem is not Medicament medicament ||
                !int.TryParse(txtQuantite.Text, out int quantite))
            {
                MessageBox.Show("Veuillez remplir correctement les champs.");
                return;
            }

            using var db = new SmartPharmaDbContext();

            var medicamentDb = db.Medicaments.Find(medicament.Id);

            if (medicamentDb == null)
            {
                MessageBox.Show("Médicament introuvable.");
                return;
            }

            if (medicamentDb.QuantiteStock < quantite)
            {
                MessageBox.Show("Stock insuffisant.");
                return;
            }

            var vente = new Vente
            {
                DateVente = DateTime.Now,
                MedicamentId = medicamentDb.Id,
                QuantiteVendue = quantite,
                MontantTotal = medicamentDb.Prix * quantite
            };

            medicamentDb.QuantiteStock -= quantite;

            db.Ventes.Add(vente);
            db.SaveChanges();

            MessageBox.Show("Vente enregistrée avec succès.");

            txtQuantite.Clear();
            txtTotal.Text = "Total : 0 $";

            ChargerMedicaments();
            ChargerVentes();
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}