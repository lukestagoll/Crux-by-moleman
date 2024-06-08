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
    public void Initialize(bool isEnemy, float speedModifier, float damageModifier, Vector2 initialVelocity)
    {
        firedByEnemy = isEnemy;
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Set the velocity of the projectile
        rb.velocity = initialVelocity + (Vector2)(transform.up * BaseSpeed * speedModifier);

        // Set the damage of the projectile
        BaseDamage *= damageModifier;
    }

    // Method to handle collision with other objects
    void OnTriggerEnter2D(Collider2D other)
    {
        string tag = other.gameObject.tag;

        // Code to execute when an object enters the trigger
        if ((tag == "Enemy" && !firedByEnemy) || (tag == "Player" && firedByEnemy))
        {
            BaseShip ship = other.GetComponent<BaseShip>();
            if (ship != null)
            {
                ship.TakeDamage(BaseDamage);
            }
            // Instantiate the explosion prefab at the projectile's position
            Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }
}
