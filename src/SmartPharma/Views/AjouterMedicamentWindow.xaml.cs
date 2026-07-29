using SmartPharma.Data;
using SmartPharma.Models;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace SmartPharma.Views
{
    public partial class AjouterMedicamentWindow : Window
    {
        public AjouterMedicamentWindow()
        {
            InitializeComponent();
        }

        private void BtnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNom.Text))
            {
                MessageBox.Show("Le nom du médicament est obligatoire.");
                return;
            }

            if (!decimal.TryParse(
                    txtPrix.Text.Replace(',', '.'),
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out decimal prix) ||
                prix < 0)
            {
                MessageBox.Show("Veuillez entrer un prix valide.");
                return;
            }

            if (!int.TryParse(txtQuantite.Text, out int quantiteStock) ||
                quantiteStock < 0)
            {
                MessageBox.Show("Veuillez entrer une quantité en stock valide.");
                return;
            }

            if (!int.TryParse(txtQuantiteParBoite.Text, out int quantiteParBoite) ||
                quantiteParBoite <= 0)
            {
                MessageBox.Show("Veuillez entrer une quantité par boîte valide.");
                return;
            }

            if (cmbForme.SelectedItem is not ComboBoxItem formeSelectionnee)
            {
                MessageBox.Show("Veuillez sélectionner la forme du médicament.");
                return;
            }

            if (dpExpiration.SelectedDate == null)
            {
                MessageBox.Show("Veuillez sélectionner la date d'expiration.");
                return;
            }

            var medicament = new Medicament
            {
                Nom = txtNom.Text.Trim(),
                Prix = prix,
                QuantiteStock = quantiteStock,
                QuantiteParBoite = quantiteParBoite,
                Forme = formeSelectionnee.Content.ToString() ?? string.Empty,
                DateExpiration = dpExpiration.SelectedDate.Value
            };

            using var db = new SmartPharmaDbContext();

            db.Medicaments.Add(medicament);
            db.SaveChanges();

            MessageBox.Show("Médicament ajouté avec succès.");

            DialogResult = true;
            Close();
        }

        private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}