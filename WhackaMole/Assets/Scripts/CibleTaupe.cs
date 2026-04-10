using System.Collections;
using UnityEngine;
///https://docs.unity3d.com/6000.0/Documentation/ScriptReference/MonoBehaviour.StartCoroutine.html
///https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Component.GetComponentInParent.html
/// <summary>
/// Gère le comportement d'une taupe cible dans le jeu WhackaMole
/// La taupe peut apparaitre disparaitre après un délai et détecter les impacts du marteau
/// </summary>
public class CibleTaupe : MonoBehaviour
{
    [Header("Score")]
    public int points = 1;

    [Header("Audio")]
    public AudioSource sourceAudio;
    public AudioClip sonApparition;
    public AudioClip sonImpact;

    public bool EstVisible { get; private set; } // Indique si la taupe est visible ou non

    private Coroutine coroutineCache; // Permet de stocker la coroutine active (pour l'arrêter si necessaire)
    /// <summary>
    /// Initialisation au début
    /// Cache immédiatement la taupe
    /// </summary>
    private void Start()
    {
        CacherImmediate();
    }
    /// <summary>
    /// Fait apparaitre la taupe pendant une durée donnée
    /// </summary>
    /// <param name="duree">Temps en secondes avant disparition automatique</param>
    public void Afficher(float duree)
    {
        gameObject.SetActive(true);
        EstVisible = true;

        if (sourceAudio != null && sonApparition != null)
        {
            sourceAudio.spatialBlend = 1f;
            sourceAudio.PlayOneShot(sonApparition);
        }
        // Stoppe la coroutine précédente si elle existe
        if (coroutineCache != null)
            StopCoroutine(coroutineCache);
        // Lance une nouvelle coroutine pour cacher après un délai
        coroutineCache = StartCoroutine(CacherApresDelai(duree));
    }
    /// <summary>
    /// Cache la taupe immédiatement
    /// </summary>
    public void Cacher()
    {
        // Stoppe la coroutine en cours
        if (coroutineCache != null)
            StopCoroutine(coroutineCache);

        EstVisible = false;
        gameObject.SetActive(false);
    }
    /// <summary>
    /// Cache la taupe sans passer par la logique utilisé au début
    /// </summary>
    private void CacherImmediate()
    {
        EstVisible = false;
        gameObject.SetActive(false);
    }

    /// <summary>
    /// Coroutine qui attend un délai avant de cacher la taupe
    /// </summary>
    /// <param name="delai">Temps d'attente en secondes</param>
    private IEnumerator CacherApresDelai(float delai)
    {
        yield return new WaitForSeconds(delai);
        Cacher();
    }
    /// <summary>
    /// Détecte les collisions avec le marteau.
    /// Si la taupe est visible et que la partie est en cours
    /// elle donne des points et joue les effets
    /// </summary>
    /// <param name="other">Collider entrant en collision</param>
    private void OnTriggerEnter(Collider other)
    {
        // Ignore si la taupe n'est pas visible
        if (!EstVisible)
            return;
        // Ignore si la partie n'est pas active
        if (!GestionnaireJeu.Instance.PartieEnCours())
            return;
        // Ignore si ce n'est pas le marteau
        if (!other.CompareTag("Marteau"))
            return;

        // Ajoute les points au score
        GestionnaireJeu.Instance.AjouterScore(points);

        // Déclenche vibration haptique si possible
        MarteauHaptique marteau = other.GetComponentInParent<MarteauHaptique>();
        if (marteau != null)
            marteau.JouerHaptiqueImpact();

        if (sourceAudio != null && sonImpact != null)
        {
            sourceAudio.PlayOneShot(sonImpact);
        }

        Cacher();
    }
}
