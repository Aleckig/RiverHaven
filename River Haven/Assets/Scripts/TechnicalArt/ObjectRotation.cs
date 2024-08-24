using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 100f;   // Speed of rotation
    [SerializeField] private Vector3 rotationAxis = Vector3.up;  // Axis of rotation

    void Update()
    {
        // Rotate the object around the specified axis
        transform.Rotate(rotationAxis, rotationSpeed * Time.deltaTime);
    }
}
