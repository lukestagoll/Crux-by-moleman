public class Cannon : WeaponBase
{
    // Initialize default values
    protected void Awake()
    {
        hasAnimation = false;
        CurrentFireRate = BaseFireRate;
    }

}
