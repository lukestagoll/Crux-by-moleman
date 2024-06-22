using UnityEngine;

public abstract class SingleFireWeaponBase : WeaponBase
{
    public override void AttemptFire(bool isEnemy, float fireRateModifier, float damageModifier, float bulletSpeedModifier)
    {
        CurrentFireRate = BaseFireRate * fireRateModifier;

        if (fireRateTimer >= 1f / CurrentFireRate)
        {
            StartAnimation();
            Fire(isEnemy, bulletSpeedModifier, damageModifier);
            fireRateTimer = 0f; // Reset timer after shooting
        }
    }

    // Unecessary for this weapon type
    public override void AttemptCeaseFire() {}

    protected void Fire(bool isEnemy, float bulletSpeedModifier, float damageModifier)
    {
        if (ProjectilePrefab != null && FirePoint != null)
        {
            GameObject projectile = Instantiate(ProjectilePrefab, FirePoint.position, FirePoint.rotation);

            var projectileScript = projectile.GetComponent<ProjectileBase>();
            if (projectileScript != null)
            {
                // Use ships current velocity as the initial velocity of projectile
                Vector2 initialVelocity = rb != null ? rb.velocity : Vector2.zero;
                projectileScript.Initialize(isEnemy, bulletSpeedModifier, damageModifier, initialVelocity, Side);
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
}
