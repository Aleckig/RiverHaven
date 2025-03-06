using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WalkToPlayer : MonoBehaviour
{
    [SerializeField] private Transform targetPoint; // The point the NPC will walk towards
    [SerializeField] private float moveSpeed; // Speed at which the NPC moves
    [SerializeField] private float stoppingDistance; // Distance at which the NPC stops
    [SerializeField] private Animator animator; // Animator to control animations

    private bool isWalking = false;

    void Start()
    {
        // Ensure the animator is assigned
        if (animator == null)
        {
            animator = GetComponent<Animator>();
        }
    }

    void Update()
    {
        // If the NPC is walking, move towards the target point
        if (isWalking && targetPoint != null)
        {
            MoveToTarget();

            // Play walking animation when moving
            if (Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(targetPoint.position.x, 0, targetPoint.position.z)) > stoppingDistance)
            {
                animator.SetBool("isWalking", true); // Assuming you have a "isWalking" parameter in your Animator
            }
            else
            {
                animator.SetBool("isWalking", false);
            }
        }
    }

    // This method starts the walking behavior
    public void StartWalkingToTarget()
    {
        if (targetPoint != null)
        {
            // Start walking
            isWalking = true;
        }
        else
        {
            Debug.LogError("Target point not assigned.");
        }
    }

    // Method to move the NPC towards the target, ignoring Y-axis
    void MoveToTarget()
    {
        // Convert target position to world space and ignore the Y-axis
        Vector3 targetPosition = new Vector3(targetPoint.position.x, transform.position.y, targetPoint.position.z);

        // Calculate the distance to the target in world space (ignoring Y-axis)
        float distanceToTarget = Vector3.Distance(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(targetPosition.x, 0, targetPosition.z));

        // Check if we are within the stopping distance
        if (distanceToTarget <= stoppingDistance)
        {
            // Stop moving and reset animation
            isWalking = false;
            animator.SetBool("isWalking", false);
            return;
        }

        // Calculate the direction to the target in world space (ignoring Y-axis)
        Vector3 direction = (targetPosition - new Vector3(transform.position.x, 0, transform.position.z)).normalized;

        // Move the NPC towards the target point, but don't overshoot (only on X and Z axes)
        transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, 0, transform.position.z), new Vector3(targetPosition.x, 0, targetPosition.z), moveSpeed * Time.deltaTime);

        // Update the NPC's Y position to stay at the original Y level
        transform.position = new Vector3(transform.position.x, targetPosition.y, transform.position.z);

        // Rotate the NPC to face the target point, ignoring Y-axis for rotation
        if (direction != Vector3.zero)
        {
            Quaternion targetRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Keep rotation on XZ plane
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 0.1f);
        }
    }
}
