using UnityEngine;

public class WeaponItem : ItemBase
{
    public GameObject weaponPrefab;
    public int quantity;

    protected override void TriggerEffect(GameObject player)
    {
        BaseShip ship = player.GetComponent<BaseShip>();
        if (ship != null)
        {
            ship.AttachWeapon(weaponPrefab, quantity);
        }
    }
}
