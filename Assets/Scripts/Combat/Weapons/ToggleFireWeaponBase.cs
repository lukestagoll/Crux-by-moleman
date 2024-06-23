using System;

public abstract class ToggleFireWeaponBase : WeaponBase
{
    public float Cooldown = 3f;

    public override void AttemptFire(bool isEnemy, float fireRateModifier, float damageModifier, float bulletSpeedModifier)
    {
        Fire(isEnemy, bulletSpeedModifier, damageModifier);
        ShipBase ship = GetComponentInParent<ShipBase>();
        ship.SpecialIsActivated = true;
    }

    public override void AttemptCeaseFire()
    {
        ShipBase ship = GetComponentInParent<ShipBase>();
        ship.SpecialIsCeasing = true;
        CeaseFire(OnCeaseFireCompleted);
    }

    protected abstract void Fire(bool isEnemy, float bulletSpeedModifier, float damageModifier);
    protected abstract void CeaseFire(Action onCompleted);

    protected virtual void OnCeaseFireCompleted()
    {
        ShipBase ship = GetComponentInParent<ShipBase>();
        if (ship != null)
        {
            ship.SpecialIsActivated = false;
            ship.SpecialIsCeasing = false;
        }
    }
}
