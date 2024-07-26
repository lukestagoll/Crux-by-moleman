using UnityEngine;

public class FireRate : SkillBase
{
    public FireRate(int level) : base(level)
    {
        MaxLevel = 3;
        SkillName = "FireRate";
    }

    public override void Activate()
    {
        TargetShip.OnSpawn += OnSpawn;
    }

    private float DetermineFireRateModifier()
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
        TargetShip.FireRateModifier += DetermineFireRateModifier();
    }

    public override void Deactivate()
    {
        // Implementation for FireRate deactivation
    }
}