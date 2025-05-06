using UnityEngine;

public class PlayerTeleportTracker : MonoBehaviour
{
    public float teleportThreshold = 1.5f; // Minimum distance to count as a teleport
    private Vector3 lastPosition;
    public bool justTeleported { get; private set; }

    private void Start()
    {
        lastPosition = transform.position;
    }

    private void Update()
    {
        float distanceMoved = Vector3.Distance(transform.position, lastPosition);
        justTeleported = distanceMoved > teleportThreshold;

        if (justTeleported)
        {
            Debug.Log("Player teleported");
        }

        lastPosition = transform.position;
    }
}
