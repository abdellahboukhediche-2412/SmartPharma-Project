using SmartPharma.Data;
using SmartPharma.Views;
using System;
using System.Globalization;
using System.Linq;
using System.Windows;

namespace SmartPharma
{
    public partial class MainWindow : Window
    {
        private readonly CultureInfo cultureCanada =
            new CultureInfo("fr-CA");

        public MainWindow()
        {
            InitializeComponent();

            ChargerStatistiques();
        }

        private void ChargerStatistiques()
        {
            try
            {
                using var db = new SmartPharmaDbContext();

                DateTime aujourdHui = DateTime.Today;
                DateTime demain = aujourdHui.AddDays(1);

                // Nombre total de médicaments
                int totalMedicaments = db.Medicaments.Count();

                // Stock faible : moins de 10 unités
                int stockFaible = db.Medicaments
                    .Count(m => m.QuantiteStock < 10);

                // Médicaments périmés
                int medicamentsExpires = db.Medicaments
                    .Count(m => m.DateExpiration < aujourdHui);

                // Nombre de ventes aujourd'hui
                int ventesAujourdhui = db.Ventes
                    .Count(v =>
                        v.DateVente >= aujourdHui &&
                        v.DateVente < demain);

                // Montant total des ventes aujourd'hui
                decimal montantAujourdhui = db.Ventes
                    .Where(v =>
                        v.DateVente >= aujourdHui &&
                        v.DateVente < demain)
                    .Select(v => (decimal?)v.MontantTotal)
                    .Sum() ?? 0;

                // Mise à jour du tableau de bord
                txtDashboardMedicaments.Text =
                    totalMedicaments.ToString();

                txtDashboardStockFaible.Text =
                    stockFaible.ToString();

                txtDashboardExpires.Text =
                    medicamentsExpires.ToString();

                txtDashboardVentesJour.Text =
                    ventesAujourdhui.ToString();

                txtDashboardMontantJour.Text =
                    montantAujourdhui.ToString(
                        "C2",
                        cultureCanada);
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    "Erreur lors du chargement des statistiques :\n" +
                    ex.Message,
                    "SmartPharma",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }

        private void BtnMedicaments_Click(
            object sender,
            RoutedEventArgs e)
        {
            new MedicamentsWindow().ShowDialog();

            ChargerStatistiques();
        }

        private void BtnStock_Click(
            object sender,
            RoutedEventArgs e)
        {
            new StockWindow().ShowDialog();

            ChargerStatistiques();
        }

        private void BtnVentes_Click(
            object sender,
            RoutedEventArgs e)
        {
            new VentesWindow().ShowDialog();

            ChargerStatistiques();
        }

        private void BtnClients_Click(
            object sender,
            RoutedEventArgs e)
        {
            var fenetre = new ClientsWindow();
            fenetre.ShowDialog();

            ChargerStatistiques();
        }

        private void BtnFournisseurs_Click(
            object sender,
            RoutedEventArgs e)
        {
            var fenetre = new FournisseursWindow();
            fenetre.ShowDialog();

            ChargerStatistiques();
        }

        private void BtnContact_Click(
            object sender,
            RoutedEventArgs e)
        {
            var contact = new ContactWindow();
            contact.ShowDialog();
        }

        private void BtnRapports_Click(
            object sender,
            RoutedEventArgs e)
        {
            var fenetre = new RapportsWindow();
            fenetre.ShowDialog();

            ChargerStatistiques();
        }

        private void BtnActualiser_Click(
            object sender,
            RoutedEventArgs e)
        {
            ChargerStatistiques();

            MessageBox.Show(
                "Le tableau de bord a été actualisé.",
                "SmartPharma",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
    }
}