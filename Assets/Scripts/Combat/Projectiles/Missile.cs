using UnityEngine;
using System.Collections;

public class Missile : ProjectileBase
{
    public float sideDuration = 0.5f;
    public float sideEaseDuration = 0.5f;
    public float forwardDuration = 2f;
    public float BaseSideSpeed = 1f;

    protected override void InitializeBehaviour(float speedModifier, Vector2 initialVelocity, WeaponBase.RelativeSide side)
    {
        StartCoroutine(MoveMissile(initialVelocity, speedModifier, side));
    }

    private IEnumerator MoveMissile(Vector2 initialVelocity, float speedModifier, WeaponBase.RelativeSide side)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Determine the direction based on the side
        Vector2 sideDirection;
        if (side == WeaponBase.RelativeSide.Left)
        {
            sideDirection = Quaternion.Euler(0, 0, 15) * Vector2.left; // 15 degrees off to the left
        }
        else if (side == WeaponBase.RelativeSide.Right)
        {
            sideDirection = Quaternion.Euler(0, 0, -15) * Vector2.right; // 15 degrees off to the right
        }
        else
        {
            sideDirection = Vector2.zero; // No side movement for center
        }

        // Step 1: Slowly move to the side and a bit behind for half a second
        float elapsedTime = 0f;
        while (elapsedTime < sideDuration)
        {
            rb.velocity = initialVelocity + sideDirection * BaseSideSpeed * speedModifier * (elapsedTime / sideDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Step 2 and Step 3: Reduce side momentum and accelerate forwards simultaneously
        elapsedTime = 0f;
        while (elapsedTime < sideEaseDuration)
        {
            float sideMomentum = BaseSideSpeed * speedModifier * (1 - (elapsedTime / sideEaseDuration));
            float forwardMomentum = BaseSpeed * speedModifier * Mathf.Min(1, elapsedTime / sideEaseDuration); // Accelerate faster
            rb.velocity = initialVelocity + sideDirection * sideMomentum + (Vector2)(transform.up * forwardMomentum);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the missile continues moving forward at full speed after acceleration
        rb.velocity = initialVelocity + (Vector2)(transform.up * BaseSpeed * speedModifier);
    }
}
