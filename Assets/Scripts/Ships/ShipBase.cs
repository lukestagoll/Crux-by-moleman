using System.Collections.Generic;
using UnityEngine;

public abstract class BaseShip : MonoBehaviour
{
    protected bool isDestroyed;
    protected List<WeaponBase> AttachedWeapons;
    [SerializeField] protected GameObject ExplosionPrefab;
    [SerializeField] protected float FireRateModifier = 1f; // These should be visible in the Inspector
    [SerializeField] protected float DamageModifier = 1f;   // These should be visible in the Inspector
    [SerializeField] protected float BulletSpeedModifier = 1f;   // These should be visible in the Inspector
    [SerializeField] protected float MovementSpeedModifier = 1f;   // These should be visible in the Inspector
    [SerializeField] protected float hitpoints;
    public bool IsAllowedToShoot { get; set; }

    // List of weapon slots
    [SerializeField] private List<Transform> WeaponSlots;

    protected virtual void Start()
    {
        // Initialize the AttachedWeapons list
        AttachedWeapons = new List<WeaponBase>();

        // Find all WeaponBase components in children and add them to the list
        WeaponBase[] weapons = GetComponentsInChildren<WeaponBase>();
        foreach (var weapon in weapons)
        {
            AttachedWeapons.Add(weapon);
        }
        IsAllowedToShoot = true;
    }

    public virtual void ToggleShooting()
    {
        IsAllowedToShoot = !IsAllowedToShoot;
    }

    public abstract void Die();
    public abstract void TakeDamage(float damage);
    public abstract void AddHitpoints(float amt);
    public abstract void FireWeapons();

    public void Explode()
    {
        // Instantiate the explosion prefab at the projectile's position
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
