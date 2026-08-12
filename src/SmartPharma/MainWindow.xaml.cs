using SmartPharma.Data;
using SmartPharma.Helpers;
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
            AfficherUtilisateur();
            AfficherDate();
            AfficherHeureConnexion();
        }

        // =====================================================
        // STATISTIQUES DU TABLEAU DE BORD
        // =====================================================

        private void ChargerStatistiques()
        {
            try
            {
                using var db = new SmartPharmaDbContext();

                DateTime aujourdHui = DateTime.Today;
                DateTime demain = aujourdHui.AddDays(1);

                int totalMedicaments =
                    db.Medicaments.Count();

                int stockFaible =
                    db.Medicaments
                        .Count(m => m.QuantiteStock < 10);

                int medicamentsExpires =
                    db.Medicaments
                        .Count(m => m.DateExpiration < aujourdHui);

                int ventesAujourdhui =
                    db.Ventes
                        .Count(v =>
                            v.DateVente >= aujourdHui &&
                            v.DateVente < demain);

                decimal montantAujourdhui =
                    db.Ventes
                        .Where(v =>
                            v.DateVente >= aujourdHui &&
                            v.DateVente < demain)
                        .Select(v => (decimal?)v.MontantTotal)
                        .Sum() ?? 0;

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
        private void AfficherHeureConnexion()
        {
            txtHeureConnexion.Text =
                "Connecté à " + DateTime.Now.ToString("HH:mm");
        }

        // =====================================================
        // MÉDICAMENTS
        // =====================================================

        private void BtnMedicaments_Click(
            object sender,
            RoutedEventArgs e)
        {
            new MedicamentsWindow().ShowDialog();

            ChargerStatistiques();
        }

        // =====================================================
        // STOCK
        // =====================================================

        private void BtnStock_Click(
            object sender,
            RoutedEventArgs e)
        {
            new StockWindow().ShowDialog();

            ChargerStatistiques();
        }

        // =====================================================
        // VENTES
        // =====================================================

        private void BtnVentes_Click(
            object sender,
            RoutedEventArgs e)
        {
            new VentesWindow().ShowDialog();

            ChargerStatistiques();
        }

        // =====================================================
        // CLIENTS
        // =====================================================

        private void BtnClients_Click(
            object sender,
            RoutedEventArgs e)
        {
            var fenetre = new ClientsWindow();

            fenetre.ShowDialog();

            ChargerStatistiques();
        }

        // =====================================================
        // FOURNISSEURS
        // =====================================================

        private void BtnFournisseurs_Click(
            object sender,
            RoutedEventArgs e)
        {
            var fenetre = new FournisseursWindow();

            fenetre.ShowDialog();

            ChargerStatistiques();
        }

        // =====================================================
        // CONTACT
        // =====================================================

        private void BtnContact_Click(
            object sender,
            RoutedEventArgs e)
        {
            var contact =
                new ContactWindow();

            contact.ShowDialog();
        }

        // =====================================================
        // RAPPORTS
        // =====================================================

        private void BtnRapports_Click(
            object sender,
            RoutedEventArgs e)
        {
            var fenetre =
                new RapportsWindow();

            fenetre.ShowDialog();

            ChargerStatistiques();
        }

        // =====================================================
        // UTILISATEURS
        // =====================================================

        private void BtnUtilisateurs_Click(
            object sender,
            RoutedEventArgs e)
        {
            var fenetre =
                new UtilisateursWindow();

            fenetre.ShowDialog();
        }

        // =====================================================
        // ACTUALISER
        // =====================================================

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

        // =====================================================
        // AFFICHER UTILISATEUR CONNECTÉ
        // =====================================================

        private void AfficherUtilisateur()
        {
            if (SessionUtilisateur.UtilisateurConnecte != null)
            {
                var utilisateur =
                    SessionUtilisateur.UtilisateurConnecte;

                txtUtilisateurConnecte.Text =
                    utilisateur.Prenom +
                    " " +
                    utilisateur.Nom;

                txtBienvenue.Text =
                    "Bonjour, " +
                    utilisateur.Prenom +
                    " 👋";
            }
            else
            {
                txtUtilisateurConnecte.Text =
                    "Aucun utilisateur";

                txtBienvenue.Text =
                    "Bonjour 👋";
            }
        }

        // =====================================================
        // DÉCONNEXION
        // =====================================================

        private void BtnDeconnexion_Click(
            object sender,
            RoutedEventArgs e)
        {
            var resultat =
                MessageBox.Show(
                    "Voulez-vous vraiment vous déconnecter ?",
                    "Déconnexion",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

            if (resultat != MessageBoxResult.Yes)
            {
                return;
            }

            // Effacer la session actuelle
            SessionUtilisateur.UtilisateurConnecte = null;

            // Ouvrir la page de connexion
            var login =
                new LoginWindow();

            login.Show();

            // Fermer le tableau de bord
            Close();
        }

        // =====================================================
        // DATE
        // =====================================================

        private void AfficherDate()
        {
            txtDate.Text =
                "Date : " +
                DateTime.Now.ToString(
                    "dd MMMM yyyy",
                    cultureCanada);
        }
    }
}