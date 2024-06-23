using UnityEngine;
using System;

public class ElectroShield : ToggleFireWeaponBase
{
    // Initialize default values
    private GameObject Shield;
    private GameObject ElectroShieldEffectPrefab;
    private GameObject ElectricExplosionPrefab;

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

        // Fetch the ElectricExplosion prefab from the AssetManager
        ElectricExplosionPrefab = AssetManager.GetProjectilePrefab("ElectricExplosion");
        if (ElectricExplosionPrefab == null)
        {
            Debug.LogError("Failed to load ElectricExplosion prefab!");
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

    protected override void CeaseFire(Action onCompleted)
    {
        if (ElectricExplosionPrefab != null)
        {
            // Instantiate the ElectricExplosion at the center of the parent ship
            ShipBase parentShip = GetComponentInParent<ShipBase>();
            if (parentShip != null)
            {
                Debug.Log("Spawning ElectricExplosion");
                GameObject electricExplosion = Instantiate(ElectricExplosionPrefab, parentShip.transform.position, Quaternion.identity, parentShip.transform);
                ElectricExplosion explosionScript = electricExplosion.GetComponent<ElectricExplosion>();
                if (explosionScript != null)
                {
                    explosionScript.OnExplosionFinished += () =>
                    {
                        Destroy(Shield);
                        onCompleted?.Invoke();
                    };
                }
            }
            else
            {
                Debug.LogError("Parent ship not found!");
                onCompleted?.Invoke();
            }
        }
        else
        {
            Debug.Log("ElectricExplosionPrefab was null");
            Destroy(Shield);
            onCompleted?.Invoke();
        }
    }
}
