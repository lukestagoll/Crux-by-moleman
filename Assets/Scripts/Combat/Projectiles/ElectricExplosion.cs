using UnityEngine;
using System;

public class ElectricExplosion : MonoBehaviour
{
    private ParticleSystem particleSystem;

    public event Action OnExplosionFinished;

    void Awake()
    {
        // Fetch the ParticleSystem component
        particleSystem = GetComponent<ParticleSystem>();

        // Check if the ParticleSystem component is found
        if (particleSystem != null)
        {
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

    void OnParticleSystemStopped()
    {
        OnExplosionFinished?.Invoke();
        Destroy(gameObject);
    }
}
