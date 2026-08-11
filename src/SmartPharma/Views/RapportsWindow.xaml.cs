using SmartPharma.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SmartPharma.Views
{
    public partial class RapportsWindow : Window
    {
        private readonly CultureInfo cultureCanada =
            new CultureInfo("fr-CA");

        public RapportsWindow()
        {
            InitializeComponent();

            ChargerStatistiques();
            ChargerGraphiqueVentes();
        }


        // =====================================================
        // STATISTIQUES PRINCIPALES
        // =====================================================

        private void ChargerStatistiques()
        {
            using var db = new SmartPharmaDbContext();

            DateTime aujourdHui = DateTime.Today;
            DateTime demain = aujourdHui.AddDays(1);
            DateTime dans30Jours = aujourdHui.AddDays(30);


            // Nombre total de médicaments
            int totalMedicaments =
                db.Medicaments.Count();


            // Nombre de clients
            int totalClients =
                db.Clients.Count();


            // Nombre de fournisseurs
            int totalFournisseurs =
                db.Fournisseurs.Count();


            // Nombre total de ventes
            int totalVentes =
                db.Ventes.Count();


            // Montant total de toutes les ventes
            decimal montantTotal = db.Ventes
                .Select(v => (decimal?)v.MontantTotal)
                .Sum() ?? 0;


            // Stock inférieur à 10
            int stockFaible = db.Medicaments
                .Count(m => m.QuantiteStock < 10);


            // Médicaments expirés
            int medicamentsExpires = db.Medicaments
                .Count(m => m.DateExpiration < aujourdHui);


            // Médicaments expirant dans les 30 prochains jours
            int procheExpiration = db.Medicaments
                .Count(m =>
                    m.DateExpiration >= aujourdHui &&
                    m.DateExpiration <= dans30Jours);


            // Nombre de ventes aujourd'hui
            int ventesAujourdhui = db.Ventes
                .Count(v =>
                    v.DateVente >= aujourdHui &&
                    v.DateVente < demain);


            // Montant des ventes aujourd'hui
            decimal montantAujourdhui = db.Ventes
                .Where(v =>
                    v.DateVente >= aujourdHui &&
                    v.DateVente < demain)
                .Select(v => (decimal?)v.MontantTotal)
                .Sum() ?? 0;


            // Affichage
            txtTotalMedicaments.Text =
                totalMedicaments.ToString();

            txtTotalClients.Text =
                totalClients.ToString();

            txtTotalFournisseurs.Text =
                totalFournisseurs.ToString();

            txtTotalVentes.Text =
                totalVentes.ToString();

            txtMontantVentes.Text =
                montantTotal.ToString("C2", cultureCanada);

            txtStockFaible.Text =
                stockFaible.ToString();

            txtMedicamentsExpires.Text =
                medicamentsExpires.ToString();

            txtProcheExpiration.Text =
                procheExpiration.ToString();

            txtVentesAujourdhui.Text =
                ventesAujourdhui.ToString();

            txtMontantAujourdhui.Text =
                montantAujourdhui.ToString("C2", cultureCanada);
        }


        // =====================================================
        // GRAPHIQUE DES 7 DERNIERS JOURS
        // =====================================================

        private void ChargerGraphiqueVentes()
        {
            using var db = new SmartPharmaDbContext();

            spGraphiqueVentes.Children.Clear();

            DateTime aujourdHui = DateTime.Today;
            DateTime debut = aujourdHui.AddDays(-6);

            // On récupère les ventes une seule fois
            var ventes = db.Ventes
                .Where(v => v.DateVente >= debut)
                .ToList();


            var statistiques = new List<(DateTime Date, decimal Montant)>();


            for (int i = 0; i < 7; i++)
            {
                DateTime jour = debut.AddDays(i);

                decimal montant = ventes
                    .Where(v => v.DateVente.Date == jour.Date)
                    .Sum(v => v.MontantTotal);

                statistiques.Add((jour, montant));
            }


            decimal montantMaximum =
                statistiques.Max(s => s.Montant);


            // Évite division par zéro
            if (montantMaximum <= 0)
            {
                montantMaximum = 1;
            }


            foreach (var statistique in statistiques)
            {
                // Hauteur proportionnelle
                double hauteur =
                    (double)(statistique.Montant / montantMaximum) * 140;

                if (statistique.Montant > 0 && hauteur < 10)
                {
                    hauteur = 10;
                }


                // Colonne principale
                StackPanel colonne = new StackPanel
                {
                    Width = 105,
                    Margin = new Thickness(5),
                    VerticalAlignment = VerticalAlignment.Bottom
                };


                // Montant en haut
                TextBlock montantText = new TextBlock
                {
                    Text = statistique.Montant
                        .ToString("C0", cultureCanada),

                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 11,
                    FontWeight = FontWeights.Bold,
                    Foreground = new SolidColorBrush(
                        Color.FromRgb(11, 61, 62)),

                    Margin = new Thickness(0, 0, 0, 5)
                };


                // Barre graphique
                Border barre = new Border
                {
                    Width = 55,
                    Height = hauteur,
                    MinHeight = 2,
                    Background = new SolidColorBrush(
                        Color.FromRgb(52, 152, 219)),

                    CornerRadius = new CornerRadius(6, 6, 0, 0),

                    HorizontalAlignment = HorizontalAlignment.Center
                };


                // Jour
                TextBlock dateText = new TextBlock
                {
                    Text = statistique.Date
                        .ToString("ddd\ndd MMM", cultureCanada),

                    TextAlignment = TextAlignment.Center,
                    HorizontalAlignment = HorizontalAlignment.Center,
                    FontSize = 11,
                    Foreground = Brushes.DimGray,

                    Margin = new Thickness(0, 6, 0, 0)
                };


                colonne.Children.Add(montantText);
                colonne.Children.Add(barre);
                colonne.Children.Add(dateText);

                spGraphiqueVentes.Children.Add(colonne);
            }
        }


        // =====================================================
        // ACTUALISER
        // =====================================================

        private void BtnActualiser_Click(
            object sender,
            RoutedEventArgs e)
        {
            ChargerStatistiques();
            ChargerGraphiqueVentes();

            MessageBox.Show(
                "Les statistiques ont été actualisées.",
                "SmartPharma",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }


        // =====================================================
        // RETOUR
        // =====================================================

        private void BtnRetour_Click(
            object sender,
            RoutedEventArgs e)
        {
            Close();
        }
    }
}