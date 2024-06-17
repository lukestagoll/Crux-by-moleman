using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public enum SlotType
    {
        Single,
        Dual,
        System
    }

[Serializable]
public class WeaponSlot
{
    [HideInInspector]
    public bool IsEmpty = true;

    public SlotType Type;

    public List<AttachPoint> AttachPoints;
}

public abstract class BaseShip : MonoBehaviour
{
    protected bool isDestroyed;
    [SerializeField] protected List<WeaponSlot> WeaponSlots;
    protected List<AttachPoint> ActiveAttachPoints;
    [SerializeField] protected GameObject ExplosionPrefab;
    [SerializeField] protected float FireRateModifier = 1f; // These should be visible in the Inspector
    [SerializeField] protected float DamageModifier = 1f;   // These should be visible in the Inspector
    [SerializeField] protected float BulletSpeedModifier = 1f;   // These should be visible in the Inspector
    [SerializeField] protected float MovementSpeedModifier = 1f;   // These should be visible in the Inspector
    [SerializeField] protected float Hitpoints;
    protected bool isEnemy;
    public bool IsAllowedToShoot { get; set; }

    protected virtual void Start()
    {
        // This is the initial check when starting.
        // It is used to ensure the variables are set to the right things
        // WeaponSlots will have a list but a bool that needs to be set
        InitialiseWeaponSlots();

        IsAllowedToShoot = true;
    }

    public void InitialiseWeaponSlots()
    {
        foreach (WeaponSlot weaponSlot in WeaponSlots)
        {
            if (weaponSlot.AttachPoints.Count > 0)
            {
                weaponSlot.IsEmpty = false;

                ActiveAttachPoints.AddRange(weaponSlot.AttachPoints);
            }
        }
    }

    public WeaponSlot GetEmptyWeaponSlot(SlotType type)
    {
        foreach(WeaponSlot weaponSlot in WeaponSlots)
        {
            if (weaponSlot.Type == type && weaponSlot.IsEmpty)
            {
                return weaponSlot;
            }
        }
        return null;
    }

    public virtual void ToggleShooting()
    {
        IsAllowedToShoot = !IsAllowedToShoot;
    }

    public void FireWeapons()
    {
        if (IsAllowedToShoot)
        {
            foreach (AttachPoint attachPoint in ActiveAttachPoints)
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

    public void AttemptWeaponAttachment(WeaponBase weaponPrefabComponent, bool force)
    {
        // Is weapon SINGLE, DUAL, OR SYSTEM?
        // Fetch all attachPoints with that side that are free
        foreach(WeaponSlot weaponSlot in WeaponSlots)
        {
            if (weaponSlot.IsEmpty && weaponSlot.Type == weaponPrefabComponent.SlotType )
            {
                WeaponSlot emptySlot = GetEmptyWeaponSlot(weaponSlot.Type);
                // attachPoint.AttachWeapon(weaponPrefabComponent, force);
                return;
            }
        }
        Debug.Log("All weapon slots are filled!");
    }
}
