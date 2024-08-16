using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NPCpath : MonoBehaviour
{
    public Transform[] waypoints;  // Array to hold the checkpoints
    [SerializeField] private float speed = 2f;       // Speed of the NPC
    [SerializeField] private float rotationSpeed = 5f; // Speed of the rotation towards the waypoint

    private int currentWaypointIndex = 0;

    void Update()
    {
        // If there are no waypoints, return
        if (waypoints.Length == 0) return;

        // Get the current target waypoint
        Transform targetWaypoint = waypoints[currentWaypointIndex];

        // Move towards the current waypoint
        Vector3 direction = (targetWaypoint.position - transform.position).normalized;
        transform.position += direction * speed * Time.deltaTime;

        // Smoothly rotate towards the current waypoint
        SmoothRotateTowards(direction);

        // Check if the NPC has reached the waypoint
        if (Vector3.Distance(transform.position, targetWaypoint.position) < 0.1f)
        {
            // Update the waypoint index to the next one
            currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
        }
    }

    void SmoothRotateTowards(Vector3 direction)
    {
        // Calculate the target rotation based on the direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly interpolate towards the target rotation using Slerp with Time.deltaTime
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    
}
