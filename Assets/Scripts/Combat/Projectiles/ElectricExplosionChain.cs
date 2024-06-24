using UnityEngine;

public class ElectricExplosionChain : MonoBehaviour
{
    private ParticleSystem particleSystem;
    private GameObject ElectricExplosionChainPrefab;
    public float Charge;

    void Awake()
    {
        // Fetch the ElectricExplosion prefab from the AssetManager
        ElectricExplosionChainPrefab = AssetManager.GetProjectilePrefab("ElectricExplosionChain");
        if (ElectricExplosionChainPrefab == null)
        {
            Debug.LogError("Failed to load ElectricExplosion prefab!");
        }
        // Fetch the ParticleSystem component
        particleSystem = GetComponent<ParticleSystem>();

        //! This has been covered by the inspector
        // Check if the ParticleSystem component is found
        if (particleSystem != null)
        {
            // Enable collision module
            var collision = particleSystem.collision;
            collision.enabled = true;
            collision.type = ParticleSystemCollisionType.World;
            collision.mode = ParticleSystemCollisionMode.Collision2D;
            collision.sendCollisionMessages = true;

            // Play the ParticleSystem
            particleSystem.Play();

            // Register to the particle system's stopped event
            var main = particleSystem.main;
            main.stopAction = ParticleSystemStopAction.Callback;
        }
        else
        {
            Debug.LogError("ParticleSystem component not found on the ElectricExplosion prefab.");
        }
    }

    void OnParticleCollision(GameObject other)
    {
        Debug.Log("CHAIN PARTICLE COLLISION WITH: " + other.name + other.tag);
        if (other.CompareTag("Enemy"))
        {
            ShipBase ship = other.GetComponent<ShipBase>();
            if (ship != null)
            {
              //! Put into own fn
              if (Charge > 50) {
                GameObject electricExplosionChain = Instantiate(ElectricExplosionChainPrefab, ship.transform.position, Quaternion.identity, ship.transform);
                ElectricExplosionChain explosionChainScript = electricExplosionChain.GetComponent<ElectricExplosionChain>();
                explosionChainScript.Charge = Charge / 1.25f;
              }
              ship.TakeDamage(Charge);
            }
        }
    }

    void OnParticleSystemStopped()
    {
        Destroy(gameObject);
    }
}
