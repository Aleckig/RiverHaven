using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Cable : MonoBehaviour
{   
    public bool isLeftCable { get; private set; }
    
    [SerializeField] private SpriteRenderer cableSprite;
    [SerializeField] private LineRenderer lineRenderer;
    [SerializeField] private BoxCollider2D boxCollider;
    
    private Camera mainCamera;
    private bool isDragging = false;
    private Vector3 startPosition;
    private Color cableColor;

    public void Initialize(bool isLeft, Camera camera)
    {
        isLeftCable = isLeft;
        mainCamera = camera;
        
        if (lineRenderer != null)
        {
            lineRenderer.positionCount = 2;
            lineRenderer.useWorldSpace = true;
            lineRenderer.startWidth = 0.1f;
            lineRenderer.endWidth = 0.1f;
            lineRenderer.enabled = false;
        }

        // Ensure the collider is properly sized to the sprite
        if (boxCollider != null && cableSprite != null)
        {
            boxCollider.size = cableSprite.bounds.size;
        }
    }

    private void Update()
    {
        if (isDragging && isLeftCable)
        {
            Vector3 mousePosition = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            mousePosition.z = 0;
            
            if (lineRenderer != null)
            {
                lineRenderer.enabled = true;
                lineRenderer.SetPosition(0, startPosition);
                lineRenderer.SetPosition(1, mousePosition);
            }

            // Check for mouse button release
            if (Input.GetMouseButtonUp(0))
            {
                EndDrag();
            }
        }
    }

    private void OnMouseDown()
    {
        if (isLeftCable)
        {
            isDragging = true;
            startPosition = transform.position;
            if (lineRenderer != null)
            {
                lineRenderer.enabled = true;
            }
        }
    }

    private void EndDrag()
    {
        isDragging = false;
        if (lineRenderer != null)
        {
            lineRenderer.enabled = false;
        }

        // Check for connection with right cable
        Vector2 mousePos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos, Vector2.zero);

        foreach (RaycastHit2D hit in hits)
        {
            Cable otherCable = hit.collider.GetComponent<Cable>();
            if (otherCable != null && !otherCable.isLeftCable)
            {
                if (otherCable.cableColor == this.cableColor)
                {
                    Debug.Log("Correct connection!");
                    // Add your connection success logic here
                }
            }
        }
    }

    public void SetColor(Color color)
    {
        cableColor = color;
        if (cableSprite != null)
        {
            cableSprite.color = color;
        }
        if (lineRenderer != null)
        {
            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }
    }
}
