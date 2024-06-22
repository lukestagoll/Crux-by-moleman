using UnityEngine;

public class ElectroShield : ToggleFireWeaponBase
{
    // Initialize default values
    private GameObject Shield;
    private GameObject ElectroShieldEffectPrefab;

    protected void Awake()
    {
        hasAnimation = false;
        CurrentFireRate = BaseFireRate;
        WeaponType = WeaponType.Special;

        // Fetch the ElectroShieldEffect prefab from the AssetManager
        ElectroShieldEffectPrefab = AssetManager.GetWeaponPrefab("ElectroShieldEffect");
        if (ElectroShieldEffectPrefab == null)
        {
            Debug.LogError("Failed to load ElectroShieldEffect prefab!");
        }
    }
    protected override void Fire(bool isEnemy, float bulletSpeedModifier, float damageModifier)
    {
        if (ElectroShieldEffectPrefab != null)
        {
            // Instantiate the ElectroShieldEffect at the center of the parent ship
            ShipBase parentShip = GetComponentInParent<ShipBase>();
            if (parentShip != null)
            {
                Shield = Instantiate(ElectroShieldEffectPrefab, parentShip.transform.position, Quaternion.identity, parentShip.transform);
                Shield.GetComponent<ElectroShieldEffect>().IsEnemyShield = isEnemy;
            }
            else
            {
                Debug.LogError("Parent ship not found!");
            }
        }
    }
    protected override void CeaseFire()
    {
        Destroy(Shield);
    }

}
