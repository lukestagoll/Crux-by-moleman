using UnityEngine;

public class EnemyShip : BaseShip
{
    [SerializeField] protected int pointsOnKill;
    [SerializeField] protected int damageOnCollision = 10;

    public delegate void EnemyShipEvent(EnemyShip ship);
    public event EnemyShipEvent OnDestroyed;

    protected override void Start()
    {
        base.Start(); // Call the base class Start method to initialize weapons
        isEnemy = true;
    }

    public override void Die()
    {
        GameManager.IncrementScore(pointsOnKill);
        Explode();
    }

    void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }

    public override void TakeDamage(float damage)
    {
        if (!isDestroyed)
        {
            hitpoints -= damage;
            if (hitpoints <= 0)
            {
                isDestroyed = true;
                Die();
            }
        }
    }

    public override void AddHitpoints(float amt)
    {
        hitpoints += amt;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        string tag = other.gameObject.tag;

        // Code to execute when an object enters the trigger
        if (tag == "Player")
        {
            BaseShip ship = other.GetComponent<BaseShip>();
            if (ship != null)
            {
                ship.TakeDamage(damageOnCollision);
            }
            // Instantiate the explosion prefab at the projectile's position
            // Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
            Die();
        }
    }
}
