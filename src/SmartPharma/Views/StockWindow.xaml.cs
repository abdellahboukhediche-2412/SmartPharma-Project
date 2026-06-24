using SmartPharma.Data;
using SmartPharma.Models;
using System.Linq;
using System.Windows;

namespace SmartPharma.Views
{
    public partial class StockWindow : Window
    {
        public StockWindow()
        {
            InitializeComponent();
            ChargerStock();
        }

        private void ChargerStock()
        {
            using var db = new SmartPharmaDbContext();

            var stock = db.Medicaments
                .Select(m => new
                {
                    m.Id,
                    m.Nom,
                    m.Prix,
                    m.QuantiteStock,
                    m.DateExpiration,
                    EtatStock = m.QuantiteStock <= 10 ? "Stock faible" : "Stock normal"
                })
                .ToList();

            dgStock.ItemsSource = stock;
        }

        private void BtnRechercher_Click(object sender, RoutedEventArgs e)
        {
            using var db = new SmartPharmaDbContext();

            string recherche = txtRecherche.Text.Trim().ToLower();

            var resultats = db.Medicaments
                .Where(m => m.Nom.ToLower().Contains(recherche))
                .Select(m => new
                {
                    m.Id,
                    m.Nom,
                    m.Prix,
                    m.QuantiteStock,
                    m.DateExpiration,
                    EtatStock = m.QuantiteStock <= 10 ? "Stock faible" : "Stock normal"
                })
                .ToList();

            dgStock.ItemsSource = resultats;
        }

        private void BtnToutAfficher_Click(object sender, RoutedEventArgs e)
        {
            txtRecherche.Clear();
            ChargerStock();
        }

        private void BtnStockFaible_Click(object sender, RoutedEventArgs e)
        {
            using var db = new SmartPharmaDbContext();

            var stockFaible = db.Medicaments
                .Where(m => m.QuantiteStock <= 10)
                .Select(m => new
                {
                    m.Id,
                    m.Nom,
                    m.Prix,
                    m.QuantiteStock,
                    m.DateExpiration,
                    EtatStock = "Stock faible"
                })
                .ToList();

            dgStock.ItemsSource = stockFaible;
        }

        private void BtnModifierQuantite_Click(object sender, RoutedEventArgs e)
        {
            if (dgStock.SelectedItem == null)
            {
                MessageBox.Show("Veuillez sélectionner un médicament.");
                return;
            }

            dynamic ligne = dgStock.SelectedItem;

            int id = ligne.Id;
            string nom = ligne.Nom;
            int quantite = ligne.QuantiteStock;

            var fenetre = new ModifierQuantiteWindow(id, nom, quantite);
            fenetre.ShowDialog();

            ChargerStock();
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }
    }
}