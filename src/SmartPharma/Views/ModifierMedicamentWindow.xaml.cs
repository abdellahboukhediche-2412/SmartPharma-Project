using SmartPharma.Data;
using SmartPharma.Models;
using System.Windows;

namespace SmartPharma.Views
{
    public partial class ModifierMedicamentWindow : Window
    {
        private readonly Medicament _medicament;

        public ModifierMedicamentWindow(Medicament medicament)
        {
            InitializeComponent();

            _medicament = medicament;

            txtNom.Text = medicament.Nom;
            txtPrix.Text = medicament.Prix.ToString();
            txtQuantite.Text = medicament.QuantiteStock.ToString();
            dpExpiration.SelectedDate = medicament.DateExpiration;
        }

        private void BtnModifier_Click(object sender, RoutedEventArgs e)
        {
            using var db = new SmartPharmaDbContext();

            var medicamentDb = db.Medicaments.Find(_medicament.Id);

            if (medicamentDb != null)
            {
                medicamentDb.Nom = txtNom.Text;
                medicamentDb.Prix = decimal.Parse(txtPrix.Text);
                medicamentDb.QuantiteStock = int.Parse(txtQuantite.Text);
                medicamentDb.DateExpiration = dpExpiration.SelectedDate.Value;

                db.SaveChanges();

                MessageBox.Show("Médicament modifié avec succès");

                Close();
            }
        }
    }
}
