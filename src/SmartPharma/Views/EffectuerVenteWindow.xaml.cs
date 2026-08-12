using SmartPharma.Data;
using SmartPharma.Models;
using System;
using System.Linq;
using System.Windows;

namespace SmartPharma.Views
{
    public partial class EffectuerVenteWindow : Window
    {
        private decimal total = 0;

        public EffectuerVenteWindow()
        {
            InitializeComponent();
            ChargerMedicaments();
        }

        // =====================================================
        // CHARGER LES MÉDICAMENTS
        // =====================================================

        private void ChargerMedicaments()
        {
            using var db = new SmartPharmaDbContext();

            cmbMedicaments.ItemsSource =
                db.Medicaments
                    .OrderBy(m => m.Nom)
                    .ToList();
        }

        // =====================================================
        // CALCULER LE TOTAL
        // =====================================================

        private void BtnCalculer_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (cmbMedicaments.SelectedItem
                    is not Medicament medicament)
            {
                MessageBox.Show(
                    "Veuillez choisir un médicament.",
                    "SmartPharma",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (!int.TryParse(
                    txtQuantite.Text,
                    out int quantite) ||
                quantite <= 0)
            {
                MessageBox.Show(
                    "Veuillez entrer une quantité valide.",
                    "SmartPharma",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (quantite > medicament.QuantiteStock)
            {
                MessageBox.Show(
                    "Stock insuffisant.",
                    "SmartPharma",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            total =
                medicament.Prix * quantite;

            txtTotal.Text =
                $"Total : {total:N2} $";
        }

        // =====================================================
        // ENREGISTRER LA VENTE
        // =====================================================

        private void BtnEnregistrerVente_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (cmbMedicaments.SelectedItem
                    is not Medicament medicament)
            {
                MessageBox.Show(
                    "Veuillez choisir un médicament.",
                    "SmartPharma",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            if (!int.TryParse(
                    txtQuantite.Text,
                    out int quantite) ||
                quantite <= 0)
            {
                MessageBox.Show(
                    "Veuillez entrer une quantité valide.",
                    "SmartPharma",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            using var db =
                new SmartPharmaDbContext();

            var medicamentDb =
                db.Medicaments.Find(medicament.Id);

            if (medicamentDb == null)
            {
                MessageBox.Show(
                    "Médicament introuvable.",
                    "SmartPharma",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);

                return;
            }

            if (medicamentDb.QuantiteStock < quantite)
            {
                MessageBox.Show(
                    "Stock insuffisant.",
                    "SmartPharma",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            decimal montantTotal =
                medicamentDb.Prix * quantite;

            // =============================================
            // 1. CRÉER LA VENTE
            // =============================================

            var vente = new Vente
            {
                DateVente = DateTime.Now,
                MontantTotal = montantTotal
            };

            db.Ventes.Add(vente);

            // On sauvegarde pour obtenir vente.Id
            db.SaveChanges();

            // =============================================
            // 2. CRÉER LA LIGNE DE VENTE
            // =============================================

            var ligneVente = new LigneVente
            {
                VenteId = vente.Id,

                MedicamentId =
                    medicamentDb.Id,

                Quantite =
                    quantite,

                PrixUnitaire =
                    medicamentDb.Prix,

                SousTotal =
                    montantTotal
            };

            db.LignesVente.Add(ligneVente);

            // =============================================
            // 3. METTRE LE STOCK À JOUR
            // =============================================

            medicamentDb.QuantiteStock -=
                quantite;

            db.SaveChanges();

            MessageBox.Show(
                "Vente enregistrée avec succès.",
                "SmartPharma",
                MessageBoxButton.OK,
                MessageBoxImage.Information);

            Close();
        }
    }
}