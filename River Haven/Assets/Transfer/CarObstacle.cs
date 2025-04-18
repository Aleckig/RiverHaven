using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarObstacle : MonoBehaviour
{
    [SerializeField] private CarBehaviour carBehaviour;
    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            carBehaviour.PauseMovement();
            carBehaviour.PlayHonkSound();
        }
        if (other.gameObject.CompareTag("Car"))
            carBehaviour.PauseMovement();
    }
    void OnTriggerExit(Collider other)
    {
        carBehaviour.ContinueMovement();
    }
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
