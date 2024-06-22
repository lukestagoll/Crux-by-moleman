using UnityEngine;

public class WeaponEffect : EffectBase
{
    private WeaponSlot AssignedWeaponSlot;
    public override void Activate(GameObject targetShip)
    {
        TargetShip = targetShip;
        GameObject weaponPrefab = AssetManager.GetWeaponPrefab(SubType.ToString());
        if (weaponPrefab == null)
        {
           Debug.LogError("Weapon prefab not found for " + targetShip.name);
        }

        ShipBase shipComponent = TargetShip.GetComponent<ShipBase>();
        AssignedWeaponSlot = shipComponent.AttemptWeaponAttachment(weaponPrefab, false);

        if (Expiry == ExpiryType.Time && Duration > 0) {
            Debug.Log($"Expiry deteceted for {gameObject.name} with duration {Duration}");
            CoroutineManager.Inst.DeactivateEffectAfterDelay(this, Duration);
        }
    }
    public override void Deactivate()
    {
        Debug.Log("Deactivating");
        ShipBase shipComponent = TargetShip.GetComponent<ShipBase>();
        shipComponent.DetachWeaponsFromSlot(AssignedWeaponSlot);
    }
}