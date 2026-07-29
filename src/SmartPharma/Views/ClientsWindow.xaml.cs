using SmartPharma.Data;
using SmartPharma.Models;
using System.Linq;
using System.Windows;

namespace SmartPharma.Views
{
    public partial class ClientsWindow : Window
    {
        public ClientsWindow()
        {
            InitializeComponent();
            ChargerClients();
        }

        private void ChargerClients()
        {
            using var db = new SmartPharmaDbContext();

            dgClients.ItemsSource = db.Clients.ToList();
        }

        private void BtnAjouter_Click(object sender, RoutedEventArgs e)
        {
            var fenetre = new AjouterClientWindow();
            fenetre.ShowDialog();

            ChargerClients();
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (dgClients.SelectedItem is Client clientSelectionne)
            {
                var fenetre = new ModifierClientWindow(clientSelectionne);

                bool? resultat = fenetre.ShowDialog();

                if (resultat == true)
                {
                    ChargerClients();
                }
            }
            else
            {
                MessageBox.Show(
                    "Veuillez sélectionner un client à modifier.",
                    "Aucune sélection",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
            }
        }

        private void BtnSupprimer_Click(object sender, RoutedEventArgs e)
        {
            if (dgClients.SelectedItem is Client clientSelectionne)
            {
                var confirmation = MessageBox.Show(
                    "Voulez-vous vraiment supprimer ce client ?",
                    "Confirmation",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question);

                if (confirmation == MessageBoxResult.Yes)
                {
                    using var db = new SmartPharmaDbContext();

                    var clientDb = db.Clients.Find(clientSelectionne.Id);

                    if (clientDb != null)
                    {
                        db.Clients.Remove(clientDb);
                        db.SaveChanges();

                        MessageBox.Show("Client supprimé avec succès.");
                        ChargerClients();
                    }
                }
            }
            else
            {
                MessageBox.Show("Veuillez sélectionner un client à supprimer.");
            }
        }

        private void BtnRechercher_Click(object sender, RoutedEventArgs e)
        {
            using var db = new SmartPharmaDbContext();

            string recherche = txtRecherche.Text.Trim().ToLower();

            var resultats = db.Clients
                .Where(c =>
                    c.Nom.ToLower().Contains(recherche) ||
                    c.Telephone.ToLower().Contains(recherche) ||
                    c.Email.ToLower().Contains(recherche))
                .ToList();

            dgClients.ItemsSource = resultats;
        }

        private void BtnToutAfficher_Click(object sender, RoutedEventArgs e)
        {
            txtRecherche.Clear();
            ChargerClients();
        }

        private void BtnRetour_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}