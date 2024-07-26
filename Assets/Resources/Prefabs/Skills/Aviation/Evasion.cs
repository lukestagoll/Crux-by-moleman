using UnityEngine;

public class Evasion : SkillBase
{
    public Evasion(int level) : base(level)
    {
        MaxLevel = 3;
        SkillName = "Evasion";
    }

    public override void Activate()
    {
        TargetShip.OnSpawn += OnSpawn;
    }

    private float DetermineEvasionModifier()
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
                return 0.1f;
        }
    }

    private void OnSpawn()
    {
        TargetShip.EvasionChanceModifier += DetermineEvasionModifier();
    }

    public override void Deactivate()
    {
        // Implementation for Evasion deactivation
    }
}