public class MissileLauncher : WeaponBase
{
    // Initialize default values
    protected override void Awake()
    {
        base.Awake(); // Call the base class Awake method if needed
        hasAnimation = false;
        CurrentFireRate = BaseFireRate;
    }

}
