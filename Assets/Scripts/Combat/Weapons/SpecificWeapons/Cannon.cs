using UnityEngine;

public class Cannon : WeaponBase
{
    // Initialize default values
    private void Awake()
    {
        hasAnimation = false;
        CurrentFireRate = BaseFireRate;
    }

}
