using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ElectroShieldEffect : MonoBehaviour
{
    public float MaxPitch = 3.0f; // Maximum pitch value to prevent endless growth
    private float charge = 0.1f; // Increment value for pitch
    private AudioSource audioSource;
    public bool IsEnemyShield;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on the GameObject.");
        }

        Collider2D collider = GetComponent<Collider2D>();
        if (collider != null)
        {
            collider.isTrigger = true; // Ensure the collider is set to trigger
        }
        else
        {
            Debug.LogError("Collider2D component not found on the GameObject.");
        }
    }

    public void AbsorbHit(float damage) {
        if (audioSource != null)
        {
            audioSource.pitch = Mathf.Min(audioSource.pitch + charge, MaxPitch);
            Debug.Log("Collision detected. New pitch: " + audioSource.pitch);
        }
    }

    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     if (audioSource != null)
    //     {
    //         audioSource.pitch = Mathf.Min(audioSource.pitch + charge, MaxPitch);
    //         Debug.Log("Collision detected. New pitch: " + audioSource.pitch);
    //     }
    // }
}
