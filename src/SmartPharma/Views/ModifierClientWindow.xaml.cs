using SmartPharma.Data;
using SmartPharma.Models;
using System.Windows;

namespace SmartPharma.Views
{
    public partial class ModifierClientWindow : Window
    {
        private readonly int _clientId;

        public ModifierClientWindow(Client client)
        {
            InitializeComponent();

            _clientId = client.Id;

            txtNom.Text = client.Nom;
            txtTelephone.Text = client.Telephone;
            txtEmail.Text = client.Email;
            txtAdresse.Text = client.Adresse;
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNom.Text))
            {
                MessageBox.Show(
                    "Le nom du client est obligatoire.",
                    "Champ obligatoire",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            using var db = new SmartPharmaDbContext();

            var clientDb = db.Clients.Find(_clientId);

            if (clientDb == null)
            {
                MessageBox.Show(
                    "Le client est introuvable dans la base de données.",
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            clientDb.Nom = txtNom.Text.Trim();
            clientDb.Telephone = txtTelephone.Text.Trim();
            clientDb.Email = txtEmail.Text.Trim();
            clientDb.Adresse = txtAdresse.Text.Trim();

            db.SaveChanges();

            MessageBox.Show(
                "Les informations du client ont été modifiées avec succès.",
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