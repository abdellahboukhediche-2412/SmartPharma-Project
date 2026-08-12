using SmartPharma.Data;
using SmartPharma.Helpers;
using System.IO;
using System.Linq;
using System.Windows;

namespace SmartPharma.Views
{
    public partial class LoginWindow : Window
    {
        private readonly string cheminMemoire =
            Path.Combine(
                Environment.GetFolderPath(
                    Environment.SpecialFolder.ApplicationData),
                "SmartPharma",
                "utilisateur.txt");

        public LoginWindow()
        {
            InitializeComponent();

            ChargerUtilisateurMemorise();
        }

        private void ChargerUtilisateurMemorise()
        {
            try
            {
                if (File.Exists(cheminMemoire))
                {
                    string nomUtilisateur =
                        File.ReadAllText(cheminMemoire);

                    if (!string.IsNullOrWhiteSpace(nomUtilisateur))
                    {
                        txtNomUtilisateur.Text =
                            nomUtilisateur;

                        chkSeSouvenir.IsChecked = true;

                        txtMotDePasse.Focus();
                    }
                }
            }
            catch
            {
                // On ignore si le fichier ne peut pas être lu
            }
        }

        private void MemoriserUtilisateur(
            string nomUtilisateur)
        {
            try
            {
                string? dossier =
                    Path.GetDirectoryName(cheminMemoire);

                if (!string.IsNullOrWhiteSpace(dossier))
                {
                    Directory.CreateDirectory(dossier);
                }

                if (chkSeSouvenir.IsChecked == true)
                {
                    File.WriteAllText(
                        cheminMemoire,
                        nomUtilisateur);
                }
                else
                {
                    if (File.Exists(cheminMemoire))
                    {
                        File.Delete(cheminMemoire);
                    }
                }
            }
            catch
            {
                // La connexion continue même si la mémorisation échoue
            }
        }

        private void BtnConnexion_Click(
            object sender,
            RoutedEventArgs e)
        {
            string nomUtilisateur =
                txtNomUtilisateur.Text.Trim();

            string motDePasse =
                txtMotDePasse.Password;

            txtErreur.Visibility =
                Visibility.Collapsed;

            if (string.IsNullOrWhiteSpace(nomUtilisateur) ||
                string.IsNullOrWhiteSpace(motDePasse))
            {
                AfficherErreur(
                    "Veuillez saisir votre nom d'utilisateur et votre mot de passe.");

                return;
            }

            using var db =
                new SmartPharmaDbContext();

            var utilisateur =
                db.Utilisateurs
                    .FirstOrDefault(u =>
                        u.NomUtilisateur == nomUtilisateur &&
                        u.MotDePasse == motDePasse &&
                        u.Actif);

            if (utilisateur == null)
            {
                AfficherErreur(
                    "Nom d'utilisateur ou mot de passe incorrect.");

                return;
            }

            SessionUtilisateur.UtilisateurConnecte =
                utilisateur;

            MemoriserUtilisateur(
                nomUtilisateur);

            var mainWindow =
                new MainWindow();

            mainWindow.Show();

            Close();
        }

        private void AfficherErreur(
            string message)
        {
            txtErreur.Text =
                message;

            txtErreur.Visibility =
                Visibility.Visible;
        }

        private void BtnQuitter_Click(
            object sender,
            RoutedEventArgs e)
        {
            Application.Current.Shutdown();
        }
    }
}