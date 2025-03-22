using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuNPC : MonoBehaviour
{
    public Transform[] checkpoints; // Assign the checkpoints in the Unity Editor
    public float speed = 5f;        // Movement speed
    public float reachDistance = 0.5f; // Distance at which the checkpoint is considered "reached"
    public float rotationSpeed = 5f;  // Speed for smooth rotation towards the checkpoint

    private int currentCheckpoint = 0;

    void Update()
    {
        if (checkpoints.Length == 0) return; // Ensure there are checkpoints

        // Move towards the current checkpoint
        Transform targetCheckpoint = checkpoints[currentCheckpoint];
        Vector3 direction = (targetCheckpoint.position - transform.position).normalized;

        // Rotate towards the checkpoint
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        // Move towards the target
        transform.position += direction * speed * Time.deltaTime;

        // Check if the NPC has reached the current checkpoint
        float distance = Vector3.Distance(transform.position, targetCheckpoint.position);
        if (distance <= reachDistance)
        {
            currentCheckpoint++;
            if (currentCheckpoint >= checkpoints.Length)
            {
                // Reset to the first checkpoint (looping behavior)
                currentCheckpoint = 0;
            }
        }
    }
}
