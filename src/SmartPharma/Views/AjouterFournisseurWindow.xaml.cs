using SmartPharma.Data;
using SmartPharma.Models;
using System.Windows;

namespace SmartPharma.Views
{
    public partial class AjouterFournisseurWindow : Window
    {
        public AjouterFournisseurWindow()
        {
            InitializeComponent();
        }

        private void BtnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNom.Text))
            {
                MessageBox.Show("Le nom du fournisseur est obligatoire.");
                return;
            }

            var fournisseur = new Fournisseur
            {
                Nom = txtNom.Text.Trim(),
                Entreprise = txtEntreprise.Text.Trim(),
                Telephone = txtTelephone.Text.Trim(),
                Email = txtEmail.Text.Trim(),
                Adresse = txtAdresse.Text.Trim()
            };

            using var db = new SmartPharmaDbContext();

            db.Fournisseurs.Add(fournisseur);
            db.SaveChanges();

            MessageBox.Show("Fournisseur ajouté avec succès.");

            DialogResult = true;
            Close();
        }

        private void BtnAnnuler_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}