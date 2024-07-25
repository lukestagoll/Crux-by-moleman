using UnityEngine;

public class EnemyShip : ShipBase
{
    [SerializeField] protected int pointsOnKill;
    public int damageOnCollision = 10;

    public bool TargetedByTurret;
    
    public delegate void EnemyShipEvent(EnemyShip ship);
    public event EnemyShipEvent OnDestroyed;

    private EffectData AssignedEffectData;

    void Start()
    {
        isEnemy = true;
    }

    public void AssignItemDrop(EffectData effectData)
    {
        if (effectData != null)
        {
            AssignedEffectData = effectData;
        }
    }

    public override void Die()
    {
        GameManager.IncrementScore(pointsOnKill);
        Explode();
        if (AssignedEffectData != null)
        {
            // Instantiate the ItemDropPrefab at the enemy ship's position
            var itemDrop = Instantiate(AssetManager.ItemDropPrefab, transform.position, Quaternion.identity);
        
            // Initialize the item drop with the assigned effect data
            itemDrop.InitialiseItem(AssignedEffectData);
        }
    }

    void OnDestroy()
    {
        OnDestroyed?.Invoke(this);
    }

    protected override void SubtractHealth(float damage)
    {
        Health -= damage;
        if (Health <= 0)
        {
            isDestroyed = true;
            Die();
        }
    }

    protected override void AddShield(float amt)
    {
        Shield += amt;
    }

    protected override float SubtractShield(float damage)
    {
        if (Shield <= 0) return damage;

        Shield -= damage;

        if (Shield < 0) return damage + Shield;
        else return 0;
    }

    public override void AddHealth(float amt)
    {
        Health += amt;
    }
}
