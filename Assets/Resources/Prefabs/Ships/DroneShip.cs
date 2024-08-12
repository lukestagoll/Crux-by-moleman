using UnityEngine;

public enum DroneBehavior
{
    Passive,
    Aggressive
}

public abstract class DroneShip : ShipBase
{
    public ShipBase ParentShip;
    public DroneBehavior CurrentBehavior = DroneBehavior.Passive;

    private float chargeRate = 5f;
    private float maxDistance = 2f;

    protected Vector3 currentLocalTarget;
    protected Vector3 nextLocalTarget;
    protected float curveProgress = 1f;
    protected float curveDuration = 1.5f;
    public GameObject ParentDroneAnchor;

    protected virtual void Start()
    {
        ShieldIsActive = false;
        EmitOnSpawn();
        InvokeRepeating("UpdateCharge", 0f, 1f);
    }

    protected virtual void Update()
    {
        UpdateBehavior();
        MoveDrone();
    }

    public void SubtractCharge(float amount)
    {
        Charge = Mathf.Max(0, Charge - amount);
    }

    void UpdateCharge()
    {
        if (CurrentBehavior == DroneBehavior.Passive && Charge < MaxCharge)
        {
            Charge = Mathf.Min(MaxCharge, Charge + chargeRate * ChargeRateModifier);
        }
    }

    void UpdateBehavior()
    {
        if (Charge > 85)
        {
            SetBehavior(DroneBehavior.Aggressive);
        }
        else if (Charge == 0)
        {
            SetBehavior(DroneBehavior.Passive);
        }
    }

    void SetBehavior(DroneBehavior newBehavior)
    {
        if (CurrentBehavior != newBehavior)
        {
            CurrentBehavior = newBehavior;
            curveProgress = 1f; // Force new target selection
            ActivateEffect();
        }
    }

    protected abstract void ActivateEffect();

    protected abstract void MoveDrone();

    protected Vector3 PickNewRelativeLocation()
    {
        if (CurrentBehavior == DroneBehavior.Aggressive)
        {
            float x = Random.Range(-maxDistance, maxDistance);
            float y = Random.Range(0.5f, 1f);
            return new Vector3(x, y, 0);
        }
        else
        {
            float x = Random.Range(-maxDistance, maxDistance);
            float y = Random.Range(-maxDistance, maxDistance * 0.5f);
            return new Vector3(x, y, 0);
        }
    }

    protected Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        return uu * p0 + 2 * u * t * p1 + tt * p2;
    }

    // BASIC BITCH STUFF
    public override void Die()
    {
        Explode();
    }

    protected override float SubtractShield(float damage)
    {
        Shield -= damage;

        if (Shield == 0) {
            DeactivateShield();
            return 0;
        }
        else if (Shield < 0) {
            float excessDamage = damage + Shield;
            DeactivateShield();
            return excessDamage;
        }
        else return 0;
    }

    protected override void SubtractHealth(float damage)
    {
        Health -= damage;
        if (Health <= 0) {
            Health = 0;
            isDestroyed = true;
            Die();
            return;
        }
    }

    public override void AddShield(float amt)
    {
        Shield += amt;
        ActivateShield();
    }

    public override void AddHealth(float amt)
    {
        Health += amt;
    }

    private void DeactivateShield()
    {
        ShieldIsActive = false;
        Shield = 0;
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = DefaultMaterial;
    }

    private void ActivateShield()
    {
        ShieldIsActive = true;
        Renderer renderer = GetComponent<Renderer>();
        renderer.material = ShieldGlowMaterial;
    }

    protected override void AttachWeaponsToSlot(GameObject weaponPrefab, WeaponSlot weaponSlot)
    {
        foreach (AttachPoint attachPoint in weaponSlot.AttachPoints)
        {
            attachPoint.AttachWeapon(weaponPrefab, true);
            ActiveAttachPoints.Add(attachPoint);
        }
        weaponSlot.IsEmpty = false;
        // play audio here
    }

    //! DRONES CAN BE EITHER SIDE SO.... THINK ABOUT IT
    // void OnTriggerEnter2D(Collider2D other)
    // {
    //     string tag = other.gameObject.tag;

    //     // Code to execute when an object enters the trigger
    //     if (tag == "Enemy")
    //     {
    //         EnemyShip enemyShip = other.GetComponent<EnemyShip>();
    //         if (enemyShip != null)
    //         {
    //             TakeDamage(enemyShip.damageOnCollision, 0);
    //             enemyShip.Die();
    //         }
    //         // Instantiate the explosion prefab at the projectile's position
    //         // Instantiate(ExplosionPrefab, transform.position, Quaternion.identity);
    //     }
    // }
}
