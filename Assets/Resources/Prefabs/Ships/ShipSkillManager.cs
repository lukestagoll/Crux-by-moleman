using System.Collections.Generic;
using UnityEngine;

public static class ShipSkillManager
{
    public static List<SkillBase> Initialise(InitialShipData initialShipData)
    {
        List<SkillBase> _skills = new List<SkillBase>();
        var skills = initialShipData.Skills;
        foreach (var skillEntry in skills)
        {
            SkillBase skill = CreateSkillInstance(skillEntry.Key, skillEntry.Value);
            if (skill != null)
            {
                skill.Activate();
                _skills.Add(skill);
            }
        }
        return _skills;
    }

    private static SkillBase CreateSkillInstance(string skillName, int level)
    {
        switch (skillName)
        {
            // case "ShieldDrones":
            //     return new ShieldDrones(level);
            case "HullDurability":
                return new HullDurability(level);
            default:
                Debug.LogError($"Unknown skill: {skillName}");
                return null;
        }
    }
}
