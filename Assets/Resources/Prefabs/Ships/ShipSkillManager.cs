using System.Collections.Generic;
using UnityEngine;

public static class ShipSkillManager
{
    public static List<SkillBase> BuildSkillList(InitialShipData initialShipData, ShipBase targetShip)
    {
        List<SkillBase> _skills = new List<SkillBase>();
        var skills = initialShipData.Skills;
        foreach (var skillEntry in skills)
        {
            if (skillEntry.Value == 0) continue;

            SkillBase skill = CreateSkillInstance(skillEntry.Key, skillEntry.Value);
            if (skill != null)
            {
                if (targetShip != null) skill.AttemptActivation(targetShip);
                _skills.Add(skill);
            }
        }
        return _skills;
    }

    public static void AssignShipToSkills(List<SkillBase> skills, ShipBase targetShip)
    {
        if (skills.Count == 0 || targetShip == null) return;

        foreach (SkillBase skill in skills)
        {
            skill.AttemptActivation(targetShip);
        }
    }

    private static SkillBase CreateSkillInstance(string skillName, int level)
    {
        switch (skillName)
        {
            case "ShieldCapacity":
                return new ShieldCapacity(level);
            case "HullDurability":
                return new HullDurability(level);
            case "ShieldRegen":
                return new ShieldRegen(level);
            case "Damage":
                return new Damage(level);
            case "FireRate":
                return new FireRate(level);
            case "Piercing":
                return new Piercing(level);
            case "Speed":
                return new Speed(level);
            case "Evasion":
                return new Evasion(level);
            case "CriticalHit":
                return new CriticalHit(level);
            case "ShieldDrones":
                return new ShieldDrones(level);
            case "AttackDrones":
                return new AttackDrones(level);
            case "DroneFireRate":
                return new DroneFireRate(level);
            case "DroneChargeRate":
                return new DroneChargeRate(level);
            default:
                Debug.LogError($"Unknown skill: {skillName}");
                return null;
        }
    }
}
