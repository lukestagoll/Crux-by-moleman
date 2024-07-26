using UnityEngine;

public class Damage : SkillBase
{
    public Damage(int level) : base(level)
    {
        MaxLevel = 3;
        SkillName = "Damage";
    }

    public override void Activate()
    {
        TargetShip.OnSpawn += OnSpawn;
    }

    private float DetermineDamageModifier()
    {
        switch (Level)
        {
            case 1:
                return 0.15f;
            case 2:
                return 0.3f;
            case 3:
                return 0.45f;
            default:
                Debug.LogError(SkillName + " level is invalid");
                return 1f;
        }
    }

    private void OnSpawn()
    {
        TargetShip.DamageModifier += DetermineDamageModifier();
    }

    public override void Deactivate()
    {
        // Implementation for Damage deactivation
    }
}