using UnityEngine;

public class Piercing : SkillBase
{
    public Piercing(int level) : base(level)
    {
        MaxLevel = 3;
        SkillName = "Piercing";
    }

    public override void Activate()
    {
        TargetShip.OnSpawn += OnSpawn;
    }

    private int DeterminePiercingModifier()
    {
        switch (Level)
        {
            case 1:
                return 1;
            case 2:
                return 2;
            case 3:
                return 3;
            default:
                Debug.LogError(SkillName + " level is invalid");
                return 1;
        }
    }

    private void OnSpawn()
    {
        TargetShip.PiercingModifier += DeterminePiercingModifier();
    }

    public override void Deactivate()
    {
        // Implementation for Piercing deactivation
    }
}