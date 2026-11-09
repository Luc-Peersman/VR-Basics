using UnityEngine;

/// <summary>
/// Zet dit object terug op zijn beginplek als het van de vloer valt
/// (bijvoorbeeld een bal die je te ver weggooit).
/// </summary>
[RequireComponent(typeof(Rigidbody))]
public class TerugNaarStart : MonoBehaviour
{
    [Tooltip("Valt het object lager dan deze hoogte (in meters), dan gaat het terug naar de start.")]
    public float minimaleHoogte = -2f;

    private Vector3 startPositie;
    private Quaternion startRotatie;
    private Rigidbody rb;

    void Start()
    {
        // Onthoud waar het object aan het begin ligt
        startPositie = transform.position;
        startRotatie = transform.rotation;
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        if (transform.position.y < minimaleHoogte)
        {
            TerugZetten();
        }
    }

    /// <summary>
    /// Zet het object terug. Je kunt dit ook vanuit een knop (UnityEvent) aanroepen.
    /// </summary>
    public void TerugZetten()
    {
        // Eerst de snelheid op nul, anders vliegt het object meteen weer weg
        rb.linearVelocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.SetPositionAndRotation(startPositie, startRotatie);
    }
}
