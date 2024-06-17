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

    public void ActivateEffect(GameObject target, EffectData effect)
    {
        switch (effect.name)
        {
            case Effect.Points:
                GameManager.IncrementScore((int)effect.amt);
                break;
            case Effect.Lives:
                PlayerManager.Inst.IncrementLives((int)effect.amt);
                break;
            case Effect.Health:
                ActivateHealthEffect(target, effect);
                break;
            case Effect.Speed:
                ActivateSpeedEffect(target, effect);
                break;
            case Effect.FireRate:
                ActivateFireRateEffect(target, effect);
                break;
            case Effect.Weapon:
                ActivateWeaponEffect(target, effect);
                break;
            default:
                Debug.LogError("Unknown effect type");
                break;
        }
    }

    private void ActivateWeaponEffect(GameObject targetShip, EffectData effect)
    {
        WeaponBase weaponComponent = AssetManager.GetWeaponPrefab(effect.type);
        if (weaponComponent != null)
        {
            BaseShip ship = targetShip.GetComponent<BaseShip>();
            ship.AttachWeapon(weaponComponent);
        }
    }

    private void ActivateHealthEffect(GameObject targetShip, EffectData effect)
    {
        BaseShip ship = targetShip.GetComponent<BaseShip>();
        ship.AddHitpoints(effect.amt);
    }

    private void ActivateSpeedEffect(GameObject targetShip, EffectData effect)
    {
    }

    private void ActivateFireRateEffect(GameObject targetShip, EffectData effect)
    {
    }
}
