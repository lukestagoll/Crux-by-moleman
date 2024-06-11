using UnityEngine;

public class PlasmaLight : ProjectileBase
{
    protected override void InitializeBehaviour(float speedModifier, Vector2 initialVelocity, WeaponBase.RelativeSide side)
    {
        Rigidbody2D rb = GetComponent<Rigidbody2D>();

        // Set the velocity of the projectile
        rb.velocity = initialVelocity + (Vector2)(transform.up * BaseSpeed * speedModifier);
    }
}
