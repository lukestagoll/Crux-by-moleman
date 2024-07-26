using UnityEngine;

public class CriticalHit : SkillBase
{
    public CriticalHit(int level) : base(level)
    {
        MaxLevel = 3;
        SkillName = "CriticalHit";
    }

    public override void Activate()
    {
        TargetShip.OnSpawn += OnSpawn;
    }

    private float DetermineCriticalHitModifier()
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
        TargetShip.CriticalHitChanceModifier += DetermineCriticalHitModifier();
    }

    public override void Deactivate()
    {
        // Implementation for CriticalHit deactivation
    }
}