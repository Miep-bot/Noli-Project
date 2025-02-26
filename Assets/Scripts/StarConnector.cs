using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class StarConnector : MonoBehaviour
{
    public InputActionReference connectAction; // Button to hold for connecting
    public InputActionReference throwAction;   // Button for throwing the fireball
    public GameObject objectToSpawn;           // The object to spawn (fireball)
    public Material lineMaterial;              // Line material
    public float lineWidth = 0.02f;
    public StarSpawner starSpawner;            // Reference to the StarSpawner script
    public UnityEngine.XR.Interaction.Toolkit.Interactors.XRRayInteractor rightRayInteractor; // Right VR controller reference
    private GameObject spawnedFireball;        // Fireball that will spawn
    private LineRenderer currentLine;
    private List<Vector3> linePoints = new List<Vector3>();
    private List<GameObject> allLines = new List<GameObject>();
    private bool isConnecting = false;
    private Transform lastStar;
    private int connectedStarsCount = 0;

    // Variables for throwing mechanics
    private Vector3 lastControllerPosition;
    private Vector3 controllerVelocity;
    private bool isThrowing = false;
    private bool isFireballThrown = false; // Flag to check if the fireball is thrown

    private void OnEnable()
    {
        connectAction.action.performed += StartConnecting;
        connectAction.action.canceled += StopConnecting;
        throwAction.action.started += StartThrowing; // Start tracking the controller for throwing
        throwAction.action.canceled += StopThrowing; // When button is released, throw the fireball
    }

    private void OnDisable()
    {
        connectAction.action.performed -= StartConnecting;
        connectAction.action.canceled -= StopConnecting;
        throwAction.action.started -= StartThrowing;
        throwAction.action.canceled -= StopThrowing;
    }

    private void StartConnecting(InputAction.CallbackContext context)
    {
        isConnecting = true;
        linePoints.Clear();
        connectedStarsCount = 0;
        lastStar = null;
        CreateNewLine();
    }

    private void StopConnecting(InputAction.CallbackContext context)
    {
        isConnecting = false;
        lastStar = null;
        ClearAllLines();
    }

    private void StartThrowing(InputAction.CallbackContext context)
    {
        // Start tracking the controller movement for throwing
        if (spawnedFireball != null) // Only track if the fireball is spawned
        {
            isThrowing = true;
            lastControllerPosition = rightRayInteractor.transform.position;
            Debug.Log("Throwing started"); // Debugging line
        }
    }

    private void StopThrowing(InputAction.CallbackContext context)
    {
        if (isThrowing && spawnedFireball != null)
        {
            ThrowFireball();
        }
        isThrowing = false;
        Debug.Log("Throwing stopped"); // Debugging line
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
            if (linePoints.Contains(starTransform.position))
            {
                return;
            }

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
        if (starSpawner != null)
        {
            Vector3 pos1 = starSpawner.GetStarPosition(0);
            Vector3 pos2 = starSpawner.GetStarPosition(1);
            Vector3 pos3 = starSpawner.GetStarPosition(2);

            return (linePoints.Count == 3 &&
                    linePoints[0] == pos1 &&
                    linePoints[1] == pos2 &&
                    linePoints[2] == pos3);
        }
        return false;
    }

    private void SpawnObjectNextToController()
    {
        if (rightRayInteractor != null)
        {
            Transform controllerTransform = rightRayInteractor.transform;
            Vector3 spawnPosition = controllerTransform.position + controllerTransform.forward * 0.2f;

            if (spawnedFireball != null)
            {
                Destroy(spawnedFireball); // Destroy any previous fireball
            }

            spawnedFireball = Instantiate(objectToSpawn, spawnPosition, controllerTransform.rotation);
            isFireballThrown = false; // Reset flag so new fireball follows the controller

            if (spawnedFireball != null)
            {
                Rigidbody fireballRigidbody = spawnedFireball.GetComponent<Rigidbody>();
                if (fireballRigidbody != null)
                {
                    fireballRigidbody.isKinematic = true;
                    fireballRigidbody.useGravity = false;
                }

                AttachFireballToController();

                if (starSpawner != null)
                {
                    starSpawner.RemoveStars();
                }
                ClearAllLines();
                Debug.Log("Fireball spawned successfully!");
            }
        }
    }

    private void AttachFireballToController()
    {
        if (spawnedFireball != null && rightRayInteractor != null)
        {
            // Attach the fireball to the controller (keep it close to the controller's position)
            spawnedFireball.transform.position = rightRayInteractor.transform.position + rightRayInteractor.transform.forward * 0.2f;
            spawnedFireball.transform.rotation = rightRayInteractor.transform.rotation;
        }
    }

    private void ThrowFireball()
    {
        if (spawnedFireball != null)
        {
            isFireballThrown = true; // Stop the fireball from following the controller
            Rigidbody fireballRigidbody = spawnedFireball.GetComponent<Rigidbody>();

            if (fireballRigidbody != null)
            {
                fireballRigidbody.isKinematic = false;
                fireballRigidbody.useGravity = true;

                Vector3 throwDirection = smoothedVelocity.normalized;
                float throwSpeed = smoothedVelocity.magnitude;

                float speedMultiplier = 10f; // Adjust this if needed

                fireballRigidbody.AddForce(throwDirection * throwSpeed * speedMultiplier, ForceMode.VelocityChange);
                Debug.Log("Fireball thrown with velocity: " + fireballRigidbody.velocity);
            }
            else
            {
                Debug.LogError("Fireball does not have a Rigidbody component!");
            }
        }
    }

    private Vector3 previousControllerPosition;
    private Vector3 smoothedVelocity;

    private void Update()
    {
        if (rightRayInteractor != null)
        {
            Vector3 currentControllerPosition = rightRayInteractor.transform.position;
            Vector3 rawVelocity = (currentControllerPosition - previousControllerPosition) / Time.deltaTime;
            smoothedVelocity = Vector3.Lerp(smoothedVelocity, rawVelocity, 0.1f); // Smooth out the velocity
            previousControllerPosition = currentControllerPosition;
        }

        if (spawnedFireball != null && !isFireballThrown)
        {
            Transform controllerTransform = rightRayInteractor.transform;
            spawnedFireball.transform.position = controllerTransform.position + controllerTransform.forward * 0.2f;
            spawnedFireball.transform.rotation = controllerTransform.rotation;
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
