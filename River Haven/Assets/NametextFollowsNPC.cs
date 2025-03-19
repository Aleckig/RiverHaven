using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class NametextFollowsNPC : MonoBehaviour
{
    public Transform npcTransform; // The NPC's transform
    public Vector3 offset = new Vector3(0f, 3f, 0f); // Initial offset (position above the NPC)
    [SerializeField] private Vector3 nameOffset = new Vector3(0f, 0f, 0f); // Initial offset for the name

    [SerializeField] private float zOffsetChange = 0.5f; // The amount to move the names on the z-axis when they overlap (set in the Inspector)
    [SerializeField] private float moveSpeed = 5f; // Speed at which the name offset will transition

    private BoxCollider nameCollider; // The BoxCollider attached to this name object
    private bool isOverlapping = false; // Flag to check if the name is currently overlapping another
    private bool hasMoved = false; // Flag to track if the offset has been adjusted already

    [SerializeField] private Transform textMeshProTransform;

    void Start()
    {
        // Get the BoxCollider component attached to this GameObject
        nameCollider = GetComponent<BoxCollider>();

        // Find the TextMeshPro component, assuming it is a child of the GameObject
        textMeshProTransform = transform.GetChild(0);
    }

    void Update()
    {
        if (npcTransform != null)
        {
            // Update the position of the TextMeshPro object to follow the NPC, using the offset
            transform.position = npcTransform.position + offset;
            textMeshProTransform.localPosition = Vector3.Lerp(textMeshProTransform.localPosition, nameOffset, Time.deltaTime * moveSpeed);
        }
    }

    // Called when another collider stays within this trigger area
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Name")) // Check if the collided object is also a name
        {
            // Keep track of the overlap while staying within the trigger and adjust accordingly
            AdjustPosition(other);
            isOverlapping = true; // Ensure we are marking the overlap as true during OnTriggerStay
            hasMoved = true; // Ensure the offset is only adjusted once
        }
    }

    // Called when another collider exits this trigger area
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Name"))
        {
            // Reset position to the original once the overlap ends
            ResetPosition();
            isOverlapping = false; // We are no longer overlapping
            hasMoved = false; // Reset the hasMoved flag
        }
    }

    // Adjust this object's position based on the relative Z-axis of the NPCs
    private void AdjustPosition(Collider other)
    {
        // Get the other NPC's position (we assume the NPC has the same name as the name object's collider)
        Transform otherNpcTransform = other.GetComponentInParent<NametextFollowsNPC>().npcTransform;

        // Check if this NPC is above or below the other NPC based on the Z-axis
        if (npcTransform.position.z > otherNpcTransform.position.z)
        {
            // If this NPC is above the other NPC, move this name downward
            nameOffset = new Vector3(0, zOffsetChange, 0); // Move this name downwards
        }
        else
        {
            // If this NPC is below the other NPC, move this name upward
            nameOffset = new Vector3(0, -zOffsetChange, 0); // Move this name upwards
        }
    }

    // Reset the name's position to its original offset (no overlap)
    private void ResetPosition()
    {
        nameOffset = Vector3.zero; // Reset to the original offset of (0, 0, 0)
    }
}
