using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDrones : SkillBase
{
    private List<DroneShip> ActiveDrones = new List<DroneShip>();
    private int MaxShieldDrones;
    private Coroutine spawnCoroutine;

    public ShieldDrones(int level) : base(level)
    {
        MaxLevel = 3;
        SkillName = "ShieldDrones";
    }

    public override void Activate()
    {
        MaxShieldDrones = DetermineMaxShieldDrones();
        TargetShip.OnSpawn += OnSpawn;
        // OnDeath += OnDeath;
    }

    private IEnumerator SpawnDrones()
    {
        while (ActiveDrones.Count < MaxShieldDrones)
        {
            yield return new WaitForSeconds(3f);
            DroneShip droneShip = TargetShip.SpawnShieldDrone();

            if (droneShip != null)
            {
                ActiveDrones.Add(droneShip);
            }
            else
            {
                Debug.LogError("DroneShip component not found on instantiated drone prefab");
            }
        }
    }

    private int DetermineMaxShieldDrones()
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
        spawnCoroutine = TargetShip.StartCoroutine(SpawnDrones());
    }

    private void OnDeath()
    {
        TargetShip.StopCoroutine(spawnCoroutine);
        DestroyAllDrones();
    }

    private void DestroyAllDrones()
    {
      foreach (DroneShip droneShip in ActiveDrones)
        {
          if (droneShip != null) Object.Destroy(droneShip.gameObject);
        }
    }

    public override void Deactivate()
    {
        // Implementation for ShieldDrones deactivation
    }
}