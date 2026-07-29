using SmartPharma.Views;
using System.Windows;

namespace SmartPharma
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void BtnMedicaments_Click(object sender, RoutedEventArgs e)
        {
            new MedicamentsWindow().ShowDialog();
        }

        private void BtnStock_Click(object sender, RoutedEventArgs e)
        {
            new StockWindow().ShowDialog();
        }

        private void BtnVentes_Click(object sender, RoutedEventArgs e)
        {
            new VentesWindow().ShowDialog();
        }

        private void BtnClients_Click(object sender, RoutedEventArgs e)
        {
            var fenetre = new ClientsWindow();
            fenetre.ShowDialog();
        }

        private void BtnFournisseurs_Click(object sender, RoutedEventArgs e)
        {
            var fenetre = new FournisseursWindow();
            fenetre.ShowDialog();
        }

        private void BtnContact_Click(object sender, RoutedEventArgs e)
        {
            ContactWindow contact = new ContactWindow();
            contact.ShowDialog();
        }
    }
}