using UnityEngine;

public class ShieldDrone : DroneShip
{
    private GameObject activeShield;

    protected override void ActivateEffect()
    {
        if (CurrentBehavior == DroneBehavior.Aggressive)
        {
            ActivateDroneShield();
        }
        else
        {
            DeactivateDroneShield();
        }
    }

    private void ActivateDroneShield()
    {
        if (activeShield == null)
        {
            Vector3 shieldPosition = transform.position + new Vector3(0, 0.5f, 0);
            activeShield = Instantiate(AssetManager.DroneShieldPrefab, shieldPosition, Quaternion.identity, transform);
        }
    }

    private void DeactivateDroneShield()
    {
        if (activeShield != null)
        {
            Destroy(activeShield);
            activeShield = null;
        }
    }

    // protected override void OnDestroy()
    // {
    //     base.OnDestroy();
    //     DeactivateDroneShield();
    // }
}
