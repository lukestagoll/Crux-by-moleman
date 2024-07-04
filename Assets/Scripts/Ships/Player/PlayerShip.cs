using UnityEngine;

public class PlayerShip : ShipBase
{
    void Start()
    {

        Hitpoints = GameConfig.MaxPlayerHealth; // Assign a default value or make this configurable via the Inspector
        HUDManager.Inst.UpdateHealthBar(Hitpoints);
    }

    public override void Die()
    {
        PlayerManager.Inst.HandlePlayerDestroyed();
        Explode();
    }

    public override void TakeDamage(float damage)
    {
        if (!isDestroyed)
        {
            Hitpoints -= damage;
            if (Hitpoints <= 0) {
                isDestroyed = true;
                HUDManager.Inst.UpdateHealthBar(0);
                Die();
                return;
            }
            HUDManager.Inst.UpdateHealthBar(Hitpoints);
        }
    }

    public override void AddHitpoints(float amt)
    {
        Hitpoints += amt;
        HUDManager.Inst.UpdateHealthBar(Hitpoints);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        string tag = other.gameObject.tag;

        // Code to execute when an object enters the trigger
        if (tag == "Enemy")
        {
            EnemyShip enemyShip = other.GetComponent<EnemyShip>();
            if (enemyShip != null)
            {
                TakeDamage(enemyShip.damageOnCollision);
                enemyShip.Die();
            }
            // Instantiate the explosion prefab at the projectile's position
            // Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
        }
    }
}
