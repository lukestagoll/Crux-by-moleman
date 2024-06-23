using UnityEngine;

public class PlayerShip : ShipBase
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

        Hitpoints = GameConfig.MaxPlayerHealth; // Assign a default value or make this configurable via the Inspector
        HUDManager.Inst.UpdateHealthBar(Hitpoints);
    }

    void Update()
    {
        if (GameManager.IsPaused) return;
        // Check if left mouse button is held down
        if (Input.GetMouseButton(0))
        {
            FireWeapons(WeaponType.Primary);
        }

        // Check if spacebar is held down
        if (Input.GetKey(KeyCode.Space) && !SpecialIsActivated)
        {
            Debug.Log("[PlayerShip] Special firing");
            FireWeapons(WeaponType.Special);
        }
        else if (!Input.GetKey(KeyCode.Space) && SpecialIsActivated && !SpecialIsCeasing)
        {
            Debug.Log("[PlayerShip] Special ceasing");
            CeaseFire(WeaponType.Special);
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
}
