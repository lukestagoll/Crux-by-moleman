using UnityEngine;

public abstract class ToggleFireWeaponBase : WeaponBase
{ 
    public override void AttemptFire(bool isEnemy, float fireRateModifier, float damageModifier, float bulletSpeedModifier)
    {
      Debug.Log("AttemptingFire");
      Fire(isEnemy, bulletSpeedModifier, damageModifier);
      ShipBase ship = GetComponentInParent<ShipBase>();
      ship.SpecialIsActivated = true;
    }

    public override void AttemptCeaseFire()
    {
      Debug.Log("CeasingFire");
      CeaseFire();
      ShipBase ship = GetComponentInParent<ShipBase>();
      ship.SpecialIsActivated = false;
    }
    protected abstract void Fire(bool isEnemy, float bulletSpeedModifier, float damageModifier);
    protected abstract void CeaseFire();
}
