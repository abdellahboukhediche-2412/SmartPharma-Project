using SmartPharma.Data;
using SmartPharma.Models;
using System.Windows;

namespace SmartPharma.Views
{
    public partial class AjouterClientWindow : Window
    {
        public AjouterClientWindow()
        {
            InitializeComponent();
        }

        private void BtnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNom.Text))
            {
                MessageBox.Show("Le nom du client est obligatoire.");
                return;
            }

            var client = new Client
            {
                Nom = txtNom.Text.Trim(),
                Telephone = txtTelephone.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Adresse = txtAdresse.Text.Trim()
            };

            using var db = new SmartPharmaDbContext();

            db.Clients.Add(client);
            db.SaveChanges();

            MessageBox.Show("Client ajouté avec succès.");
            Close();
        }
    }
}