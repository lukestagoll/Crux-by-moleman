public class HullDurability : SkillBase
{
    public HullDurability(int level) : base(level)
    {
        MaxLevel = 3;
        SkillName = "HullDurability";
    }

    public override void Activate()
    {
        TargetShip.OnSpawn += OnSpawn;
    }

    private float DetermineMaxHealthModifier()
    {
        switch (Level)
        {
            case 1:
                return 1.5f;
            case 2:
                return 2f;
            case 3:
                return 2.5f;
            default:
                return 1f;
        }
    }

    private void OnSpawn()
    {
        TargetShip.MaxHealth *= DetermineMaxHealthModifier();
        TargetShip.Health = TargetShip.MaxHealth;
    }

    public override void Deactivate()
    {
        // Implementation for HullDurability deactivation
    }
}