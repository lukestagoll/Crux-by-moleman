using UnityEngine;

public class DroneFireRate : SkillBase
{
    public DroneFireRate(int level) : base(level)
    {
        MaxLevel = 3;
        SkillName = "DroneFireRate";
    }

    public override void Activate()
    {
        TargetShip.OnSpawn += OnSpawn;
    }

    private float DetermineDroneFireRateModifier()
    {
        switch (Level)
        {
            case 1:
                return 0.05f;
            case 2:
                return 0.1f;
            case 3:
                return 0.15f;
            default:
                Debug.LogError(SkillName + " level is invalid");
                return 0.05f;
        }
    }

    private void OnSpawn()
    {
        TargetShip.DroneFireRateModifier += DetermineDroneFireRateModifier();
    }

    public override void Deactivate()
    {
        // Implementation for DroneFireRate deactivation
    }
}