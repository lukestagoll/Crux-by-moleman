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
    public string PrefabName;
    [HideInInspector] public bool IsEmpty = true;

    public SlotType Type;

    public List<AttachPoint> AttachPoints;
}

public abstract class ShipBase : MonoBehaviour
{
    public List<WeaponSlot> WeaponSlots;
    [SerializeField] protected GameObject ExplosionPrefab;
    public float FireRateModifier = 1f; // These should be visible in the Inspector
    public float DamageModifier = 1f;   // These should be visible in the Inspector
    [SerializeField] protected float BulletSpeedModifier = 1f;   // These should be visible in the Inspector
    [SerializeField] protected float MovementSpeedModifier = 1f;   // These should be visible in the Inspector
    [SerializeField] protected float Hitpoints;

    protected bool isEnemy;
    protected bool isDestroyed;
    private List<AttachPoint> ActiveAttachPoints = new List<AttachPoint>();
    public bool IsAllowedToShoot { get; set; }

    // Weapon States
    private bool PrimaryFireEnabled = false;
    private bool SpecialFireEnabled = false;
    private bool SpecialFireCeasing = false;

    void Awake()
    {
        InitialiseWeaponSlots();
        PrimaryFireEnabled = false;
        SpecialFireEnabled = false;
        SpecialFireCeasing = false;
        IsAllowedToShoot = true;
    }

    public void EnablePrimaryFire()
    {
        if (SpecialFireEnabled) return;
        if (!HasActiveAttachPoint(WeaponType.Primary)) return;
        FireWeapons(WeaponType.Primary);
        PrimaryFireEnabled = true;
    }
    public void DisablePrimaryFire()
    {
        if (!PrimaryFireEnabled) return;
        CeaseFire(WeaponType.Primary);
        PrimaryFireEnabled = false;
    }
    public void EnableSpecialFire()
    {
        if (SpecialFireEnabled) return;
        if (!HasActiveAttachPoint(WeaponType.Special)) return;
        DisablePrimaryFire();
        FireWeapons(WeaponType.Special);
        SpecialFireEnabled = true;
    }
    public void DisableSpecialFire()
    {
        if (!SpecialFireEnabled || SpecialFireCeasing) return;
        if (!HasActiveAttachPoint(WeaponType.Special))
        {
            HandleSpecialFireCeased();
            return;
        };
        SpecialFireCeasing = true;
        CeaseFire(WeaponType.Special);
        // SpecialFireEnabled is set to false by the cease fire method
    }
    public void HandleSpecialFireCeased()
    {
        SpecialFireCeasing = false;
        SpecialFireEnabled = false;
    }

    public void InitialiseWeaponSlots()
    {
            foreach (WeaponSlot weaponSlot in WeaponSlots)
            {
                bool hasActiveAttachPoint = false;
                foreach (AttachPoint attachPoint in weaponSlot.AttachPoints)
                {
                    attachPoint.InitialiseAttachPoint();
                    if (!attachPoint.IsEmpty)
                    {
                        ActiveAttachPoints.Add(attachPoint);
                        hasActiveAttachPoint = true;
                    }
                }
                weaponSlot.IsEmpty = !hasActiveAttachPoint;
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

    public WeaponSlot GetWeaponSlot(SlotType type)
    {
        foreach(WeaponSlot weaponSlot in WeaponSlots)
        {
            if (weaponSlot.Type == type) return weaponSlot;
        }
        return null;
    }

    public bool HasActiveAttachPoint(WeaponType weaponType)
    {
        foreach (AttachPoint attachPoint in ActiveAttachPoints)
            {
                WeaponBase attachedWeapon = attachPoint.AttachedWeapon.GetComponent<WeaponBase>();
                if (attachedWeapon.WeaponType == weaponType) return true;
            }
        return false;
    }

    public virtual void ToggleShooting()
    {
        IsAllowedToShoot = !IsAllowedToShoot;
    }

    public virtual void DisableShooting()
    {
        IsAllowedToShoot = false;
    }

    public virtual void EnableShooting()
    {
        IsAllowedToShoot = false;
    }

    public void FireWeapons(WeaponType weaponType)
    {
        if (IsAllowedToShoot)
        {
            foreach (AttachPoint attachPoint in ActiveAttachPoints)
            {
                WeaponBase attachedWeapon = attachPoint.AttachedWeapon.GetComponent<WeaponBase>();
                if (attachedWeapon.WeaponType != weaponType) continue;
                attachedWeapon.AttemptFire(isEnemy, FireRateModifier, DamageModifier, BulletSpeedModifier);
            }
        }
    }

    public void CeaseFire(WeaponType weaponType)
    {
        foreach (AttachPoint attachPoint in ActiveAttachPoints)
        {
            WeaponBase attachedWeapon = attachPoint.AttachedWeapon.GetComponent<WeaponBase>();
            if (attachedWeapon.WeaponType != weaponType) continue;
            attachedWeapon.AttemptCeaseFire();
        }
    }

    public void Explode()
    {
        // Instantiate the explosion prefab at the projectile's position
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public WeaponSlot AttemptWeaponAttachment(GameObject weaponPrefab, bool force)
    {
        // Is weapon SINGLE, DUAL, OR SYSTEM?
        // Attempt to fetch an empty slot of that type
        WeaponBase weaponPrefabComponent = weaponPrefab.GetComponent<WeaponBase>();
        Debug.Log("FETCHING SLOTTYPE:" + weaponPrefabComponent.SlotType);
        WeaponSlot emptySlot = GetEmptyWeaponSlot(weaponPrefabComponent.SlotType);
        if (emptySlot != null)
        {
            AttachWeaponsToSlot(weaponPrefab, emptySlot);
            return emptySlot;
        }
        else if (emptySlot == null && force)
        {
            WeaponSlot weaponSlot = GetWeaponSlot(weaponPrefabComponent.SlotType);
            if (weaponSlot == null)
            {
                Debug.LogWarning("No weapon slot of type " + weaponPrefabComponent.SlotType + " found!");
                return null;
            }
            AttachWeaponsToSlot(weaponPrefab, weaponSlot);
            return weaponSlot;
        }
        else
        {
            Debug.Log("All weapon slots are filled!");
            return null;
        }
    }

    public void DetachWeaponsFromSlot(WeaponSlot weaponSlot)
    {
        foreach(AttachPoint attachPoint in weaponSlot.AttachPoints)
        {
            if (!attachPoint.IsEmpty)
            {
                attachPoint.DetachWeapon();
                ActiveAttachPoints.Remove(attachPoint);
            }
        }
        weaponSlot.IsEmpty = true;
    }

    private void AttachWeaponsToSlot(GameObject weaponPrefab, WeaponSlot weaponSlot)
    {
        foreach(AttachPoint attachPoint in weaponSlot.AttachPoints)
        {
                attachPoint.AttachWeapon(weaponPrefab, true);
                ActiveAttachPoints.Add(attachPoint);
        }
        weaponSlot.IsEmpty = false;
    }

    public abstract void Die();
    public abstract void TakeDamage(float damage);
    public abstract void AddHitpoints(float amt);
}
