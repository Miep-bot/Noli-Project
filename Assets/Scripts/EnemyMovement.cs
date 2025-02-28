using System.Collections;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 2f;      // How fast the enemy moves up and down
    public float moveDistance = 1f;   // How far the enemy moves up and down
    private Vector3 startPosition;    // Starting position of the enemy
    private float timeElapsed;        // Time tracking for sine wave movement
    public bool CanMove;

    private void Start()
    {
        // Save the starting position of the enemy
        startPosition = transform.position;
    }

    private void Update()
    {
        if (CanMove)
        {
            // Move the enemy up and down using a sine wave function
            timeElapsed += Time.deltaTime * moveSpeed;
            float offsetY = Mathf.Cos(timeElapsed) * moveDistance;

            // Update the position based on sine wave for smooth up and down movement
            transform.position = new Vector3(startPosition.x, startPosition.y + offsetY, startPosition.z);
        }
        else
        {
            transform.position = startPosition;
        }
    }
}
