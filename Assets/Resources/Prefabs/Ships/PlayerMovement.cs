using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float verticalMoveSpeed = 5f; // Vertical movement speed of the player's ship.
    public float horizontalMoveSpeed = 7.5f; // Horizontal movement speed of the player's ship.
    public float accelerationSmoothing = 8f; // Smoothing factor for acceleration.
    
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 currentVelocity; // Current velocity, used for smoothing acceleration.

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    void Update()
    {
        // Input handling for movement direction based on WASD or arrow keys.
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate()
    {
        if (PlayerManager.Inst.ActivePlayerShip == null) return;
        // Target velocity based on input, with separate speeds for horizontal and vertical movement.
        float movementSpeedModifier = PlayerManager.Inst.ActivePlayerShip.MovementSpeedModifier;
        Vector2 targetVelocity = new Vector2(movement.x * horizontalMoveSpeed * movementSpeedModifier, movement.y * verticalMoveSpeed * movementSpeedModifier);
        
        // Smoothly interpolate from the current velocity to the target velocity to achieve acceleration smoothing.
        currentVelocity = Vector2.Lerp(currentVelocity, targetVelocity, accelerationSmoothing * Time.fixedDeltaTime);
        
        // Apply the smoothed velocity to move the player's ship.
        rb.MovePosition(rb.position + currentVelocity * Time.fixedDeltaTime);
    }
}
