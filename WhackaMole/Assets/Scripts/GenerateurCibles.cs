using NUnit.Framework.Internal;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
///https://docs.unity3d.com/2022.3/Documentation/ScriptReference/Bounds-ctor.html
///https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Random.Range.html
/// <summary>
/// Gère la génération aléatoire des cibles sur le plateau
/// Fait apparatre des cibles aléatoirement sur une zone définie
/// S'assure que les cibles ne se chevauchent pas (ne soient pas un par dessus l'autre)
/// </summary>
public class GenerateurCibles : MonoBehaviour
{
    [Header("Cibles")]
    public List<CibleTaupe> cibles = new List<CibleTaupe>();

    [Header("Reglages")]
    public float intervalleSpawn = 1.2f;
    public float dureeVisible = 1.5f;

    [Header("Zone de spawn")]
    public Transform plateauSpawn;
    public float margeBord = 0.03f;
    public float hauteurSpawn = 0.035f;

    private Coroutine routineSpawn;
    private Collider colliderPlateau;
    private Renderer rendererPlateau;

    /// <summary>
    /// Initialisation automatique du plateau et récupération de ses composants
    /// Cherche le plateau s'il n'est pas assigné
    /// Récupère son Collider ou Renderer pour déterminer les limites de spawn
    /// </summary>
    private void Awake()
    {
        // Si le plateau n'est pas assigné dans l'inspecteur
        if (plateauSpawn == null)
        {
            GameObject plateauTrouve = GameObject.Find("PlateauP");
            // Si on trouve l'objet dans la scène
            if (plateauTrouve != null)
                plateauSpawn = plateauTrouve.transform;
        }
        // Si on a un plateau valide
        if (plateauSpawn != null)
        {
            // On récupère le collider et l'autre le Renderer
            colliderPlateau = plateauSpawn.GetComponent<Collider>();
            rendererPlateau = plateauSpawn.GetComponent<Renderer>();
        }
    }
    /// <summary>
    /// Démarre la génération des cibles
    /// Arrête toute génération existante
    /// Cache toutes les cibles
    /// Lance la coroutine de spawn
    /// </summary>
    public void DemarrerGeneration()
    {
        ArreterGeneration();
        CacherToutesLesCibles();
        // Lance la boucle de spawn en coroutine
        routineSpawn = StartCoroutine(BoucleSpawn());
    }
    /// <summary>
    /// Arrête la génération des cibles
    /// Stop la coroutine
    /// Cache toutes les cibles
    /// </summary>
    public void ArreterGeneration()
    {
        // Vérifie si une coroutine de spawn est déjà en cours
        if (routineSpawn != null)
        {
            StopCoroutine(routineSpawn);
            routineSpawn = null;
        }

        CacherToutesLesCibles();
    }
    /// <summary>
    /// Coroutine principale de génération des cibles
    /// Tant que la partie est en cours :
    /// Choisit une cible aléatoire non visible
    /// La positionne sur le plateau
    ///  La rend visible pendant un certain temps
    /// </summary>
    /// <returns>IEnumerator pour la coroutine</returns>
    private IEnumerator BoucleSpawn()
    {
        //tant que la partie est en cours
        while (GestionnaireJeu.Instance.PartieEnCours())
        {
            CibleTaupe cible = ObtenirCibleAleatoire();

            // Vérifie qu'on a bien une cible valide (pas null)
            if (cible != null)
            {
                PositionnerCibleSurPlateau(cible.transform);
                cible.Afficher(dureeVisible);
            }
            // Attente avant le prochain spawn
            yield return new WaitForSeconds(intervalleSpawn);
        }
    }
    /// <summary>
    /// Sélectionne une cible aléatoire parmi celles qui ne sont pas visibles.
    /// Évite de réutiliser une cible déjà affichée
    /// </summary>
    /// <returns>Une cible disponible ou null si aucune</returns>
    private CibleTaupe ObtenirCibleAleatoire()
    {
        List<CibleTaupe> disponibles = new List<CibleTaupe>();

        // Parcours toutes les cibles
        foreach (var cible in cibles)
        {
            // Si la cible n'est pas visible
            if (!cible.EstVisible)
                disponibles.Add(cible);
        }

        if (disponibles.Count == 0)
            return null;

        // Retourne une cible aléatoire dans la liste
        return disponibles[Random.Range(0, disponibles.Count)];
    }
    /// <summary>
    /// Cache toutes les cibles
    /// Utilisé au début et à l'arrêt du jeu
    /// </summary>
    private void CacherToutesLesCibles()
    {
        // Parcourt toutes les cibles
        foreach (var cible in cibles)
        {
            cible.Cacher();
        }
    }
    /// <summary>
    /// Positionne une cible aléatoirement sur le plateau
    /// Utilise les limites du collider ou du renderer
    /// Applique une marge pour éviter les bords
    /// </summary>
    /// <param name="cible">Transform de la cible à déplacer</param>
    private void PositionnerCibleSurPlateau(Transform cible)
    {
        Bounds limites; // Contiendra les limites du plateau

        if (colliderPlateau != null)
        {
            //// Si le plateau a un collider, on utilise ses limites
            limites = colliderPlateau.bounds;
        }
        else if (rendererPlateau != null)
        {
            // Sinon on utilise les limites visuelles du renderer (fallback)
            limites = rendererPlateau.bounds;
        }
        else
        {
            //si aucun des 2 n'existe, on arrete
            return;
        }
        // Définition des limites avec une marge
        float minX = limites.min.x + margeBord;
        float maxX = limites.max.x - margeBord;
        float minZ = limites.min.z + margeBord;
        float maxZ = limites.max.z - margeBord;


        // Génère une position aléatoire dans la zone
        Vector3 positionAleatoire = new Vector3(
            Random.Range(minX, maxX),// Position X aléatoire
            limites.max.y + hauteurSpawn,  // Hauteur au dessus du plateau
            Random.Range(minZ, maxZ)); // Position Z aléatoire

        // Applique la position à la cible
        cible.position = positionAleatoire;
    }
}
