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

    private bool firedByEnemy;

    // Initialize method to set the projectile's properties
    public void Initialize(bool isEnemy, float speedModifier, float damageModifier, Vector2 initialVelocity, AttachPoint.RelativeSide side)
    {
        firedByEnemy = isEnemy;
        // Set the damage of the projectile
        BaseDamage *= damageModifier;
       // Call the specific behavior initialization
        InitializeBehaviour(speedModifier, initialVelocity, side);
    }

    // Abstract method to be implemented by specific projectiles
    protected abstract void InitializeBehaviour(float speedModifier,  Vector2 initialVelocity, AttachPoint.RelativeSide side);

    // Method to handle collision with other objects
    void OnTriggerEnter2D(Collider2D other)
    {
        string tag = other.gameObject.tag;

        // Code to execute when an object enters the trigger
        if ((tag == "Enemy" && !firedByEnemy) || (tag == "Player" && firedByEnemy))
        {
            ShipBase ship = other.GetComponent<ShipBase>();
            if (ship != null)
            {
                ship.TakeDamage(BaseDamage);
            }
            Explode();
        }

        if (tag == "Shield")
        {
            ElectroShieldEffect shield = other.GetComponent<ElectroShieldEffect>();
            if (shield != null)
            {
                if (shield.IsEnemyShield != firedByEnemy)
                {
                    shield.AbsorbHit(BaseDamage);
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
