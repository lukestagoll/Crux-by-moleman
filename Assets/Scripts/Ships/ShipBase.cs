using System.Collections.Generic;
using UnityEngine;

public abstract class BaseShip : MonoBehaviour
{
    protected bool isDestroyed;
    [SerializeField] protected List<AttachPoint> AttachPoints;
    [SerializeField] protected GameObject ExplosionPrefab;
    [SerializeField] protected float FireRateModifier = 1f; // These should be visible in the Inspector
    [SerializeField] protected float DamageModifier = 1f;   // These should be visible in the Inspector
    [SerializeField] protected float BulletSpeedModifier = 1f;   // These should be visible in the Inspector
    [SerializeField] protected float MovementSpeedModifier = 1f;   // These should be visible in the Inspector
    [SerializeField] protected float hitpoints;
    protected bool isEnemy;
    public bool IsAllowedToShoot { get; set; }

    protected virtual void Start()
    {
        IsAllowedToShoot = true;
    }

    public virtual void ToggleShooting()
    {
        IsAllowedToShoot = !IsAllowedToShoot;
    }

    public void FireWeapons()
    {
        if (IsAllowedToShoot)
        {
            foreach (AttachPoint attachPoint in AttachPoints)
            {
                attachPoint.FireWeapon(isEnemy, FireRateModifier, DamageModifier, BulletSpeedModifier);
            }
        }
    }

    public abstract void Die();
    public abstract void TakeDamage(float damage);
    public abstract void AddHitpoints(float amt);

    public void Explode()
    {
        // Instantiate the explosion prefab at the projectile's position
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void AttachWeapon(WeaponBase weaponPrefabComponent)
    {
        foreach(AttachPoint attachPoint in AttachPoints)
        {
            if (!attachPoint.isSlotFilled)
            {
                attachPoint.AttachWeapon(weaponPrefabComponent, false);
                return;
            }
        }
        Debug.Log("All weapon slots are filled!");
    }
}
