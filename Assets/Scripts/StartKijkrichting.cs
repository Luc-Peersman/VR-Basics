using System.Collections;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;

/// <summary>
/// Zorgt dat de speler bij de start op de startplek staat en de goede kant op kijkt.
/// Zonder dit script bepaalt de bril (de Guardian) waar "vooruit" is, en kan de scene
/// achter je staan. Na herijken met de Meta-knop lijnt het script opnieuw uit.
/// Zet dit script op de XR Origin.
/// </summary>
[RequireComponent(typeof(XROrigin))]
public class StartKijkrichting : MonoBehaviour
{
    [Tooltip("Waar het hoofd van de speler bij de start staat (de hoogte komt van de bril zelf).")]
    public Vector3 startPlek = Vector3.zero;

    [Tooltip("Welke kant de speler bij de start op kijkt, in graden (0 = langs de blauwe Z-pijl).")]
    public float startRichting = 0f;

    private XROrigin xrOrigin;
    private readonly List<XRInputSubsystem> subsystemen = new List<XRInputSubsystem>();

    IEnumerator Start()
    {
        xrOrigin = GetComponent<XROrigin>();

        // Luister naar herijken (lang op de Meta-knop drukken)
        SubsystemManager.GetSubsystems(subsystemen);
        foreach (var s in subsystemen)
        {
            s.trackingOriginUpdated += OpnieuwUitlijnen;
        }

        // Wacht tot de bril een positie doorgeeft (maximaal 3 seconden)
        float wachttijd = 0f;
        while (xrOrigin.Camera.transform.localPosition == Vector3.zero && wachttijd < 3f)
        {
            wachttijd += Time.deltaTime;
            yield return null;
        }
        yield return null;
        Uitlijnen();
    }

    void OnDestroy()
    {
        foreach (var s in subsystemen)
        {
            s.trackingOriginUpdated -= OpnieuwUitlijnen;
        }
    }

    private void OpnieuwUitlijnen(XRInputSubsystem subsystem)
    {
        StartCoroutine(UitlijnenNaEenFrame());
    }

    private IEnumerator UitlijnenNaEenFrame()
    {
        yield return null;
        Uitlijnen();
    }

    /// <summary>Draai en verplaats de XR Origin zodat het hoofd op de startplek staat en de goede kant op kijkt.</summary>
    public void Uitlijnen()
    {
        Vector3 kijkrichting = Quaternion.Euler(0f, startRichting, 0f) * Vector3.forward;
        xrOrigin.MatchOriginUpCameraForward(Vector3.up, kijkrichting);

        float hoofdHoogte = xrOrigin.Camera.transform.position.y;
        xrOrigin.MoveCameraToWorldLocation(new Vector3(startPlek.x, hoofdHoogte, startPlek.z));
    }
}
