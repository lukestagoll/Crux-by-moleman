using UnityEngine;

public enum DroneBehavior
{
    Defensive,
    Passive,
    Aggressive
}

public class DroneShip : ShipBase
{
    public float Charge = 25f;
    public ShipBase ParentShip;
    public DroneBehavior CurrentBehavior = DroneBehavior.Passive;

    private Vector3 aggressivePosition;
    private float chargeRate = 5f;
    private float maxDistance = 2f;


    //MOVEMENMT
private Vector3 currentLocalTarget;
private Vector3 nextLocalTarget;
private float curveProgress = 1f; // Start at 1 to immediately pick a new target
private float curveDuration = 1.5f; // Time to complete one curve
public GameObject ParentDroneAnchor;
private bool isLeftSide = true;

    void Start()
    {
        ShieldIsActive = false;
        EmitOnSpawn();
        InvokeRepeating("UpdateCharge", 0f, 1f);
    }

    void Update()
    {
        UpdateBehavior();
        MoveDrone();
    }

    void UpdateCharge()
    {
        if (CurrentBehavior == DroneBehavior.Aggressive)
        {
            Charge = Mathf.Max(0, Charge - chargeRate);
        }
        else
        {
            Charge = Mathf.Min(100, Charge + chargeRate);
        }
    }

    void UpdateBehavior()
    {
        if (Charge > 85)
        {
            SetBehavior(DroneBehavior.Aggressive);
        }
        else if (Charge > 50)
        {
            SetBehavior(DroneBehavior.Passive);
        }
        else if (Charge == 0)
        {
            SetBehavior(DroneBehavior.Defensive);
        }
    }

    void SetBehavior(DroneBehavior newBehavior)
    {
        if (CurrentBehavior != newBehavior)
        {
            CurrentBehavior = newBehavior;
            if (newBehavior == DroneBehavior.Aggressive)
            {
                SetAggressivePosition();
            }
        }
    }

    void SetAggressivePosition()
    {
        float x = Random.Range(-maxDistance, maxDistance);
        float y = Random.Range(0, 2f);
        aggressivePosition = ParentShip.transform.position + new Vector3(x, y, 0);
    }
    
void MoveDrone()
{
    if (curveProgress >= 1f)
    {
        currentLocalTarget = nextLocalTarget;
        nextLocalTarget = PickNewLocalTarget();
        curveProgress = 0f;
    }

    curveProgress += Time.deltaTime / curveDuration;
    Vector3 localPosition = CalculateBezierPoint(curveProgress, currentLocalTarget, Vector3.zero, nextLocalTarget);
    
    Vector3 worldPosition = ParentDroneAnchor.transform.TransformPoint(localPosition);
    transform.position = Vector3.Lerp(transform.position, worldPosition, Time.deltaTime * 5f * MovementSpeedModifier);
}


private Vector3 PickNewLocalTarget()
{
    float x = isLeftSide ? Random.Range(-maxDistance, -maxDistance * 1.5f) : Random.Range(maxDistance, maxDistance * 1.5f);
    float y = GetYPositionForBehavior();
    
    isLeftSide = !isLeftSide; // Switch sides for next time
    
    return new Vector3(x, y, 0);
}

    private float GetYPositionForBehavior()
    {
        switch (CurrentBehavior)
        {
            case DroneBehavior.Defensive:
                return Random.Range(-2f, -1f);
            case DroneBehavior.Passive:
                return Random.Range(-1f, 1f);
            case DroneBehavior.Aggressive:
                return Random.Range(0f, 2f);
            default:
                return 0f;
        }
    }

    private Vector3 CalculateBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
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
