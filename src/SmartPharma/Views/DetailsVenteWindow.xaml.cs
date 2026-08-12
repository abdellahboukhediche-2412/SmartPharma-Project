using Microsoft.EntityFrameworkCore;
using SmartPharma.Data;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace SmartPharma.Views
{
    public partial class DetailsVenteWindow : Window
    {
        private readonly int _venteId;

        private readonly CultureInfo cultureCanada =
            new CultureInfo("fr-CA");

        public DetailsVenteWindow(int venteId)
        {
            InitializeComponent();

            _venteId = venteId;

            ChargerDetails();
        }

        private void ChargerDetails()
        {
            using var db = new SmartPharmaDbContext();

            var vente = db.Ventes
                .Include(v => v.Lignes)
                .ThenInclude(l => l.Medicament)
                .FirstOrDefault(v => v.Id == _venteId);

            if (vente == null)
            {
                MessageBox.Show(
                    "Vente introuvable.",
                    "SmartPharma",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                Close();
                return;
            }

            txtNumeroVente.Text =
                "#" + vente.Id;

            txtDateVente.Text =
                vente.DateVente.ToString(
                    "dd/MM/yyyy HH:mm");

            txtMontantTotal.Text =
                vente.MontantTotal.ToString(
                    "C2",
                    cultureCanada);

            dgDetails.ItemsSource =
                vente.Lignes.ToList();
        }

        private void BtnFermer_Click(
            object sender,
            RoutedEventArgs e)
        {
            Close();
        }
    }
}