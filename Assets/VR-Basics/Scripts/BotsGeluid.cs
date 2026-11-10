using UnityEngine;

/// <summary>
/// Speelt een geluid als dit object ergens hard genoeg tegenaan botst.
/// Bijvoorbeeld een blik dat omvalt, of een bal die de grond raakt.
/// Zet dit script op een object met een Collider en een Rigidbody.
/// Sleep je geluid in het vak 'Geluid'.
/// </summary>
[RequireComponent(typeof(AudioSource))]
public class BotsGeluid : MonoBehaviour
{
    [Tooltip("Het geluid dat je hoort bij een botsing.")]
    public AudioClip geluid;

    [Tooltip("Hoe hard moet de botsing zijn? Hoger = alleen harde botsingen.")]
    public float minimaleSnelheid = 0.5f;

    [Tooltip("Hoeveel seconden er minstens tussen twee geluiden zit.")]
    public float wachttijd = 0.15f;

    private AudioSource bron;
    private float laatsteKeer = -1f;

    void Awake()
    {
        bron = GetComponent<AudioSource>();
        bron.playOnAwake = false;
        bron.spatialBlend = 1f; // 3D: je hoort waar het geluid vandaan komt

        if (geluid == null)
        {
            Debug.LogWarning($"BotsGeluid '{name}': er is geen geluid ingevuld bij 'Geluid'.", this);
        }
    }

    void OnCollisionEnter(Collision botsing)
    {
        if (geluid == null)
        {
            return;
        }

        // Te zacht? Dan geen geluid. Zo hoor je niets als de blikken bij de start op elkaar landen.
        float snelheid = botsing.relativeVelocity.magnitude;
        if (snelheid < minimaleSnelheid)
        {
            return;
        }

        // Niet te vaak achter elkaar.
        if (Time.time - laatsteKeer < wachttijd)
        {
            return;
        }
        laatsteKeer = Time.time;

        // Hoe harder de botsing, hoe harder het geluid (maximaal 1).
        float volume = Mathf.Clamp01(snelheid / 5f);
        bron.PlayOneShot(geluid, volume);
    }
}
