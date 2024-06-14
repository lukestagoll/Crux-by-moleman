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

    public void ActivateEffect(GameObject gameObject, EffectData effect)
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
                ActivateHealthEffect(gameObject, effect);
                break;
            case Effect.Speed:
                ActivateSpeedEffect(gameObject, effect);
                break;
            case Effect.FireRate:
                ActivateFireRateEffect(gameObject, effect);
                break;
            case Effect.Weapon:
                ActivateWeaponEffect(gameObject, effect);
                break;
            default:
                Debug.LogError("Unknown effect type");
                break;
        }
    }

    private void ActivateWeaponEffect(GameObject gameObject, EffectData effect)
    {
        WeaponBase weaponPrefab = AssetManager.GetWeaponPrefab(effect.type);
        if (weaponPrefab != null)
        {
            BaseShip ship = gameObject.GetComponent<BaseShip>();
            ship.AttachWeapon(weaponPrefab);
        }
    }

    private void ActivateHealthEffect(GameObject gameObject, EffectData effect)
    {
        BaseShip ship = gameObject.GetComponent<BaseShip>();
        ship.AddHitpoints(effect.amt);
    }

    private void ActivateSpeedEffect(BaseShip ship, EffectData effect)
    {
    }

    private void ActivateFireRateEffect(BaseShip ship, EffectData effect)
    {
    }
}
