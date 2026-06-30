using SmartPharma.Data;
using SmartPharma.Models;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SmartPharma.Views
{
    public partial class VentesWindow : Window
    {
        private decimal total = 0;
        private readonly CultureInfo cultureCanada = new CultureInfo("fr-CA");

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
                    MontantTotal = v.MontantTotal.ToString("C2", cultureCanada),
                    Medicament = v.Medicament.Nom
                })
                .ToList();

            dgVentes.ItemsSource = ventes;
        }

        private void cmbMedicaments_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (cmbMedicaments.SelectedItem is Medicament medicament)
            {
                txtPrix.Text = medicament.Prix.ToString("C2", cultureCanada);
                txtStock.Text = medicament.QuantiteStock.ToString();
                txtExpiration.Text = medicament.DateExpiration.ToString("dd/MM/yyyy");

                if (medicament.QuantiteStock < 10)
                {
                    txtStock.Foreground = Brushes.Red;
                    txtStock.FontWeight = FontWeights.Bold;
                }
                else
                {
                    txtStock.Foreground = Brushes.Black;
                    txtStock.FontWeight = FontWeights.Normal;
                }

                txtQuantite.Clear();
                txtTotal.Text = "0,00 $";
                total = 0;
            }
        }

        private void BtnCalculer_Click(object sender, RoutedEventArgs e)
        {
            if (cmbMedicaments.SelectedItem is not Medicament medicament)
            {
                MessageBox.Show("Veuillez choisir un médicament.");
                return;
            }

            if (!int.TryParse(txtQuantite.Text, out int quantite))
            {
                MessageBox.Show("Veuillez entrer une quantité valide.");
                return;
            }

            if (quantite <= 0)
            {
                MessageBox.Show("La quantité doit être supérieure à 0.");
                return;
            }

            if (quantite > medicament.QuantiteStock)
            {
                MessageBox.Show("La quantité demandée dépasse le stock disponible.");
                return;
            }

            total = medicament.Prix * quantite;
            txtTotal.Text = total.ToString("C2", cultureCanada);
        }

        private void BtnEnregistrerVente_Click(object sender, RoutedEventArgs e)
        {
            if (cmbMedicaments.SelectedItem is not Medicament medicament)
            {
                MessageBox.Show("Veuillez choisir un médicament.");
                return;
            }

            if (!int.TryParse(txtQuantite.Text, out int quantite))
            {
                MessageBox.Show("Veuillez entrer une quantité valide.");
                return;
            }

            if (quantite <= 0)
            {
                MessageBox.Show("La quantité doit être supérieure à 0.");
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
            txtPrix.Clear();
            txtStock.Clear();
            txtExpiration.Clear();
            txtTotal.Text = "0,00 $";
            total = 0;

            ChargerMedicaments();
            ChargerVentes();
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}