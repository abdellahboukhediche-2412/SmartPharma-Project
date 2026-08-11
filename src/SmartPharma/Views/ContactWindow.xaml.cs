using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Configuration;
using MimeKit;
using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace SmartPharma.Views
{
    public partial class ContactWindow : Window
    {
        private readonly IConfiguration _configuration;

        public ContactWindow()
        {
            InitializeComponent();

            _configuration = new ConfigurationBuilder()
                .AddUserSecrets<ContactWindow>()
                .Build();

            // Compteur automatique du message
            txtMessage.TextChanged += TxtMessage_TextChanged;
        }

        // =====================================================
        // VALIDATION DU FORMULAIRE
        // =====================================================

        private bool ValiderFormulaire()
        {
            bool valide = true;

            // Masquer les anciennes erreurs
            erreurNom.Visibility = Visibility.Collapsed;
            erreurEmail.Visibility = Visibility.Collapsed;
            erreurSujet.Visibility = Visibility.Collapsed;
            erreurMessage.Visibility = Visibility.Collapsed;

            messageSucces.Visibility = Visibility.Collapsed;
            messageErreur.Visibility = Visibility.Collapsed;

            // =================================================
            // NOM
            // =================================================

            if (string.IsNullOrWhiteSpace(txtNom.Text))
            {
                erreurNom.Text = "Champ obligatoire";
                erreurNom.Visibility = Visibility.Visible;
                valide = false;
            }
            else if (txtNom.Text.Trim().Length < 2)
            {
                erreurNom.Text = "Minimum 2 caractères";
                erreurNom.Visibility = Visibility.Visible;
                valide = false;
            }

            // =================================================
            // COURRIEL
            // =================================================

            if (string.IsNullOrWhiteSpace(txtEmail.Text))
            {
                erreurEmail.Text = "Champ obligatoire";
                erreurEmail.Visibility = Visibility.Visible;
                valide = false;
            }
            else if (!EmailValide(txtEmail.Text.Trim()))
            {
                erreurEmail.Text = "Adresse courriel invalide";
                erreurEmail.Visibility = Visibility.Visible;
                valide = false;
            }

            // =================================================
            // SUJET
            // =================================================

            if (cmbSujet.SelectedItem == null)
            {
                erreurSujet.Text = "Veuillez sélectionner un sujet";
                erreurSujet.Visibility = Visibility.Visible;
                valide = false;
            }

            // =================================================
            // MESSAGE
            // =================================================

            if (string.IsNullOrWhiteSpace(txtMessage.Text))
            {
                erreurMessage.Text = "Champ obligatoire";
                erreurMessage.Visibility = Visibility.Visible;
                valide = false;
            }
            else if (txtMessage.Text.Trim().Length > 500)
            {
                erreurMessage.Text = "Maximum 500 caractères";
                erreurMessage.Visibility = Visibility.Visible;
                valide = false;
            }

            return valide;
        }

        // =====================================================
        // VALIDATION DU COURRIEL
        // =====================================================

        private bool EmailValide(string email)
        {
            try
            {
                MailboxAddress.Parse(email);
                return true;
            }
            catch
            {
                return false;
            }
        }

        // =====================================================
        // COMPTEUR MESSAGE
        // =====================================================

        private void TxtMessage_TextChanged(
            object sender,
            TextChangedEventArgs e)
        {
            int nombreCaracteres = txtMessage.Text.Length;

            txtCompteur.Text =
                $"{nombreCaracteres} / 500";

            if (nombreCaracteres > 500)
            {
                txtCompteur.Foreground =
                    Brushes.Red;
            }
            else
            {
                txtCompteur.Foreground =
                    Brushes.Gray;
            }
        }

        // =====================================================
        // RÉCUPÉRER LE SUJET
        // =====================================================

        private string ObtenirSujet()
        {
            if (cmbSujet.SelectedItem is ComboBoxItem item)
            {
                return item.Content?.ToString()
                    ?? "Contact SmartPharma";
            }

            return "Contact SmartPharma";
        }

        // =====================================================
        // BOUTON ENVOYER
        // =====================================================

        private async void BtnEnvoyer_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (!ValiderFormulaire())
            {
                return;
            }

            btnEnvoyer.IsEnabled = false;
            btnEnvoyer.Content = "Envoi en cours...";

            try
            {
                // 1. Message reçu par SmartPharma
                await EnvoyerMessageAdministrateur();

                // 2. Confirmation automatique au client
                await EnvoyerConfirmationClient();

                messageSucces.Visibility =
                    Visibility.Visible;

                messageErreur.Visibility =
                    Visibility.Collapsed;

                // Réinitialiser le formulaire
                txtNom.Clear();
                txtEmail.Clear();
                txtMessage.Clear();

                cmbSujet.SelectedIndex = -1;

                txtCompteur.Text = "0 / 500";
                txtCompteur.Foreground = Brushes.Gray;

                // Masquer le message après 2 secondes
                await Task.Delay(2000);

                messageSucces.Visibility =
                    Visibility.Collapsed;
            }
            catch (Exception ex)
            {
                messageErreur.Visibility =
                    Visibility.Visible;

                messageSucces.Visibility =
                    Visibility.Collapsed;

                MessageBox.Show(
                    "Erreur lors de l'envoi :\n\n" +
                    ex.Message,
                    "SmartPharma",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
            finally
            {
                btnEnvoyer.IsEnabled = true;

                btnEnvoyer.Content =
                    "📩  Envoyer";
            }
        }

        // =====================================================
        // MESSAGE ENVOYÉ À L'ADMINISTRATEUR
        // =====================================================

        private async Task EnvoyerMessageAdministrateur()
        {
            string smtpHost =
                _configuration["Email:SmtpHost"] ?? "";

            string smtpPortTexte =
                _configuration["Email:SmtpPort"] ?? "587";

            string username =
                _configuration["Email:Username"] ?? "";

            string password =
                _configuration["Email:Password"] ?? "";

            string destination =
                _configuration["Email:Destination"] ?? "";

            // Vérification configuration
            if (string.IsNullOrWhiteSpace(smtpHost) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password) ||
                string.IsNullOrWhiteSpace(destination))
            {
                throw new Exception(
                    "La configuration du service de courriel est incomplète.");
            }

            if (!int.TryParse(
                    smtpPortTexte,
                    out int smtpPort))
            {
                smtpPort = 587;
            }

            string sujet = ObtenirSujet();

            var message = new MimeMessage();

            // Expéditeur
            message.From.Add(
                new MailboxAddress(
                    "SmartPharma",
                    username));

            // Destinataire
            message.To.Add(
                new MailboxAddress(
                    "SmartPharma",
                    destination));

            // Réponse directement au client
            message.ReplyTo.Add(
                new MailboxAddress(
                    txtNom.Text.Trim(),
                    txtEmail.Text.Trim()));

            // Sujet du courriel
            message.Subject =
                $"SmartPharma - {sujet} - {txtNom.Text.Trim()}";

            // Corps du courriel
            message.Body = new TextPart("plain")
            {
                Text =
                    "Nouveau message reçu depuis SmartPharma\n\n" +

                    "----------------------------------------\n\n" +

                    $"Nom : {txtNom.Text.Trim()}\n\n" +

                    $"Courriel : {txtEmail.Text.Trim()}\n\n" +

                    $"Sujet : {sujet}\n\n" +

                    "Message :\n" +
                    txtMessage.Text.Trim() +

                    "\n\n----------------------------------------"
            };

            using var client =
                new SmtpClient();

            // Correction SSL/TLS
            client.CheckCertificateRevocation =
                false;

            await client.ConnectAsync(
                smtpHost,
                smtpPort,
                SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(
                username,
                password);

            await client.SendAsync(message);

            await client.DisconnectAsync(true);
        }

        // =====================================================
        // CONFIRMATION AUTOMATIQUE AU CLIENT
        // =====================================================

        private async Task EnvoyerConfirmationClient()
        {
            string smtpHost =
                _configuration["Email:SmtpHost"] ?? "";

            string smtpPortTexte =
                _configuration["Email:SmtpPort"] ?? "587";

            string username =
                _configuration["Email:Username"] ?? "";

            string password =
                _configuration["Email:Password"] ?? "";

            if (string.IsNullOrWhiteSpace(smtpHost) ||
                string.IsNullOrWhiteSpace(username) ||
                string.IsNullOrWhiteSpace(password))
            {
                throw new Exception(
                    "La configuration du service de courriel est incomplète.");
            }

            if (!int.TryParse(
                    smtpPortTexte,
                    out int smtpPort))
            {
                smtpPort = 587;
            }

            string sujet =
                ObtenirSujet();

            var confirmation =
                new MimeMessage();

            // Expéditeur
            confirmation.From.Add(
                new MailboxAddress(
                    "SmartPharma",
                    username));

            // Destinataire = client
            confirmation.To.Add(
                new MailboxAddress(
                    txtNom.Text.Trim(),
                    txtEmail.Text.Trim()));

            confirmation.Subject =
                $"Confirmation - {sujet} - SmartPharma";

            confirmation.Body =
                new TextPart("plain")
                {
                    Text =
                        $"Bonjour {txtNom.Text.Trim()},\n\n" +

                        "Nous avons bien reçu votre message.\n\n" +

                        $"Sujet : {sujet}\n\n" +

                        "Notre équipe vous répondra dans les meilleurs délais.\n\n" +

                        "Votre message :\n" +

                        "----------------------------------------\n" +

                        txtMessage.Text.Trim() +

                        "\n----------------------------------------\n\n" +

                        "Merci d'avoir contacté SmartPharma.\n\n" +

                        "Cordialement,\n" +

                        "L'équipe SmartPharma"
                };

            using var client =
                new SmtpClient();

            // Correction SSL/TLS
            client.CheckCertificateRevocation =
                false;

            await client.ConnectAsync(
                smtpHost,
                smtpPort,
                SecureSocketOptions.StartTls);

            await client.AuthenticateAsync(
                username,
                password);

            await client.SendAsync(
                confirmation);

            await client.DisconnectAsync(true);
        }

        // =====================================================
        // RETOUR
        // =====================================================

        private void BtnRetour_Click(
            object sender,
            RoutedEventArgs e)
        {
            Close();
        }
    }
}