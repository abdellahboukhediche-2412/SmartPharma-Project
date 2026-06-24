using SmartPharma.Data;
using SmartPharma.Models;
using System.Windows;

namespace SmartPharma.Views
{
    public partial class AjouterMedicamentWindow : Window
    {
        public AjouterMedicamentWindow()
        {
            InitializeComponent();
        }

        private void BtnEnregistrer_Click(object sender, RoutedEventArgs e)
        {
            using var db = new SmartPharmaDbContext();

            var medicament = new Medicament
            {
                Nom = txtNom.Text,
                Prix = decimal.Parse(txtPrix.Text),
                QuantiteStock = int.Parse(txtQuantite.Text),
                DateExpiration = dpExpiration.SelectedDate.Value
            };

            db.Medicaments.Add(medicament);

            db.SaveChanges();

            MessageBox.Show("Médicament ajouté avec succès");

            Close();
        }
    }
}
