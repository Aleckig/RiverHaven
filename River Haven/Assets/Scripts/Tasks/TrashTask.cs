using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrashTask : MonoBehaviour
{
    [Header("UI Elements")]
    public UnityEngine.UI.Button collectTrashButton;
    [SerializeField] private Transform trashContainer;

    [Header("Settings")]
    [SerializeField] private LayerMask trashLayer;
    [SerializeField] private float clickRadius = 0.5f;

    private List<GameObject> remainingTrash;
    private bool isActive = true;

    private void Start()
    {
        remainingTrash = new List<GameObject>();
        
        // Initialize trash list
        foreach (Transform trash in trashContainer)
        {
            remainingTrash.Add(trash.gameObject);
        }

        // Set up collect button
        collectTrashButton.onClick.AddListener(OnCollectTrashButtonClicked);
        collectTrashButton.interactable = false;
    }

    private void Update()
    {
        if (!isActive) return;

        // Handle mouse click
        if (Input.GetMouseButtonDown(0))
        {
            HandleTrashClick();
        }

        // Update collect button interactability
        collectTrashButton.interactable = remainingTrash.Count == 0;
    }

    private void HandleTrashClick()
    {
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(mousePosition, clickRadius, trashLayer);

        foreach (Collider2D collider in hitColliders)
        {
            GameObject trashObject = collider.gameObject;
            if (remainingTrash.Contains(trashObject))
            {
                CollectTrash(trashObject);
            }
        }
    }

    private void CollectTrash(GameObject trashObject)
    {
        remainingTrash.Remove(trashObject);
        Destroy(trashObject);
    }

    private void OnCollectTrashButtonClicked()
    {
        if (remainingTrash.Count == 0)
        {
            isActive = false;
            // You can add an event here to notify your UI management script
        }
    }
}
