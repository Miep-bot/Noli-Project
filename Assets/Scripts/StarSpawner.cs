using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class StarSpawner : MonoBehaviour
{
    public GameObject starPrefab; // Assign in Inspector
    public InputActionReference spawnAction; // Assign in Inspector (controller button)

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
        Vector3 pos1 = transform.position + new Vector3(0, 0, 0);
        Vector3 pos2 = transform.position + new Vector3(-0.5f, 0, 0.5f);
        Vector3 pos3 = transform.position + new Vector3(0.5f, 0, 0.5f);

        spawnedStars.Add(Instantiate(starPrefab, pos1, Quaternion.identity));
        spawnedStars.Add(Instantiate(starPrefab, pos2, Quaternion.identity));
        spawnedStars.Add(Instantiate(starPrefab, pos3, Quaternion.identity));

        starsSpawned = true;
    }

    private void RemoveStars()
    {
        foreach (GameObject star in spawnedStars)
        {
            Destroy(star);
        }
        spawnedStars.Clear();
        starsSpawned = false;
    }
}