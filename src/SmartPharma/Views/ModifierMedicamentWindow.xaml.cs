using SmartPharma.Data;
using SmartPharma.Models;
using System.Globalization;
using System.Windows;
using System.Windows.Controls;

namespace SmartPharma.Views
{
    public partial class ModifierMedicamentWindow : Window
    {
        private readonly int _medicamentId;

        public ModifierMedicamentWindow(Medicament medicament)
        {
            InitializeComponent();

            _medicamentId = medicament.Id;

            txtNom.Text = medicament.Nom;
            txtPrix.Text = medicament.Prix.ToString("0.00");
            txtQuantite.Text = medicament.QuantiteStock.ToString();
            txtQuantiteParBoite.Text = medicament.QuantiteParBoite.ToString();
            dpExpiration.SelectedDate = medicament.DateExpiration;

            SelectionnerForme(medicament.Forme);
        }

        private void SelectionnerForme(string forme)
        {
            foreach (ComboBoxItem item in cmbForme.Items)
            {
                if (item.Content?.ToString() == forme)
                {
                    cmbForme.SelectedItem = item;
                    break;
                }
            }
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNom.Text))
            {
                MessageBox.Show(
                    "Le nom du médicament est obligatoire.",
                    "Champ obligatoire",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (!decimal.TryParse(
                    txtPrix.Text.Replace(',', '.'),
                    NumberStyles.Any,
                    CultureInfo.InvariantCulture,
                    out decimal prix) ||
                prix < 0)
            {
                MessageBox.Show(
                    "Veuillez entrer un prix valide.",
                    "Prix invalide",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (!int.TryParse(txtQuantite.Text, out int quantiteStock) ||
                quantiteStock < 0)
            {
                MessageBox.Show(
                    "Veuillez entrer une quantité en stock valide.",
                    "Quantité invalide",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (!int.TryParse(txtQuantiteParBoite.Text, out int quantiteParBoite) ||
                quantiteParBoite <= 0)
            {
                MessageBox.Show(
                    "Veuillez entrer une quantité par boîte valide.",
                    "Quantité invalide",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (cmbForme.SelectedItem is not ComboBoxItem formeSelectionnee)
            {
                MessageBox.Show(
                    "Veuillez sélectionner la forme du médicament.",
                    "Forme obligatoire",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (dpExpiration.SelectedDate == null)
            {
                MessageBox.Show(
                    "Veuillez sélectionner une date d'expiration.",
                    "Date obligatoire",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            using var db = new SmartPharmaDbContext();

            var medicamentDb = db.Medicaments.Find(_medicamentId);

            if (medicamentDb == null)
            {
                MessageBox.Show(
                    "Le médicament est introuvable dans la base de données.",
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            medicamentDb.Nom = txtNom.Text.Trim();
            medicamentDb.Prix = prix;
            medicamentDb.QuantiteStock = quantiteStock;
            medicamentDb.QuantiteParBoite = quantiteParBoite;
            medicamentDb.Forme =
                formeSelectionnee.Content?.ToString() ?? string.Empty;
            medicamentDb.DateExpiration = dpExpiration.SelectedDate.Value;

            db.SaveChanges();

            MessageBox.Show(
                "Le médicament a été modifié avec succès.",
                "Modification réussie",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            DialogResult = true;
            Close();
        }

        private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}