using UnityEngine;

public class WeaponEffect : EffectBase
{
    public override void Activate(GameObject targetShip)
    {
        TargetShip = targetShip;
        WeaponBase weaponComponent = AssetManager.GetWeaponPrefab(SubType.ToString());
        if (weaponComponent != null)
        {
            BaseShip ship = targetShip.GetComponent<BaseShip>();
            ship.AttemptWeaponAttachment(weaponComponent, false);
        }
    }

    public override void DeActivate()
    {
        throw new System.NotImplementedException();
    }
}