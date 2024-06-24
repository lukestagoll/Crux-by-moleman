using UnityEngine;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ElectroShieldEffect : MonoBehaviour
{
    public float MaxPitch = 3.0f; // Maximum pitch value to prevent endless growth
    public float CurrentCharge = 150;
    private float MaxCharge = 500;
    private AudioSource audioSource;
    public bool IsEnemyShield;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on the GameObject.");
        }
    }

    public void AbsorbHit(float damage) {
        CurrentCharge += damage;
        Debug.Log("CHARGE: " + CurrentCharge);
        if (CurrentCharge > MaxCharge)
        {
            CurrentCharge = MaxCharge;
        }
        if (audioSource != null)
        {
            audioSource.pitch = 3 * (CurrentCharge / MaxCharge);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        string tag = other.gameObject.tag;
        if (tag == "Enemy" && !IsEnemyShield)
        {
            EnemyShip enemyShip = other.GetComponent<EnemyShip>();
            if (enemyShip != null)
            {
                AbsorbHit(enemyShip.damageOnCollision);
                enemyShip.Die();
            }
        }
    }
}
