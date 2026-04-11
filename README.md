# WhackAMole

**Auteur : Philippe Beaulieu**

------------------------------------------------------------------------

## Description brève

WhackAMole est une application de type Whack-a-Mole en réalité virtuelle
(VR) développée avec Unity et le XR Interaction Toolkit.

Le projet permet à l'utilisateur de :

- Attraper un marteau en VR avec les contrôleurs
- Frapper des cibles qui apparaissent aléatoirement sur le plateau
- Gagner des points lorsqu'une cible est touchée
- Jouer pendant une durée limitée
- Voir le score final et rejouer une nouvelle partie

Le projet combine : Interaction VR avec grab cinématique - Détection de
collision entre le marteau et les cibles - Gestion complète de la boucle
de jeu en C#

------------------------------------------------------------------------

## Versions Unity et packages utilisés

### Unity

- Unity 6.3 LTS (6000.3.5f1)

### Packages principaux

- XR Interaction Toolkit 3.3.1
- OpenXR
- XR Plug-in Management
- Input System
- TextMeshPro
- Universal Render Pipeline (URP)

------------------------------------------------------------------------

## Fonctionnement du projet

### Grab du marteau en VR

Le marteau est manipulé avec le XR Interaction Toolkit grâce au
composant XRGrabInteractable.

Le joueur peut donc saisir le marteau directement avec son contrôleur
VR, le garder en main, puis le relâcher volontairement.

------------------------------------------------------------------------

### Apparition des cibles

Les cibles sont gérées par un générateur qui choisit aléatoirement une
cible inactive, la place sur le plateau de jeu et l'affiche pendant une
durée limitée.

Le spawn repose sur une coroutine en C# :

routineSpawn = StartCoroutine(BoucleSpawn());

Chaque cible apparaît ensuite à une position aléatoire sur le plateau
PlateauP.

------------------------------------------------------------------------

### Interaction avec les cibles

Les interactions entre le marteau et les cibles utilisent la détection
de collision via les colliders Unity :


private void OnTriggerEnter(Collider other)


Lorsqu'une cible est frappée par le marteau :

- la cible disparaît
- le score augmente
- un retour haptique est déclenché
- un son peut être joué

------------------------------------------------------------------------

### Logique du jeu

- Gestion d'un score entier
- Gestion d'un minuteur de partie
- Affichage du score en temps réel
- Détection automatique de la fin de partie
- Écran de fin avec possibilité de rejouer
- Menu de départ pour lancer une partie

------------------------------------------------------------------------

## Défis rencontrés et solutions

### 1️ Le marteau ne détectait pas correctement les cibles

Problème : La collision entre le marteau et les cibles n'était pas
détectée de façon fiable.

Solution : - Utilisation d'un collider sur la tête du marteau -
Configuration du tag Marteau - Vérification de OnTriggerEnter dans
le script des cibles

------------------------------------------------------------------------

### 2️ Les références de l'interface n'étaient pas branchées

Problème : Les panneaux UI, textes et boutons n'étaient pas tous
assignés correctement dans l'Inspector.

Solution : - Branchement des références du GameManager - Assignation
des boutons Jouer et Rejouer - Vérification des canvases en World
Space

------------------------------------------------------------------------

### 3️ Les cibles apparaissaient au mauvais endroit

Problème : Les boutons pouvaient apparaître ailleurs que sur la zone de
jeu.

Solution : - Limitation du spawn à la surface du plateau PlateauP -
Calcul d'une position aléatoire dans les limites du plateau -
Repositionnement de la cible avant son affichage

------------------------------------------------------------------------

## Améliorations prévues

- Ajouter de meilleurs effets sonores pour les impacts et la fin de
  partie
- Ajouter des effets visuels lors de l'apparition et de la disparition
  des cibles
- Ajouter plusieurs niveaux de difficulté
- Ajouter un meilleur environnement immersif autour du plateau

------------------------------------------------------------------------

## Annexe : Requêtes utilisées pour écrire du code : IA

- Comment utiliser XR Grab Interactable Unity
- Comment faire un jeu Whack a Mole en Unity
- Comment détecter une collision avec OnTriggerEnter Unity
- Comment envoyer une vibration avec XR Interaction Toolkit
- Comment utiliser XRBaseInputInteractor dans Unity XR Interaction Toolkit
- Comment afficher un score avec TextMeshPro Unity
- Comment faire apparaître des objets aléatoirement sur une zone Unity
- Comment gérer une coroutine de spawn Unity
- Comment faire une boucle de jeu avec menu et game over Unity

------------------------------------------------------------------------

## Plateformes ciblées

- Meta Quest 2
- OpenXR dans Unity Editor

------------------------------------------------------------------------

## Conclusion

Ce projet démontre l'intégration :  D'une mécanique d'un jeu classique de fête foraine en
VR Du grab interactif avec XR Interaction Toolkit - De la détection
de collision - De la gestion d'état - De l'interface utilisateur en
World Space

Il met en pratique les concepts fondamentaux de la réalité virtuelle
avec Unity et le XR Interaction Toolkit.
