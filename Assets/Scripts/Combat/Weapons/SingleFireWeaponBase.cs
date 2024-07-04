using UnityEngine;
using System.Collections;

public abstract class SingleFireWeaponBase : WeaponBase
{
    private Coroutine fireCoroutine;

    public override void AttemptFire(bool isEnemy, float fireRateModifier, float damageModifier, float bulletSpeedModifier)
    {
        // ! This wont update if a modifyer is picked up during the Coroutine.
        // ! Update to check the modifier of the ship that called it.
        CurrentFireRate = BaseFireRate * fireRateModifier;

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
            yield return new WaitForSeconds(1f / CurrentFireRate);
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
