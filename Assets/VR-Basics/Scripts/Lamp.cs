using UnityEngine;

/// <summary>
/// Een lampje dat je aan en uit kunt zetten, bijvoorbeeld met een knop.
/// Koppel in de Inspector het event van de knop aan Lamp.Wissel (of Aan / Uit).
/// </summary>
public class Lamp : MonoBehaviour
{
    [Tooltip("Het licht dat aan of uit gaat.")]
    public Light lichtbron;

    [Tooltip("Het zichtbare deel van de lamp (het bolletje).")]
    public Renderer lampKap;

    [Tooltip("Materiaal als de lamp aan is (licht op).")]
    public Material materiaalAan;

    [Tooltip("Materiaal als de lamp uit is.")]
    public Material materiaalUit;

    [Tooltip("Staat de lamp aan bij de start?")]
    public bool aanBijStart = false;

    private bool isAan;

    void Start()
    {
        if (lichtbron == null)
        {
            Debug.LogWarning($"Lamp '{name}': er is geen Light ingevuld bij 'Lichtbron'.", this);
        }
        Zet(aanBijStart);
    }

    /// <summary>Aan wordt uit, uit wordt aan.</summary>
    public void Wissel()
    {
        Zet(!isAan);
    }

    public void Aan()
    {
        Zet(true);
    }

    public void Uit()
    {
        Zet(false);
    }

    private void Zet(bool aan)
    {
        isAan = aan;

        if (lichtbron != null)
        {
            lichtbron.enabled = aan;
        }

        if (lampKap != null)
        {
            Material nieuw = aan ? materiaalAan : materiaalUit;
            if (nieuw != null)
            {
                lampKap.sharedMaterial = nieuw;
            }
        }
    }
}
