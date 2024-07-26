using UnityEngine;

public class ShieldCapacity : SkillBase
{
    public ShieldCapacity(int level) : base(level)
    {
        MaxLevel = 3;
        SkillName = "ShieldCapacity";
    }

    public override void Activate()
    {
        TargetShip.OnSpawn += OnSpawn;
    }

    private float DetermineMaxShieldModifier()
    {
        switch (Level)
        {
            case 1:
                return 1.15f;
            case 2:
                return 1.3f;
            case 3:
                return 1.5f;
            default:
                Debug.LogError(SkillName + " level is invalid");
                return 1.15f;
        }
    }

    private void OnSpawn()
    {
        TargetShip.MaxShield *= DetermineMaxShieldModifier();
        TargetShip.Shield = TargetShip.MaxShield;
    }

    public override void Deactivate()
    {
        // Implementation for ShieldCapacity deactivation
    }
}