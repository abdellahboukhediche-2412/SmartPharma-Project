using SmartPharma.Data;
using SmartPharma.Models;
using System.Windows;

namespace SmartPharma.Views
{
    public partial class ModifierQuantiteWindow : Window
    {
        private readonly int _medicamentId;

        public ModifierQuantiteWindow(int medicamentId, string nom, int quantite)
        {
            InitializeComponent();

            _medicamentId = medicamentId;
            txtNom.Text = nom;
            txtQuantite.Text = quantite.ToString();
        }

        private void BtnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(txtQuantite.Text, out int nouvelleQuantite))
            {
                MessageBox.Show("Veuillez entrer une quantité valide.");
                return;
            }

            using var db = new SmartPharmaDbContext();

            var medicament = db.Medicaments.Find(_medicamentId);

            if (medicament != null)
            {
                medicament.QuantiteStock = nouvelleQuantite;
                db.SaveChanges();

                MessageBox.Show("Quantité modifiée avec succès.");
                Close();
            }
        }
    }
}