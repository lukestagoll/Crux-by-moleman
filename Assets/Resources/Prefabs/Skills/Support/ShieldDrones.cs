using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldDrones : SkillBase
{
    private List<DroneShip> ActiveDrones = new List<DroneShip>();
    private int MaxShieldDrones;

    public ShieldDrones(int level) : base(level)
    {
        MaxLevel = 3;
        SkillName = "ShieldDrones";
    }

    public override void Activate()
    {
        MaxShieldDrones = DetermineMaxShieldDrones();
        TargetShip.OnSpawn += OnSpawn;
        TargetShip.OnDeath += OnDeath;
    }

    private IEnumerator SpawnInitialDrones()
    {
        while (ActiveDrones.Count < MaxShieldDrones)
        {
            yield return new WaitForSeconds(3f);
            SpawnDrone();
        }
    }

    private void RemoveDroneFromActiveList(DroneShip drone)
    {
        ActiveDrones.Remove(drone);
        TargetShip.StartCoroutine(SpawnReplacementDrone());
    }

    private IEnumerator SpawnReplacementDrone()
    {
        yield return new WaitForSeconds(10f);

        if (ActiveDrones.Count < MaxShieldDrones)
        {
            SpawnDrone();
        }
    }

    private void SpawnDrone()
    {
        if (TargetShip == null) return;
        DroneShip newDrone = TargetShip.SpawnShieldDrone();
        if (newDrone != null)
        {
            ActiveDrones.Add(newDrone);
            newDrone.OnDeath += () => RemoveDroneFromActiveList(newDrone);
        }
        else
        {
            Debug.LogError("DroneShip component not found on instantiated drone prefab");
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
        TargetShip.StartCoroutine(SpawnInitialDrones());
    }

    private void OnDeath()
    {
        TargetShip.StopCoroutine(SpawnInitialDrones());
        TargetShip.StopCoroutine(SpawnReplacementDrone());
        DestroyAllDrones();
    }

    private void DestroyAllDrones()
    {
        for (int i = ActiveDrones.Count - 1; i >= 0; i--)
        {
            DroneShip droneShip = ActiveDrones[i];
            if (droneShip != null)
            {
                droneShip.OnDeath -= () => RemoveDroneFromActiveList(droneShip);
                droneShip.Explode();
            }
        }
        ActiveDrones.Clear();
    }

    public override void Deactivate()
    {
        // Implementation for ShieldDrones deactivation
    }
}