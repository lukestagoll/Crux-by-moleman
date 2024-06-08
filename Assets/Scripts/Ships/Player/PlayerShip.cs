using UnityEngine;

public class PlayerShip : BaseShip
{
    public static PlayerShip Inst { get; private set; }

    void Awake()
    {
        if (Inst != null && Inst != this)
        {
            Debug.Log("PlayerShip already exists");
            Destroy(gameObject);
            return;  // Ensure no further code execution in this instance
        }
        Inst = this;
    }

    protected override void Start()
    {
        base.Start(); // Call the base class Start method to initialize weapons

        hitpoints = GameConfig.MaxPlayerHealth; // Assign a default value or make this configurable via the Inspector
        HUDManager.Inst.UpdateHealthBar(hitpoints);
    }

    void Update()
    {
        // Check if left mouse button is held down
        if (Input.GetMouseButton(0) && !GameManager.IsPaused)
        {
            FireWeapons();
        }
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
            hitpoints -= damage;
            if (hitpoints <= 0) {
                isDestroyed = true;
                HUDManager.Inst.UpdateHealthBar(0);
                Die();
                return;
            }
            HUDManager.Inst.UpdateHealthBar(hitpoints);
        }
    }

    public override void FireWeapons()
    {
        foreach (var weapon in AttachedWeapons)
        {
            weapon.Shoot(false, FireRateModifier, DamageModifier, BulletSpeedModifier);
        }
    }
}
