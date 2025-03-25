using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;        // How fast the enemy moves up and down and towards the player
    public float moveDistance = 1f;     // How far the enemy moves up and down
    private Vector3 startPosition;      // Starting position of the enemy
    private float timeElapsed;          // Time tracking for sine wave movement
    public bool CanMove = true;         // Determines if the enemy can move at all
    public bool CanFly = true;          // Whether or not the enemy can move up and down (fly)
    private Rigidbody rb;               // Reference to the enemy's Rigidbody
    private Transform player;           // Reference to the player's transform

    private void Start()
    {
        // Save the starting position of the enemy
        startPosition = transform.position;
        rb = GetComponent<Rigidbody>();  // Get the Rigidbody component
        player = GameObject.FindWithTag("Player").transform; // Find the player using the "Player" tag

        // Lock the Y-axis rotation of the enemy (if needed)
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void FixedUpdate()
    {
        if (CanMove)
        {
            // Always move towards the player's position
            Vector3 directionToPlayer = (player.position - transform.position).normalized;

            // If the enemy can fly, apply the up and down movement
            if (CanFly)
            {
                timeElapsed += Time.deltaTime * moveSpeed;
                float offsetY = Mathf.Cos(timeElapsed) * moveDistance;

                // Create a movement vector that combines horizontal and vertical movement
                Vector3 targetPosition = transform.position + directionToPlayer * moveSpeed * Time.fixedDeltaTime;
                targetPosition.y = startPosition.y + offsetY;  // Maintain vertical sine wave movement

                // Apply the movement to the Rigidbody using MovePosition
                rb.MovePosition(targetPosition);
            }
            else
            {
                // If CanFly is false, don't apply any vertical movement
                Vector3 targetPosition = transform.position + directionToPlayer * moveSpeed * Time.fixedDeltaTime;
                rb.MovePosition(targetPosition);
            }

            // Optional: Keep rotation locked on Y-axis
            Vector3 fixedRotation = transform.rotation.eulerAngles;
            fixedRotation.x = 0f;  // Lock rotation on the X-axis
            fixedRotation.z = 0f;  // Lock rotation on the Z-axis
            transform.rotation = Quaternion.Euler(fixedRotation);  // Apply the fixed rotation
        }
    }

    // Optional: Handle enemy hit or collision logic, such as being pushed back after a collision with the player
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Apply a small knockback to the enemy when it collides with the player
            Vector3 knockbackDirection = transform.position - collision.transform.position;
            rb.AddForce(knockbackDirection.normalized * 5f, ForceMode.Impulse); // Adjust force as needed
        }

        // Handle other collision events, such as collisions with obstacles or other enemies
        if (collision.gameObject.CompareTag("Enemy"))
        {
            // If colliding with another enemy, ignore the collision using Rigidbody physics
            Physics.IgnoreCollision(collision.collider, rb.GetComponent<Collider>());
        }
    }

    // Optional: You could create a method that applies a movement force manually if you need physics-based movement for certain situations
    private void ApplyForce(Vector3 force)
    {
        rb.AddForce(force);
    }
}
