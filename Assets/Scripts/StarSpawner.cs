using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class StarSpawner : MonoBehaviour
{
    public GameObject starPrefab; // Assign in Inspector
    public InputActionReference spawnAction; // Assign in Inspector (controller button)
    public Transform cameraTransform; // Assign the XR Rig Camera (Player's Headset) in Inspector
    public float spawnDistance = 10f; // Distance in front of the player
    public float triangleSize = 0.5f; // Adjust to control triangle size

    private bool starsSpawned = false;
    private List<GameObject> spawnedStars = new List<GameObject>();

    private void OnEnable()
    {
        spawnAction.action.performed += ToggleStars;
    }

    private void OnDisable()
    {
        spawnAction.action.performed -= ToggleStars;
    }

    private void ToggleStars(InputAction.CallbackContext context)
    {
        if (starsSpawned)
        {
            RemoveStars();
        }
        else
        {
            SpawnStars();
        }
    }

    private void SpawnStars()
    {
        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform not assigned!");
            return;
        }

        // Get the forward, right, and down directions relative to the player's view
        Vector3 forward = cameraTransform.forward;
        Vector3 right = cameraTransform.right;
        Vector3 down = -cameraTransform.up; // Downward direction

        // First star (Anchor Point) - Spawns in front of the player
        Vector3 pos1 = cameraTransform.position + forward * spawnDistance;
        GameObject star1 = Instantiate(starPrefab, pos1, Quaternion.identity);
        spawnedStars.Add(star1);

        // Position the other two stars RELATIVE to the first one, but aligned with the player's perspective
        Vector3 pos2 = pos1 + (-right * triangleSize + down * triangleSize); // Left-bottom
        Vector3 pos3 = pos1 + (right * triangleSize + down * triangleSize);  // Right-bottom

        GameObject star2 = Instantiate(starPrefab, pos2, Quaternion.identity);
        GameObject star3 = Instantiate(starPrefab, pos3, Quaternion.identity);

        spawnedStars.Add(star2);
        spawnedStars.Add(star3);

        starsSpawned = true;
    }

    public void RemoveStars()
    {
        foreach (GameObject star in spawnedStars)
        {
            Destroy(star);
        }
        spawnedStars.Clear();
        starsSpawned = false;
    }

    public Vector3 GetStarPosition(int index)
    {
        if (index >= 0 && index < spawnedStars.Count)
        {
            return spawnedStars[index].transform.position;
        }
        return Vector3.zero;
    }
}
