# Dossier de conception - SmartPharma

## Diagramme de cas d'utilisation
Diagramme de cas d’utilisation : remplacer les quatre acteurs Pharmacien / Employé / Gestionnaire / Administrateur par un seul acteur Utilisateur de la pharmacie, puisque tous ont désormais le même accès. Les cas d’utilisation doivent être : se connecter, gérer les médicaments, consulter le stock, effectuer une vente, gérer les clients, gérer les fournisseurs, consulter les rapports, gérer les utilisateurs, utiliser le formulaire Contact, se déconnecter. Ton document actuel décrit encore plusieurs catégories d’utilisateurs avec des accès différents.

## Diagramme de classes
Diagramme de classes : supprimer ou mettre de côté Pharmacien et Ordonnance si ces classes ne sont pas réellement implémentées. Ajouter au minimum Utilisateur, Medicament, Client, Fournisseur, Vente et surtout LigneVente. La relation importante est désormais Vente 1 → N LigneVente et Medicament 1 → N LigneVente. C’est cette structure qui permet à une seule vente de contenir plusieurs médicaments.

## Diagramme de séquence
Diagramme de séquence : remplacer le scénario « vente d’un médicament » par vente de plusieurs médicaments. Après connexion, l’utilisateur sélectionne un médicament, saisit la quantité, l’ajoute au panier, répète éventuellement l’opération pour d’autres médicaments, puis SmartPharma calcule le total général, crée la vente, crée toutes les lignes de vente et met à jour le stock de chaque médicament.

## Diagramme d'activité
Diagramme d’activité : ajouter une boucle après « Ajouter au panier » avec la décision Ajouter un autre médicament ?. Si oui, retour à la sélection d’un médicament. Si non, calcul du total → enregistrement de la vente → création des lignes de vente → mise à jour des stocks → affichage dans l’historique.