using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.UI;

public class StarSpawner : MonoBehaviour
{
    public GameObject starPrefab; // Assign in Inspector
    public InputActionReference spawnAction; // Assign in Inspector (controller button)
    public Transform cameraTransform; // Assign the XR Rig Camera (Player's Headset) in Inspector
    public float spawnDistance = 10f; // Distance in front of the player
    public float triangleSize = 0.5f; // Adjust to control triangle size
    public Image transparentImage;

    private bool starsSpawned = false;
    private List<GameObject> spawnedStars = new List<GameObject>();

    public Color firstStarColor = Color.blue;

    public void ShowTransparentImage()
    {
        transparentImage.gameObject.SetActive(true);
    }

    // Call this when you want to hide the transparent image
    public void HideTransparentImage()
    {
        transparentImage.gameObject.SetActive(false);
    }

    private void OnEnable()
    {
        HideTransparentImage();
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
        Time.timeScale = 0f;
        ShowTransparentImage();

        if (cameraTransform == null)
        {
            Debug.LogError("Camera Transform not assigned!");
            return;
        }

        // Get directions based on camera orientation
        Vector3 forward = cameraTransform.forward;  // Player's forward direction
        Vector3 right = cameraTransform.right;      // Player's right direction
        Vector3 down = -cameraTransform.up;         // Downward direction

        float shapeSpacing = 0.4f; // Distance between the two shapes
        float shapeSize = 0.3f; // Size of the shapes

        // Find the central position where both shapes will spawn
        Vector3 centerPos = cameraTransform.position + forward * spawnDistance;

        // FIREBALL TRIANGLE (Left Side)
        Vector3 triangleCenter = centerPos - right * shapeSpacing;

        // Star positions for the triangle
        Vector3 triPos1 = triangleCenter + ((-down / 2) * shapeSize * 0.5f);
        Vector3 triPos2 = triangleCenter + (-right * shapeSize * 0.5f + (down / 2) * shapeSize); // Bottom-left
        Vector3 triPos3 = triangleCenter + (right * shapeSize * 0.5f + (down / 2) * shapeSize);  // Bottom-right

        GameObject star1 = Instantiate(starPrefab, triPos1, Quaternion.identity);
        star1.GetComponent<Renderer>().material.color = firstStarColor;

        GameObject star2 = Instantiate(starPrefab, triPos2, Quaternion.identity);
        GameObject star3 = Instantiate(starPrefab, triPos3, Quaternion.identity);

        spawnedStars.Add(star1);
        spawnedStars.Add(star2);
        spawnedStars.Add(star3);

        // ICE CUBE SQUARE (Right Side)
        Vector3 squareCenter = centerPos + right * shapeSpacing;

        // Star positions for the square
        Vector3 sqPos1 = squareCenter + (-right * shapeSize * 0.5f + -down * shapeSize * 0.5f); // Top-left
        Vector3 sqPos2 = squareCenter + (right * shapeSize * 0.5f + -down * shapeSize * 0.5f);  // Top-right
        Vector3 sqPos3 = squareCenter + (-right * shapeSize * 0.5f + down * shapeSize * 0.5f); // Bottom-left
        Vector3 sqPos4 = squareCenter + (right * shapeSize * 0.5f + down * shapeSize * 0.5f); // Bottom-right

        GameObject star4 = Instantiate(starPrefab, sqPos1, Quaternion.identity);
        star4.GetComponent<Renderer>().material.color = firstStarColor;

        GameObject star5 = Instantiate(starPrefab, sqPos2, Quaternion.identity);
        GameObject star6 = Instantiate(starPrefab, sqPos3, Quaternion.identity);
        GameObject star7 = Instantiate(starPrefab, sqPos4, Quaternion.identity);

        spawnedStars.Add(star4);
        spawnedStars.Add(star5);
        spawnedStars.Add(star6);
        spawnedStars.Add(star7);

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
        Time.timeScale = 1f;
        HideTransparentImage();
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
