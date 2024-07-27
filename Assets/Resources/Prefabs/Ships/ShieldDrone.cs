using UnityEngine;

public class ShieldDrone : DroneShip
{
    protected override void ActivateEffect()
    {
        if (CurrentBehavior == DroneBehavior.Aggressive)
        {
            EnableSpecialFire();
        }
        else
        {
            DisableSpecialFire();
        }
    }
}
