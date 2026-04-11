using UnityEngine;
using TMPro;
///https://docs.unity3d.com/ScriptReference/Mathf.CeilToInt.html
/// <summary>
/// Gère le déroulement général de la partie
/// Ce script fait :
/// les panneaux UI (menu, HUD, fin de partie)
/// le score
/// le temps restant
/// le début et la fin de la partie
/// le générateur de cibles
/// </summary>
public class GestionnaireJeu : MonoBehaviour
{
    public static GestionnaireJeu Instance;

    [Header("UI")]
    public GameObject panneauMenu;
    public GameObject panneauFin;
    public GameObject panneauHUD;

    public TMP_Text texteScore;
    public TMP_Text texteTemps;
    public TMP_Text texteScoreFinal;

    [Header("Jeu")]
    public float dureePartie = 60f;
    public GenerateurCibles generateur;

    private int score = 0;
    private float tempsActuel = 0f;
    private bool partieEnCours = false;

    /// <summary>
    /// Initialise l'instance unique du gestionnaire
    /// Empêche la présence de plusieurs GestionnaireJeu dans la scène
    /// </summary>
    private void Awake()
    {
        // Si aucune instance n'existe encore
        if (Instance == null)
        {
            Instance = this;  // On définit cette instance comme la seule (singleton)
        }
        else
        {
            Destroy(gameObject);
        }
    }
    /// <summary>
    /// Au démarrage affiche le menu principal
    /// </summary>
    private void Start()
    {
        AfficherMenu();
    }

    /// <summary>
    /// Met à jour le temps restant pendant la partie
    /// Si le temps atteint 0 la partie se termine
    /// </summary>
    private void Update()
    {
        if (!partieEnCours)
            return;

        tempsActuel -= Time.deltaTime;

        if (tempsActuel <= 0f)
        {
            tempsActuel = 0f;
            TerminerPartie();
        }

        MettreAJourTemps();
    }

    /// <summary>
    /// Démarre une nouvelle partie
    /// Réinitialise le score, le temps et l'interface
    /// puis lance la génération des cibles
    /// </summary>
    public void DemarrerPartie()
    {
        score = 0;
        tempsActuel = dureePartie;
        partieEnCours = true;

        panneauMenu.SetActive(false);
        panneauFin.SetActive(false);
        panneauHUD.SetActive(true);

        MettreAJourScore();
        MettreAJourTemps();
        // Vérifie si le générateur existe puis lance le spawn des cibles
        if (generateur != null)
            generateur.DemarrerGeneration();
    }
    /// <summary>
    /// Termine la partie en cours
    /// Arrête la génération des cibles, masque le HUD
    /// et affiche le panneau de fin avec le score final.
    /// </summary>
    public void TerminerPartie()
    {
        partieEnCours = false;

        if (generateur != null)
            generateur.ArreterGeneration();

        panneauHUD.SetActive(false);
        panneauFin.SetActive(true);

        texteScoreFinal.text = "Score final : " + score;
    }
    /// <summary>
    /// Redémarre une partie 
    /// </summary>
    public void Rejouer()
    {
        DemarrerPartie();
    }
    /// <summary>
    /// Affiche le menu principal
    /// Réinitialise l'état du jeu et arrête la génération des cibles
    /// </summary>
    public void AfficherMenu()
    {
        partieEnCours = false;

        if (generateur != null)
            generateur.ArreterGeneration();

        panneauMenu.SetActive(true);
        panneauHUD.SetActive(false);
        panneauFin.SetActive(false);

        score = 0;
        tempsActuel = dureePartie;

        MettreAJourScore();
        MettreAJourTemps();
    }
    /// <summary>
    /// Ajoute des points au score du joueur si la partie est active
    /// </summary>
    /// <param name="points">Nombre de points à ajouter</param>
    public void AjouterScore(int points)
    {
        if (!partieEnCours)
            return;

        score += points;
        MettreAJourScore();
    }
    /// <summary>
    /// Indique si une partie est actuellement en cours
    /// </summary>
    /// <returns> si la partie est en cours c'est true, si elle n'est pas en cours c'est false</returns>
    public bool PartieEnCours()
    {
        return partieEnCours;
    }
    /// <summary>
    /// Met à jour l'affichage du score dans le HUD
    /// </summary>
    private void MettreAJourScore()
    {
        texteScore.text = "Score : " + score;
    }
    /// <summary>
    /// Met à jour l'affichage du temps restant dans le HUD
    /// Le temps est arrondi à l'entier supérieur pour un affichage plus clair
    /// </summary>
    private void MettreAJourTemps()
    {
        texteTemps.text = "Temps : " + Mathf.CeilToInt(tempsActuel);
    }
}
