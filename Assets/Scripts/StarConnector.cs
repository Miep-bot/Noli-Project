using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit;

public class StarConnector : MonoBehaviour
{
    public InputActionReference connectAction; // Assign in Inspector (button for connecting)
    private LineRenderer currentLine;
    private List<Vector3> linePoints = new List<Vector3>();
    private List<GameObject> allLines = new List<GameObject>(); // Store all lines
    private bool isConnecting = false;
    private Transform lastStar; // The last star hovered over

    public Material lineMaterial; // Assign in Inspector (Line Material)
    public float lineWidth = 0.02f;

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
        lastStar = null; // Reset last hovered star
        CreateNewLine();
    }

    private void StopConnecting(InputAction.CallbackContext context)
    {
        isConnecting = false;
        lastStar = null;
        ClearAllLines(); // Remove all lines
    }

    private void CreateNewLine()
    {
        GameObject lineObj = new GameObject("ConnectionLine");
        currentLine = lineObj.AddComponent<LineRenderer>();
        currentLine.material = lineMaterial;
        currentLine.startWidth = lineWidth;
        currentLine.endWidth = lineWidth;
        currentLine.positionCount = 0;

        allLines.Add(lineObj); // Store the line for clearing later
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
        }
    }

    private void UpdateLine()
    {
        currentLine.positionCount = linePoints.Count;
        currentLine.SetPositions(linePoints.ToArray());
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
