using UnityEngine;

public class HullDurability : SkillBase
{
    public HullDurability(int level) : base(level) {}

    public override void Activate()
    {
        // Implementation for HullDurability activation
        Debug.Log("HULL DURABILITY ACTIVATED AT LEVEL: " + Level);
    }

        public override void Deactivate()
    {
        // Implementation for HullDurability activation
    }
}