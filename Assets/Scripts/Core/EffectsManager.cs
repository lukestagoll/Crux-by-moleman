using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Inst { get; private set; }

    void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Debug.Log("EffectsManager already exists");
            Destroy(gameObject);
            return;  // Ensure no further code execution in this instance
        }
        Inst = this;
    }

    public void ActivateEffect(GameObject target, EffectData effectData)
    {
        Debug.Log(effectData);
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

    private void ActivateWeaponEffect(GameObject targetShip, EffectData effectData)
    {
        WeaponBase weaponComponent = AssetManager.GetWeaponPrefab(effectData.SubType);
        if (weaponComponent != null)
        {
            BaseShip ship = targetShip.GetComponent<BaseShip>();
            ship.AttemptWeaponAttachment(weaponComponent, false);
        }
    }

    private void ActivateHealthEffect(GameObject targetShip, EffectData effect)
    {
        BaseShip ship = targetShip.GetComponent<BaseShip>();
        ship.AddHitpoints(effect.Amt);
    }

    private void ActivateSpeedEffect(GameObject targetShip, EffectData effect)
    {
    }

    private void ActivateFireRateEffect(GameObject targetShip, EffectData effect)
    {
    }
}
