using UnityEngine;

public class ShipEffect : EffectBase
{
    public override void Activate(GameObject targetShip)
    {
        TargetShip = targetShip;
        switch (SubType)
        {
            case EffectSubType.FireRate:
                ActivateFireRateEffect();
                break;
            case EffectSubType.Health:
                ActivateHealthEffect();
                break;
            case EffectSubType.Damage:
                ActivateDamageEffect();
                break;
            default:
                Debug.LogError("Unknown effect sub type");
                return;
        }

        if (Expiry == ExpiryType.Time && Duration > 0) {
            Debug.Log($"Expiry deteceted for {gameObject.name} with duration {Duration}");
            CoroutineManager.Inst.DeactivateEffectAfterDelay(this, Duration);
        }
    }

    public override void Deactivate()
    {
        switch (SubType)
        {
            case EffectSubType.FireRate:
                DeactivateFireRateEffect();
                break;
            case EffectSubType.Damage:
                DeactivateDamageEffect();
                break;
            default:
                Debug.LogError("Unknown effect sub type");
                return;
        }
    }

    private void ActivateHealthEffect()
    {
        BaseShip ship = TargetShip.GetComponent<BaseShip>();
        ship.AddHitpoints(Amt);
    }

    private void ActivateDamageEffect()
    {
        TargetShip.GetComponent<BaseShip>().DamageModifier += Amt;
    }

    private void DeactivateDamageEffect()
    {
        TargetShip.GetComponent<BaseShip>().DamageModifier -= Amt;
    }

    private void ActivateFireRateEffect()
    {
        TargetShip.GetComponent<BaseShip>().FireRateModifier += Amt;
    }

    private void DeactivateFireRateEffect()
    {
        TargetShip.GetComponent<BaseShip>().FireRateModifier -= Amt;
    }
}