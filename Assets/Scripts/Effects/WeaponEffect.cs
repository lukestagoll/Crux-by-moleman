using UnityEngine;

public class WeaponEffect : EffectBase
{
    private WeaponSlot AssignedWeaponSlot;
    public override void Activate(GameObject targetShip)
    {
        TargetShip = targetShip;
        WeaponBase weaponComponent = AssetManager.GetWeaponPrefab(SubType.ToString());
        if (weaponComponent == null)
        {
           Debug.LogError("Weapon prefab not found for " + targetShip.name);
        }

        BaseShip shipComponent = TargetShip.GetComponent<BaseShip>();
        AssignedWeaponSlot = shipComponent.AttemptWeaponAttachment(weaponComponent, false);

        if (Expiry == ExpiryType.Time && Duration > 0) {
            Debug.Log($"Expiry deteceted for {gameObject.name} with duration {Duration}");
            CoroutineManager.Inst.DeactivateEffectAfterDelay(this, Duration);
        }
    }
    public override void Deactivate()
    {
        Debug.Log("Deactivating");
        BaseShip shipComponent = TargetShip.GetComponent<BaseShip>();
        shipComponent.DetachWeaponsFromSlot(AssignedWeaponSlot);
    }
}