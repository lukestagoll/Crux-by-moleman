using UnityEngine;

public class PlayerShip : ShipBase
{
    void Start()
    {
        ShieldIsActive = true;
        EmitOnSpawn();
        HUDManager.Inst.UpdateShieldBar();
        HUDManager.Inst.UpdateHealthBar();
    }

    public override void Die()
    {
        PlayerManager.Inst.HandlePlayerDestroyed();
        Explode();
    }

    protected override float SubtractShield(float damage)
    {
        Shield -= damage;

        if (Shield == 0) {
            DeactivateShield();
            return 0;
        }
        if (Shield < 0) {
            DeactivateShield();
            return damage + Shield;
        }
        else
        {
            HUDManager.Inst.UpdateShieldBar();
            return 0;
        }
    }

    protected override void SubtractHealth(float damage)
    {
        Health -= damage;
        if (Health <= 0) {
            Health = 0;
            isDestroyed = true;
            HUDManager.Inst.UpdateHealthBar();
            Die();
            return;
        }
        HUDManager.Inst.UpdateHealthBar();
    }

    public override void AddShield(float amt)
    {
        Shield += amt;
        ActivateShield();
        HUDManager.Inst.UpdateShieldBar();
    }

    public override void AddHealth(float amt)
    {
        Health += amt;
        HUDManager.Inst.UpdateHealthBar();
    }

    private void DeactivateShield()
    {
        ShieldIsActive = false;
        Shield = 0;
        HUDManager.Inst.UpdateShieldBar();
        MusicManager.Inst.PlayAudioFile("ShieldPowerDown2", 1f);
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = DefaultMaterial;
    }

    private void ActivateShield()
    {
        ShieldIsActive = true;
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = ShieldGlowMaterial;
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
