using UnityEngine;
using UnityEngine.InputSystem;

public class StarSpawner : MonoBehaviour
{
    public GameObject starPrefab; // Assign in Inspector
    public InputActionReference spawnAction; // Assign in Inspector (controller button)

    private bool hasSpawned = false; // Prevent multiple spawns

    private void OnEnable()
    {
        spawnAction.action.performed += SpawnStars;
    }

    private void OnDisable()
    {
        spawnAction.action.performed -= SpawnStars;
    }

    private void SpawnStars(InputAction.CallbackContext context)
    {
        if (hasSpawned) return; // Prevent duplicate spawns

        // Define triangle positions relative to the spawner
        Vector3 pos1 = transform.position + new Vector3(0, 0, 0);
        Vector3 pos2 = transform.position + new Vector3(-0.5f, 0, 0.5f);
        Vector3 pos3 = transform.position + new Vector3(0.5f, 0, 0.5f);

        // Spawn 3 stars
        Instantiate(starPrefab, pos1, Quaternion.identity);
        Instantiate(starPrefab, pos2, Quaternion.identity);
        Instantiate(starPrefab, pos3, Quaternion.identity);

        hasSpawned = true; // Prevent multiple spawns
    }
}
