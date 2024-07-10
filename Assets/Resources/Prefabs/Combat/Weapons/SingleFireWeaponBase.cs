using UnityEngine;
using System.Collections;

public abstract class SingleFireWeaponBase : WeaponBase
{
    private Coroutine fireCoroutine;

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
            StartAnimation();
            Fire(isEnemy, bulletSpeedModifier, damageModifier);
            yield return new WaitForSeconds(1f / DetermineCurrentFireRate());
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

    protected void Fire(bool isEnemy, float bulletSpeedModifier, float damageModifier)
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
