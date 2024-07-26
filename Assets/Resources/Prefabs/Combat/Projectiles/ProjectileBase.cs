using UnityEngine;

public abstract class ProjectileBase : MonoBehaviour
{
    // Protected variables to be set by inheriting classes
    [SerializeField]
    protected float BaseDamage;
    [SerializeField]
    protected float BaseSpeed;
    [SerializeField]
    protected GameObject ExplosionPrefab;

    protected bool FiredByEnemy;
    protected float SpeedModifier;
    protected float DamageModifier;

    // Initialize method to set the projectile's properties
    public void Initialize(bool isEnemy, float speedModifier, float damageModifier, Vector2 initialVelocity, AttachPoint.RelativeSide side, Vector2? direction = null)
    {
        FiredByEnemy = isEnemy;
        SpeedModifier = speedModifier;
        DamageModifier = damageModifier;
       // Call the specific behavior initialization
        InitializeBehaviour(initialVelocity, side, direction);
    }

    // Abstract method to be implemented by specific projectiles
    protected abstract void InitializeBehaviour(Vector2 initialVelocity, AttachPoint.RelativeSide side, Vector2? direction);

    // Method to handle collision with other objects
    void OnTriggerEnter2D(Collider2D other)
    {
        string tag = other.gameObject.tag;

        // Code to execute when an object enters the trigger
        if ((tag == "Enemy" && !FiredByEnemy) || (tag == "Player" && FiredByEnemy))
        {
            ShipBase ship = other.GetComponent<ShipBase>();
            if (ship != null)
            {
                ship.TakeDamage(BaseDamage * DamageModifier);
            }
            Explode();
        }

        if (tag == "Shield")
        {
            ElectroShieldEffect shield = other.GetComponent<ElectroShieldEffect>();
            if (shield != null)
            {
                if (shield.IsEnemyShield != FiredByEnemy)
                {
                    shield.AbsorbHit(BaseDamage * DamageModifier);
                    Explode();
                }
            }
        }
    }

    public void Explode()
    {
        // Instantiate the explosion prefab at the projectile's position
        Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
