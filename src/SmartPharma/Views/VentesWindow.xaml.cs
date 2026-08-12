using Microsoft.EntityFrameworkCore;
using SmartPharma.Data;
using SmartPharma.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Controls;

namespace SmartPharma.Views
{
    // =====================================================
    // MODÈLE D'AFFICHAGE POUR L'HISTORIQUE
    // =====================================================

    public class VenteHistoriqueViewModel
    {
        public int Id { get; set; }

        public DateTime DateVente { get; set; }

        public decimal MontantTotal { get; set; }

        public int QuantiteTotale { get; set; }

        public string Medicaments { get; set; } = string.Empty;
    }

    public partial class VentesWindow : Window
    {
        private readonly List<LigneVente> panier = new();

        private readonly CultureInfo cultureCanada =
            new CultureInfo("fr-CA");

        public VentesWindow()
        {
            InitializeComponent();

            ChargerMedicaments();
            ChargerVentes();
            ActualiserPanier();
        }

        // =====================================================
        // CHARGER LES MÉDICAMENTS
        // =====================================================

        private void ChargerMedicaments()
        {
            using var db = new SmartPharmaDbContext();

            cmbMedicaments.ItemsSource = db.Medicaments
                .OrderBy(m => m.Nom)
                .ToList();

            cmbMedicaments.SelectedIndex = -1;

            txtPrix.Clear();
            txtStock.Clear();
            txtExpiration.Clear();
            txtQuantite.Clear();
        }

        // =====================================================
        // CHARGER L'HISTORIQUE
        // =====================================================

        private void ChargerVentes()
        {
            using var db = new SmartPharmaDbContext();

            var ventes = db.Ventes
                .Include(v => v.Lignes)
                .ThenInclude(l => l.Medicament)
                .OrderByDescending(v => v.DateVente)
                .ToList();

            var historique = ventes
                .Select(v => new VenteHistoriqueViewModel
                {
                    Id = v.Id,

                    DateVente = v.DateVente,

                    MontantTotal = v.MontantTotal,

                    QuantiteTotale =
                        v.Lignes.Sum(l => l.Quantite),

                    Medicaments = v.Lignes.Count == 0
                        ? "Aucun détail disponible"
                        : string.Join(
                            ", ",
                            v.Lignes.Select(l =>
                                $"{l.Medicament.Nom} x{l.Quantite}"))
                })
                .ToList();

            dgVentes.ItemsSource = historique;
        }

        // =====================================================
        // CHANGEMENT DU MÉDICAMENT
        // =====================================================

        private void cmbMedicaments_SelectionChanged(
            object sender,
            SelectionChangedEventArgs e)
        {
            if (cmbMedicaments.SelectedItem is Medicament medicament)
            {
                txtPrix.Text =
                    medicament.Prix.ToString(
                        "C2",
                        cultureCanada);

                txtStock.Text =
                    medicament.QuantiteStock.ToString();

                txtExpiration.Text =
                    medicament.DateExpiration
                        .ToString("dd-MM-yyyy");
            }
            else
            {
                txtPrix.Clear();
                txtStock.Clear();
                txtExpiration.Clear();
            }
        }

        // =====================================================
        // AJOUTER AU PANIER
        // =====================================================

        private void BtnAjouterPanier_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (cmbMedicaments.SelectedItem
                is not Medicament medicament)
            {
                MessageBox.Show(
                    "Veuillez sélectionner un médicament.",
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

            int quantiteDejaPanier = panier
                .Where(l =>
                    l.MedicamentId == medicament.Id)
                .Sum(l => l.Quantite);

            if (quantite + quantiteDejaPanier >
                medicament.QuantiteStock)
            {
                MessageBox.Show(
                    $"Stock insuffisant.\n\n" +
                    $"Stock disponible : {medicament.QuantiteStock}\n" +
                    $"Déjà dans le panier : {quantiteDejaPanier}",
                    "Stock insuffisant",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            var ligneExistante = panier
                .FirstOrDefault(l =>
                    l.MedicamentId == medicament.Id);

            if (ligneExistante != null)
            {
                ligneExistante.Quantite += quantite;

                ligneExistante.SousTotal =
                    ligneExistante.PrixUnitaire *
                    ligneExistante.Quantite;
            }
            else
            {
                var ligne = new LigneVente
                {
                    MedicamentId = medicament.Id,
                    Medicament = medicament,
                    Quantite = quantite,
                    PrixUnitaire = medicament.Prix,
                    SousTotal =
                        medicament.Prix * quantite
                };

                panier.Add(ligne);
            }

            ActualiserPanier();

            cmbMedicaments.SelectedIndex = -1;

            txtQuantite.Clear();
            txtPrix.Clear();
            txtStock.Clear();
            txtExpiration.Clear();

            cmbMedicaments.Focus();
        }

        // =====================================================
        // SUPPRIMER DU PANIER
        // =====================================================

        private void BtnSupprimerPanier_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (dgPanier.SelectedItem
                is not LigneVente ligne)
            {
                MessageBox.Show(
                    "Veuillez sélectionner un médicament dans le panier.",
                    "SmartPharma",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            panier.Remove(ligne);

            ActualiserPanier();
        }

        // =====================================================
        // ACTUALISER LE PANIER
        // =====================================================

        private void ActualiserPanier()
        {
            dgPanier.ItemsSource = null;
            dgPanier.ItemsSource = panier;

            decimal total =
                panier.Sum(l => l.SousTotal);

            txtTotal.Text =
                total.ToString(
                    "C2",
                    cultureCanada);
        }

        // =====================================================
        // ENREGISTRER LA VENTE
        // =====================================================

        private void BtnEnregistrerVente_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (panier.Count == 0)
            {
                MessageBox.Show(
                    "Le panier est vide.\nAjoutez au moins un médicament.",
                    "SmartPharma",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            using var db =
                new SmartPharmaDbContext();

            using var transaction =
                db.Database.BeginTransaction();

            try
            {
                // Vérification finale du stock
                foreach (var ligne in panier)
                {
                    var medicamentDb =
                        db.Medicaments.Find(
                            ligne.MedicamentId);

                    if (medicamentDb == null)
                    {
                        MessageBox.Show(
                            "Un médicament du panier est introuvable.");

                        transaction.Rollback();
                        return;
                    }

                    if (medicamentDb.QuantiteStock <
                        ligne.Quantite)
                    {
                        MessageBox.Show(
                            $"Stock insuffisant pour : {medicamentDb.Nom}\n\n" +
                            $"Disponible : {medicamentDb.QuantiteStock}\n" +
                            $"Demandé : {ligne.Quantite}",
                            "Stock insuffisant",
                            MessageBoxButton.OK,
                            MessageBoxImage.Warning);

                        transaction.Rollback();
                        return;
                    }
                }

                decimal montantTotal =
                    panier.Sum(l => l.SousTotal);

                // =============================================
                // CRÉER LA VENTE
                // =============================================

                var vente = new Vente
                {
                    DateVente = DateTime.Now,
                    MontantTotal = montantTotal
                };

                db.Ventes.Add(vente);

                db.SaveChanges();

                // =============================================
                // AJOUTER LES LIGNES
                // =============================================

                foreach (var lignePanier in panier)
                {
                    var medicamentDb =
                        db.Medicaments.Find(
                            lignePanier.MedicamentId);

                    if (medicamentDb == null)
                    {
                        continue;
                    }

                    var ligneVente =
                        new LigneVente
                        {
                            VenteId = vente.Id,

                            MedicamentId =
                                medicamentDb.Id,

                            Quantite =
                                lignePanier.Quantite,

                            PrixUnitaire =
                                lignePanier.PrixUnitaire,

                            SousTotal =
                                lignePanier.SousTotal
                        };

                    db.LignesVente.Add(
                        ligneVente);

                    // Diminuer le stock
                    medicamentDb.QuantiteStock -=
                        lignePanier.Quantite;
                }

                db.SaveChanges();

                transaction.Commit();

                MessageBox.Show(
                    $"Vente enregistrée avec succès.\n\n" +
                    $"Nombre de médicaments différents : {panier.Count}\n" +
                    $"Quantité totale : {panier.Sum(l => l.Quantite)}\n" +
                    $"Montant total : {montantTotal.ToString("C2", cultureCanada)}",
                    "Vente enregistrée",
                    MessageBoxButton.OK,
                    MessageBoxImage.Information);

                // =============================================
                // NOUVELLE VENTE
                // =============================================

                panier.Clear();

                ActualiserPanier();
                ChargerMedicaments();

                // L'historique se recharge immédiatement
                ChargerVentes();
            }
            catch (Exception ex)
            {
                transaction.Rollback();

                MessageBox.Show(
                    "Une erreur est survenue lors de l'enregistrement :\n\n" +
                    ex.Message,
                    "Erreur",
                    MessageBoxButton.OK,
                    MessageBoxImage.Error);
            }
        }


        private void BtnVoirDetails_Click(
                object sender,
                RoutedEventArgs e)
        {
            if (dgVentes.SelectedItem
                is not VenteHistoriqueViewModel venteSelectionnee)
            {
                MessageBox.Show(
                    "Veuillez sélectionner une vente dans l'historique.",
                    "SmartPharma",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);

                return;
            }

            var fenetre =
                new DetailsVenteWindow(
                    venteSelectionnee.Id);

            fenetre.ShowDialog();
        }
        // =====================================================
        // RETOUR
        // =====================================================

        private void BtnRetour_Click(
            object sender,
            RoutedEventArgs e)
        {
            if (panier.Count > 0)
            {
                var resultat =
                    MessageBox.Show(
                        "Une vente est en cours.\n" +
                        "Le panier sera perdu si vous quittez.\n\n" +
                        "Voulez-vous continuer ?",
                        "Vente en cours",
                        MessageBoxButton.YesNo,
                        MessageBoxImage.Question);

                if (resultat !=
                    MessageBoxResult.Yes)
                {
                    return;
                }
            }

            Close();
        }
    }
}