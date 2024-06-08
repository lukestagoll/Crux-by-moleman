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
    protected float hitpoints;
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
    public abstract void FireWeapons();

    public void Explode()
    {
        // Instantiate the explosion prefab at the projectile's position
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public void AttachWeapon(GameObject weaponPrefab, int quantity)
    {
        int slotsFilled = 0;

        foreach (var slot in WeaponSlots)
        {
            if (slotsFilled >= quantity)
                break;

            // Check if the slot is empty
            if (slot.childCount == 0)
            {
                GameObject weaponObject = Instantiate(weaponPrefab, slot.position, slot.rotation, slot);
                WeaponBase weapon = weaponObject.GetComponent<WeaponBase>();
                if (weapon != null)
                {
                    AttachedWeapons.Add(weapon);
                    slotsFilled++;
                }
                else
                {
                    Debug.LogError("The weaponPrefab does not have a WeaponBase component.");
                    Destroy(weaponObject);
                }
            }
        }

        if (slotsFilled < quantity)
        {
            Debug.LogWarning("Not enough empty weapon slots to attach all weapons.");
        }
    }
}
