using UnityEngine;

public class AttackDrone : DroneShip
{
    [SerializeField] private float chargeDrainRate = 20f; // Charge drained per second in Aggressive mode
    protected override void ActivateEffect()
    {
        if (CurrentBehavior == DroneBehavior.Aggressive)
        {
            EnablePrimaryFire();
        }
        else
        {
            DisablePrimaryFire();
        }
    }

    protected override void Update()
    {
        base.Update(); // Call the base class Update method
        
        if (CurrentBehavior == DroneBehavior.Aggressive)
        {
            DrainCharge();
        }
    }

    private void DrainCharge()
    {
        SubtractCharge(chargeDrainRate * Time.deltaTime);
    }
}
