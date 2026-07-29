using SmartPharma.Data;
using SmartPharma.Models;
using System.Windows;

namespace SmartPharma.Views
{
    public partial class ModifierFournisseurWindow : Window
    {
        private readonly int _fournisseurId;

        public ModifierFournisseurWindow(Fournisseur fournisseur)
        {
            InitializeComponent();

            _fournisseurId = fournisseur.Id;

            txtNom.Text = fournisseur.Nom;
            txtEntreprise.Text = fournisseur.Entreprise;
            txtTelephone.Text = fournisseur.Telephone;
            txtEmail.Text = fournisseur.Email;
            txtAdresse.Text = fournisseur.Adresse;
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtNom.Text))
            {
                MessageBox.Show(
                    "Le nom du fournisseur est obligatoire.",
                    "Champ obligatoire",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            using var db = new SmartPharmaDbContext();

            var fournisseurDb = db.Fournisseurs.Find(_fournisseurId);

            if (fournisseurDb == null)
            {
                MessageBox.Show(
                    "Le fournisseur est introuvable dans la base de données.",
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            fournisseurDb.Nom = txtNom.Text.Trim();
            fournisseurDb.Entreprise = txtEntreprise.Text.Trim();
            fournisseurDb.Telephone = txtTelephone.Text.Trim();
            fournisseurDb.Email = txtEmail.Text.Trim();
            fournisseurDb.Adresse = txtAdresse.Text.Trim();

            db.SaveChanges();

            MessageBox.Show(
                "Le fournisseur a été modifié avec succès.",
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