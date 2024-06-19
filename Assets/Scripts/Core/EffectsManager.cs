using UnityEngine;

public static class EffectsManager
{
    public static void ActivateEffect(GameObject target, EffectData effectData)
    {
        Debug.Log(effectData.Type);
        switch (effectData.Type)
        {
            // case Effect.Points:
            //     GameManager.IncrementScore((int)effectData.Amt);
            //     break;
            // case Effect.Lives:
            //     PlayerManager.Inst.IncrementLives((int)effectData.Amt);
            //     break;
            // case Effect.Health:
            //     ActivateHealthEffect(target, effectData);
            //     break;
            // case Effect.Speed:
            //     ActivateSpeedEffect(target, effectData);
            //     break;
            // case Effect.FireRate:
            //     ActivateFireRateEffect(target, effectData);
            //     break;
            case EffectType.Passive:
                break;
            case EffectType.Weapon:
                ActivateWeaponEffect(target, effectData);
                break;
            case EffectType.Instant:
                break;
            default:
                Debug.LogError("Unknown effect type");
                break;
        }
    }

    public static EffectData FetchEffectBySubType(EffectSubType subType)
    {
        return GameConfig.EffectDataList.Find(v => v.SubType == subType);
    }

    private static void ActivateWeaponEffect(GameObject targetShip, EffectData effectData)
    {
        WeaponBase weaponComponent = AssetManager.GetWeaponPrefab(effectData.SubType.ToString());
        if (weaponComponent != null)
        {
            BaseShip ship = targetShip.GetComponent<BaseShip>();
            ship.AttemptWeaponAttachment(weaponComponent, false);
        }
    }

    // private static void ActivateHealthEffect(GameObject targetShip, EffectData effect)
    // {
    //     BaseShip ship = targetShip.GetComponent<BaseShip>();
    //     ship.AddHitpoints(effect.Amt);
    // }

    // private static void ActivateSpeedEffect(GameObject targetShip, EffectData effect)
    // {
    // }

    // private static void ActivateFireRateEffect(GameObject targetShip, EffectData effect)
    // {
    // }
}
