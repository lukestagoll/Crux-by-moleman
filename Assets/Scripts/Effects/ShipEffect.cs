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
            default:
                Debug.LogError("Unknown effect sub type");
                return;
        }
    }

    public override void DeActivate()
    {
        throw new System.NotImplementedException();
    }

    private void ActivateHealthEffect()
    {
        BaseShip ship = TargetShip.GetComponent<BaseShip>();
        ship.AddHitpoints(Amt);
    }

    private void ActivateFireRateEffect()
    {
    }
}