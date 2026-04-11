using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;
using UnityEngine.XR.Interaction.Toolkit.Interactors;
//https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.2/api/UnityEngine.XR.Interaction.Toolkit.SelectEnterEventArgs.html
//https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@2.0/api/UnityEngine.XR.Interaction.Toolkit.XRController.html
//https://docs.unity3d.com/Packages/com.unity.xr.interaction.toolkit@3.0/api/UnityEngine.XR.Interaction.Toolkit.Interactors.XRBaseInputInteractor.html
/// <summary>
/// Gère les vibrations haptiques du marteau en VR
/// Lorsqu'on attrape le marteau (sensation léger)
/// Lorsqu'on frappe une cible (sensation plus fort)
/// </summary>
public class MarteauHaptique : MonoBehaviour
{
    public XRGrabInteractable grabInteractable; // Permet d'interagir donc prendre et lâcher le marteau

    [Header("Grab")]
    public float amplitudeGrab = 0.25f;
    public float dureeGrab = 0.08f;

    [Header("Impact")]
    public float amplitudeImpact = 0.7f;
    public float dureeImpact = 0.12f;

    private XRBaseInputInteractor interactorActuel;  // Référence à la main (contrôleur XR) qui tient le marteau

    /// <summary>
    /// À l'initialisation récupère automatiquement le XRGrabInteractable si non assigné
    /// </summary>
    private void Awake()
    {
        // Si non assigné, récupère automatiquement le composant sur l'objet
        if (grabInteractable == null)
            grabInteractable = GetComponent<XRGrabInteractable>();
    }
    /// <summary>
    /// S'abonne aux événements lorsque le script est activé
    /// </summary>
    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(QuandAttrape);
        grabInteractable.selectExited.AddListener(QuandRelache);
    }

    /// <summary>
    /// Se désabonne des événements pour éviter les erreurs/mémoires
    /// </summary>
    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(QuandAttrape);
        grabInteractable.selectExited.RemoveListener(QuandRelache);
    }
    /// <summary>
    /// Appelé quand le joueur attrape le marteau
    /// </summary>
    /// <param name="args">Infos sur l'interaction XR</param>
    private void QuandAttrape(SelectEnterEventArgs args)
    {
        // Récupère le contrôleur qui a attrapé l'objet
        interactorActuel = args.interactorObject as XRBaseInputInteractor;
        // Envoie un petit feedback haptique
        EnvoyerHaptique(amplitudeGrab, dureeGrab);
    }
    /// <summary>
    /// Appelé quand le joueur lâche le marteau
    /// </summary>
    private void QuandRelache(SelectExitEventArgs args)
    {
        interactorActuel = null;
    }
    /// <summary>
    /// Méthode  appelée lors d'un impact (ex: frapper une taupe)
    /// </summary>
    public void JouerHaptiqueImpact()
    {
        EnvoyerHaptique(amplitudeImpact, dureeImpact);
    }

    /// <summary>
    /// Envoie une vibration au contrôleur XR actif
    /// </summary>
    /// <param name="amplitude">Intensité de la vibration (0 à 1)</param>
    /// <param name="duree">Durée en secondes</param>
    private void EnvoyerHaptique(float amplitude, float duree)
    {
        // Vérifie qu'on a bien un contrôleur valide. Si il y a une interaction en cours et si l'interaction a bien un controleur Xr associé a la manette
        if (interactorActuel != null && interactorActuel.xrController != null)
        {
            //// Envoie une vibration à la manette avec l’intensité et la durée données
            interactorActuel.xrController.SendHapticImpulse(amplitude, duree);
        }
    }
}
