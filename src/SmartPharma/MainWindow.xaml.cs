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
    }
}