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

        //! Handle Inventory Stuff here
        if (AssignedWeaponSlot == null) return;

        MusicManager.Inst.PlaySoundEffect("GunLoad", 1f);
        if (AssignedWeaponSlot.WeaponType == WeaponType.Special) GameManager.HandleSpecialWeaponUnlock();

        if (Expiry == ExpiryType.Time && Duration > 0) {
            Debug.Log($"Expiry detected for {gameObject.name} with duration {Duration}");
            CoroutineManager.Inst.DeactivateEffectAfterDelay(this, Duration);
        }
        Debug.Log($"Attached {SubType}");
        HUDManager.Inst.UpdateWeaponSlotsDisplay();
    }
    public override void Deactivate()
    {
        Debug.Log("Deactivating");
        ShipBase shipComponent = TargetShip.GetComponent<ShipBase>();
        shipComponent.DetachWeaponsFromSlot(AssignedWeaponSlot);
    }
}