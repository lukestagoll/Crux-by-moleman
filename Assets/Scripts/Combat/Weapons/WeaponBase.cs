using UnityEngine;

public abstract class WeaponBase : MonoBehaviour
{
    // Public variables to be set from the inspector in derived classes
    public GameObject ProjectilePrefab;
    public Transform FirePoint;

    // Protected variables
    protected bool hasAnimation;
    [SerializeField]
    protected float BaseFireRate;
    protected float CurrentFireRate;
    protected float fireRateTimer;

    // Reference to the Rigidbody2D component of the weapon
    private Rigidbody2D rb;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        if (rb == null)
        {
            Debug.LogError("WeaponBase: No Rigidbody2D component found on the weapon.");
        }
    }

    // Abstract methods to be implemented by derived classes
    public virtual void Shoot(bool isEnemy, float fireRateModifier, float damageModifier, float bulletSpeedModifier)
    {
        CurrentFireRate = BaseFireRate * fireRateModifier;

        if (fireRateTimer >= 1f / CurrentFireRate)
        {
            StartAnimation();
            FireProjectile(isEnemy, bulletSpeedModifier, damageModifier);
            fireRateTimer = 0f; // Reset timer after shooting
        }
    }

    protected virtual void StartAnimation()
    {
        if (hasAnimation)
        {
            // Animation logic here
        }
    }

    protected virtual void FireProjectile(bool isEnemy, float bulletSpeedModifier, float damageModifier)
    {
        if (ProjectilePrefab != null && FirePoint != null)
        {
            GameObject projectile = Instantiate(ProjectilePrefab, FirePoint.position, FirePoint.rotation);

            var projectileScript = projectile.GetComponent<ProjectileBase>();
            if (projectileScript != null)
            {
                Vector2 initialVelocity = rb != null ? rb.velocity : Vector2.zero;
                projectileScript.Initialize(isEnemy, bulletSpeedModifier, damageModifier, initialVelocity);
            }
            else
            {
                Debug.LogError("Projectile does not have a ProjectileBase component.");
            }
        }
        else
        {
            if (ProjectilePrefab == null)
            {
                Debug.LogError("ProjectilePrefab is not set in the inspector.");
            }
            if (FirePoint == null)
            {
                Debug.LogError("FirePoint is not set in the inspector.");
            }
        }
    }

    protected virtual void Update()
    {
        // Update the shooting timer
        fireRateTimer += Time.deltaTime;
    }
}
