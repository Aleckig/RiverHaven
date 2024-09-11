using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowRotation : MonoBehaviour
{
    [SerializeField] private Transform target;
    private Vector3 targetRotation;

    void Start()
    {
        targetRotation = new Vector3(target.position.x, this.transform.position.y, target.position.z);
    }
    void Update()
    {
        transform.LookAt(targetRotation);
    }
}
