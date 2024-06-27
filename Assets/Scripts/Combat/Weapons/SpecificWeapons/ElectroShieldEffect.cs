using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(Collider2D))]
public class ElectroShieldEffect : MonoBehaviour
{
    public float MaxPitch = 3.0f; // Maximum pitch value to prevent endless growth
    public float CurrentCharge;
    private float MaxCharge = 500;
    private float Health;
    private AudioSource audioSource;
    public bool IsEnemyShield;
    private ToggleFireWeaponBase ElectroShieldComponent; 

    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            Debug.LogError("AudioSource component not found on the GameObject.");
        }
    }

    public void Initialise(bool isEnemy, float charge, float initialHealth, ToggleFireWeaponBase electroShieldComponent)
    {
        IsEnemyShield = isEnemy;
        CurrentCharge = charge;
        Health = initialHealth;
        ElectroShieldComponent = electroShieldComponent;
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
            audioSource.pitch = 3 * (CurrentCharge / MaxCharge) + 1;
        }
    }

    public void TakeDamage(float damage)
    {
        Health -= damage;
        Debug.Log("Shield taken damage" + damage + "Health:" + Health);
        if (Health <= 0)
        {
            Debug.Log("Shield broken!");
            // Do shield break stuff
            // Play break sound
            ElectroShieldComponent.AttemptCeaseFire();
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
                TakeDamage(enemyShip.damageOnCollision);
                enemyShip.Die();
            }
        }
    }
}
