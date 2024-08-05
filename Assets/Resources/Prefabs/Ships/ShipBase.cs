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
    [SerializeField] public float MaxHealth;
    [SerializeField] public float Health;
    [SerializeField] public float MaxShield;
    [SerializeField] public float Shield;
    protected float MaxCharge = 100f;
    protected float Charge;

    // SKILL STATES
    public bool AdvancedTargetting;
    public bool DefensiveFormations;

    // MODIFIERS
    public float FireRateModifier = 1f;
    public float DamageModifier = 1f;
    public int PiercingModifier = 0;
    [SerializeField] public float BulletSpeedModifier = 1f;
    [SerializeField] public float MovementSpeedModifier = 1f;
    public float ChargeRateModifier = 1f;
    public float EvasionChanceModifier;
    public float CriticalHitChanceModifier;
    public float DroneFireRateModifier = 1f;
    public float DroneChargeRateModifier = 1f;

    protected bool ShieldIsActive;
    public Material DefaultMaterial;
    public Material ShieldGlowMaterial;

    protected bool IsEnemy;
    protected bool isDestroyed;
    private List<AttachPoint> ActiveAttachPoints = new List<AttachPoint>();
    public bool IsAllowedToShoot { get; set; }

    // Weapon States
    private bool PrimaryFireEnabled = false;
    private bool SpecialFireEnabled = false;
    private bool SpecialFireCeasing = false;

    // Events
    public event Action OnSpawn;
    public event Action OnHit;
    public event Action OnUpdate;
    public event Action OnDeath;
    // public event Action OnDestroy;

    protected GameObject DroneAnchor;

    protected virtual void EmitOnSpawn()
    {
        OnSpawn?.Invoke();
    }

    void Awake()
    {
        InitialiseWeaponSlots();
        PrimaryFireEnabled = false;
        SpecialFireEnabled = false;
        SpecialFireCeasing = false;
        IsAllowedToShoot = true;
    }

    void Update()
    {
        OnUpdate?.Invoke();
        UpdateDroneAnchor();
    }

    void UpdateDroneAnchor()
    {
        if (DroneAnchor == null) return;
        Vector3 targetPosition = transform.position;
        DroneAnchor.transform.position = Vector3.Lerp(DroneAnchor.transform.position, targetPosition, Time.deltaTime * 5f);
    }

    public DroneShip SpawnDrone(bool shieldDrone = false)
    {
        if (DroneAnchor == null)
        {
            DroneAnchor = new GameObject("DroneAnchor");
            DroneAnchor.transform.position = transform.position;
        }

        DroneShip droneShip = Instantiate(shieldDrone ? AssetManager.ShieldDronePrefab : AssetManager.AttackDronePrefab, transform.position, transform.rotation);
        droneShip.ParentShip = this;
        droneShip.Charge = 50f;
        droneShip.ParentDroneAnchor = DroneAnchor;
        droneShip.FireRateModifier = DroneFireRateModifier;
        droneShip.ChargeRateModifier = DroneChargeRateModifier;
        droneShip.AdvancedTargetting = AdvancedTargetting;
        droneShip.DefensiveFormations = DefensiveFormations;
        return droneShip;
    }

    public abstract void Die();
    public void TakeDamage(float damage, float critChance)
    {
        if (isDestroyed) return;
        // Check for evasion
        if (UnityEngine.Random.value > EvasionChanceModifier)
        {
            // Check for crit
            if (UnityEngine.Random.value < critChance) {
                damage *= 2;
            }
            if (!ShieldIsActive)
            {
                SubtractHealth(damage);
            }
            else
            {
                float excessDamage = SubtractShield(damage);
                if (excessDamage > 0) SubtractHealth(excessDamage);
            }
            OnHit?.Invoke();
        }
        else
        {
            // Damage Evaded Logic
        }
    }
    public abstract void AddShield(float amt);
    protected abstract float SubtractShield(float amt);
    public abstract void AddHealth(float amt);
    protected abstract void SubtractHealth(float amt);

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
        foreach (WeaponSlot weaponSlot in WeaponSlots)
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
        foreach (WeaponSlot weaponSlot in WeaponSlots)
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

    private void FireWeapons(WeaponType weaponType)
    {
        if (IsAllowedToShoot)
        {
            foreach (AttachPoint attachPoint in ActiveAttachPoints)
            {
                WeaponBase attachedWeapon = attachPoint.AttachedWeapon.GetComponent<WeaponBase>();
                if (attachedWeapon.WeaponType != weaponType) continue;
                attachedWeapon.AttemptFire(IsEnemy);
            }
        }
    }

    private void CeaseFire(WeaponType weaponType)
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
        OnDeath?.Invoke();
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

    public WeaponSlot AttemptWeaponAttachment(GameObject weaponPrefab, bool force)
    {
        // Is weapon SINGLE, DUAL, OR SYSTEM?
        // Attempt to fetch an empty slot of that type
        WeaponBase weaponPrefabComponent = weaponPrefab.GetComponent<WeaponBase>();
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
        foreach (AttachPoint attachPoint in weaponSlot.AttachPoints)
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
        foreach (AttachPoint attachPoint in weaponSlot.AttachPoints)
        {
            attachPoint.AttachWeapon(weaponPrefab, true);
            ActiveAttachPoints.Add(attachPoint);
        }
        weaponSlot.IsEmpty = false;
        // play audio here
    }
}
