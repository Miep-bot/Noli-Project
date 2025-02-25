using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;


public class StarConnector : MonoBehaviour
{
    public InputActionReference connectAction; // Button to hold for connecting
    public GameObject objectToSpawn; // The object to spawn (fireball)
    public Material lineMaterial; // Assign in Inspector (Line Material)
    public float lineWidth = 0.02f;
    public StarSpawner starSpawner; // Reference to the StarSpawner script
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rightRayInteractor; // Reference to the right VR controller(assign in Inspector)
    private GameObject spawnedFireball; // The fireball that will spawn

    private LineRenderer currentLine;
    private List<Vector3> linePoints = new List<Vector3>();
    private List<GameObject> allLines = new List<GameObject>(); // Store all lines
    private bool isConnecting = false;
    private Transform lastStar; // The last star hovered over
    private int connectedStarsCount = 0; // Track how many stars are connected

    private void OnEnable()
    {
        connectAction.action.performed += StartConnecting;
        connectAction.action.canceled += StopConnecting;
    }

    private void OnDisable()
    {
        connectAction.action.performed -= StartConnecting;
        connectAction.action.canceled -= StopConnecting;
    }

    private void StartConnecting(InputAction.CallbackContext context)
    {
        isConnecting = true;
        linePoints.Clear();
        connectedStarsCount = 0; // Reset connected stars count
        lastStar = null;
        CreateNewLine();
    }

    private void StopConnecting(InputAction.CallbackContext context)
    {
        isConnecting = false;
        lastStar = null;
        ClearAllLines();
    }

    private void CreateNewLine()
    {
        GameObject lineObj = new GameObject("ConnectionLine");
        currentLine = lineObj.AddComponent<LineRenderer>();
        currentLine.material = lineMaterial;
        currentLine.startWidth = lineWidth;
        currentLine.endWidth = lineWidth;
        currentLine.positionCount = 0;

        allLines.Add(lineObj);
    }

    public void OnHoverEnter(Transform starTransform)
    {
        if (isConnecting)
        {
            if (lastStar != null && lastStar != starTransform)
            {
                linePoints.Add(starTransform.position);
                UpdateLine();
            }
            else
            {
                linePoints.Add(starTransform.position); // First star
            }

            lastStar = starTransform;
            connectedStarsCount++;

            // Check if all stars are connected in the correct order
            if (connectedStarsCount == 3 && IsCorrectOrder())
            {
                SpawnObjectNextToController();
            }
        }
    }

    private void UpdateLine()
    {
        currentLine.positionCount = linePoints.Count;
        currentLine.SetPositions(linePoints.ToArray());
    }

    private bool IsCorrectOrder()
    {
        // Get the positions of the stars from the StarSpawner
        if (starSpawner != null)
        {
            Vector3 pos1 = starSpawner.GetStarPosition(0); // Get pos1 from StarSpawner
            Vector3 pos2 = starSpawner.GetStarPosition(1); // Get pos2 from StarSpawner
            Vector3 pos3 = starSpawner.GetStarPosition(2); // Get pos3 from StarSpawner

            // Verify the order of positions matches the sequence: pos1, pos2, pos3
            return (linePoints.Count == 3 &&
                    linePoints[0] == pos1 &&
                    linePoints[1] == pos2 &&
                    linePoints[2] == pos3);
        }
        return false;
    }

    private void SpawnObjectNextToController()
    {
        // Ensure the rightRayInteractor is assigned
        if (rightRayInteractor != null)
        {
            // Get the position of the right controller using the ray interactor's transform
            Vector3 controllerPosition = rightRayInteractor.transform.position;
            Vector3 spawnOffset = new Vector3(0.5f, 0, 0); // Adjust this offset as needed
            Vector3 spawnPosition = controllerPosition + spawnOffset;

            // Instantiate the fireball (object) at the spawn position
            spawnedFireball = Instantiate(objectToSpawn, spawnPosition, Quaternion.identity);
        }
        else
        {
            Debug.LogError("Right Ray Interactor is not assigned!");
        }
    }

    private void Update()
    {
        // If the fireball has spawned, make it follow the right controller
        if (spawnedFireball != null && rightRayInteractor != null)
        {
            // Get the position of the right controller using the ray interactor's transform
            Vector3 controllerPosition = rightRayInteractor.transform.position;
            Vector3 spawnOffset = new Vector3(0.5f, 0, 0); // Adjust this offset as needed
            Vector3 spawnPosition = controllerPosition + spawnOffset;

            // Update the fireball's position to follow the controller
            spawnedFireball.transform.position = spawnPosition;
        }
    }

    private void ClearAllLines()
    {
        foreach (GameObject line in allLines)
        {
            Destroy(line);
        }
        allLines.Clear();
    }
}
