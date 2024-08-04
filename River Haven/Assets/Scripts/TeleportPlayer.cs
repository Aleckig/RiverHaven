using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TeleportPlayer : MonoBehaviour
{
    [SerializeField] private Transform playerTransformation;
    [SerializeField] private Transform destination;
    [SerializeField] private GameObject playerObject;

    void OnTriggerEnter(Collider other)
    {
        //Debug.Log("OnTriggerEnter called with " + other.gameObject.name);

        if (other.CompareTag("Player"))
        {
            //Debug.Log("Player detected. Starting teleportation.");
            
            playerObject.SetActive(false);
            //Debug.Log("Player object deactivated.");

            playerTransformation.position = destination.position;
            //Debug.Log("Player position set to destination: " + destination.position);

            playerObject.SetActive(true);
            //Debug.Log("Player object reactivated.");
        }
        else
        {
            Debug.Log("Collided object is not the player.");
        }
    }
    

    
    
}
