using UnityEngine;
using System.Collections;

public abstract class SingleFireWeaponBase : WeaponBase
{
    private Coroutine fireCoroutine;
    private float NextAllowedFireTime;


    public override void AttemptFire(bool isEnemy, float damageModifier, float bulletSpeedModifier)
    {
        fireCoroutine ??= StartCoroutine(FireCoroutine(isEnemy, damageModifier, bulletSpeedModifier));
    }

    public override void AttemptCeaseFire()
    {
        if (fireCoroutine != null)
        {
            StopCoroutine(fireCoroutine);
            fireCoroutine = null;
        }
    }

    private IEnumerator FireCoroutine(bool isEnemy, float damageModifier, float bulletSpeedModifier)
    {
        while (true)
        {
            // This is to prevent CeaseFire -> AttemptFire spam to bypass FireRate
            if (NextAllowedFireTime > Time.time)
            {
                yield return new WaitForSeconds(NextAllowedFireTime - Time.time);
            }
            
            StartAnimation();
            Fire(isEnemy, damageModifier, bulletSpeedModifier);
            float delay = 1f / DetermineCurrentFireRate();
            NextAllowedFireTime = Time.time + delay;
            yield return new WaitForSeconds(delay);
        }
    }

    private float DetermineCurrentFireRate()
    {
        // Fetch the fireRateModifier from the parent ShipBase
        ShipBase parentShip = GetComponentInParent<ShipBase>();
        if (parentShip != null)
        {
            return BaseFireRate * parentShip.FireRateModifier;
        }
        else
        {
            return BaseFireRate;
        }
    }

    protected void Fire(bool isEnemy, float damageModifier, float bulletSpeedModifier)
    {
        if (ProjectilePrefab != null && FirePoint != null)
        {
            GameObject projectile = Instantiate(ProjectilePrefab, FirePoint.position, FirePoint.rotation);

            var projectileScript = projectile.GetComponent<ProjectileBase>();
            if (projectileScript != null)
            {
                // Use ship's current velocity as the initial velocity of the projectile
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
