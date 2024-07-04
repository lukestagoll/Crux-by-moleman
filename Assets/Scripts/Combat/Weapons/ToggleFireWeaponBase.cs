using System;

public abstract class ToggleFireWeaponBase : WeaponBase
{
    public override void AttemptFire(bool isEnemy, float fireRateModifier, float damageModifier, float bulletSpeedModifier)
    {
        Fire(isEnemy, bulletSpeedModifier, damageModifier);
    }

    public override void AttemptCeaseFire()
    {
        ShipBase ship = GetComponentInParent<ShipBase>();
        CeaseFire(OnCeaseFireCompleted);
    }

    protected abstract void Fire(bool isEnemy, float bulletSpeedModifier, float damageModifier);
    protected abstract void CeaseFire(Action onCompleted);
    protected virtual void OnCeaseFireCompleted()
    {
        ShipBase ship = GetComponentInParent<ShipBase>();
        if (ship != null) ship.HandleSpecialFireCeased();
    }
}
