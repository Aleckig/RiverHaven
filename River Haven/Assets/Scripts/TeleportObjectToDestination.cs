using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportObjectToDestination : MonoBehaviour
{
    [SerializeField] private Transform destination;

    public void TeleportToTarget()
    {
        this.transform.position = destination.position;
        this.transform.rotation = destination.rotation;
    }
}
