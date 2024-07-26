using UnityEngine;

public class Speed : SkillBase
{
    public Speed(int level) : base(level)
    {
        MaxLevel = 3;
        SkillName = "Speed";
    }

    public override void Activate()
    {
        TargetShip.OnSpawn += OnSpawn;
    }

    private float DetermineMovementSpeedModifier()
    {
        switch (Level)
        {
            case 1:
                return 0.1f;
            case 2:
                return 0.15f;
            case 3:
                return 0.2f;
            default:
                Debug.LogError(SkillName + " level is invalid");
                return 0.05f;
        }
    }

    private void OnSpawn()
    {
        TargetShip.MovementSpeedModifier += DetermineMovementSpeedModifier();
    }

    public override void Deactivate()
    {
        // Implementation for Speed deactivation
    }
}