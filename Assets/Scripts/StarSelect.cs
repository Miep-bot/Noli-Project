using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;

public class StarGrow : MonoBehaviour
{
    private Vector3 originalScale;
    public float growFactor = 1.2f;
    public float growSpeed = 5f;
    private Renderer starRenderer;
    private Color originalColor;
    public Color hoverColor = Color.yellow;
    private StarConnector starConnector;

    private void Start()
    {
        originalScale = transform.localScale;
        starRenderer = GetComponent<Renderer>();
        originalColor = starRenderer.material.color;
        starConnector = FindObjectOfType<StarConnector>(); // Find the connector script
    }

    public void OnHoverEnter(HoverEnterEventArgs args)
    {
        StopAllCoroutines();
        StartCoroutine(GrowStar());
        ChangeColor(hoverColor);
        starConnector?.OnHoverEnter(transform);
    }

    public void OnHoverExit(HoverExitEventArgs args)
    {
        StopAllCoroutines();
        StartCoroutine(ShrinkStar());
        ChangeColor(originalColor);
    }

    private System.Collections.IEnumerator GrowStar()
    {
        Vector3 targetScale = originalScale * growFactor;
        while (Vector3.Distance(transform.localScale, targetScale) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, targetScale, Time.deltaTime * growSpeed);
            yield return null;
        }
    }

    private System.Collections.IEnumerator ShrinkStar()
    {
        while (Vector3.Distance(transform.localScale, originalScale) > 0.01f)
        {
            transform.localScale = Vector3.Lerp(transform.localScale, originalScale, Time.deltaTime * growSpeed);
            yield return null;
        }
    }

    private void ChangeColor(Color newColor)
    {
        if (starRenderer != null)
        {
            starRenderer.material.color = newColor;
        }
    }
}
