using UnityEngine;

public class ShieldDrone : DroneShip
{
    protected override void ActivateEffect()
    {
        if (CurrentBehavior == DroneBehavior.Aggressive)
        {
            Debug.Log("EnablingSpecialFire for ShieldDrone");
            EnableSpecialFire();
        }
        else
        {
            Debug.Log("DisablingSpecialFire for ShieldDrone");
            DisableSpecialFire();
        }
    }
}
