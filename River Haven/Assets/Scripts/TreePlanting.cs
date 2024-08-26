using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TreePlanting : MonoBehaviour
{
public GameObject questMarker;         // Quest marker child object
    public GameObject dirt;                // Dirt child object
    public GameObject smallTree;           // Small tree child object
    public GameObject finalTree;           // Final tree child object
    public GameObject dirtParticleEffect;  // Particle effect for digging
    public GameObject waterParticleEffect; // Particle effect for watering

    private bool hasShovel = false;
    private bool hasPlant = false;
    private bool hasBucket = false;
    private bool inTreePlantingArea = false;

    void Start()
    {
        // Initially, only the quest marker should be active
        dirt.SetActive(false);
        smallTree.SetActive(false);
        finalTree.SetActive(false);
    }

    void Update()
    {
        if (inTreePlantingArea && hasShovel && Input.GetKeyDown(KeyCode.E) && questMarker.activeSelf)
        {
            StartCoroutine(Dig());
        }
        else if (inTreePlantingArea && hasPlant && Input.GetKeyDown(KeyCode.E) && dirt.activeSelf)
        {
            StartCoroutine(PlantTree());
        }
        else if (inTreePlantingArea && hasBucket && Input.GetKeyDown(KeyCode.E) && smallTree.activeSelf)
        {
            StartCoroutine(WaterTree());
        }
    }

    private IEnumerator Dig()
    {
        // Disable the quest marker
        questMarker.SetActive(false);

        // Simulate digging by pressing E several times
        for (int i = 0; i < 3; i++)
        {
            Instantiate(dirtParticleEffect, questMarker.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }

        // Activate the dirt object
        dirt.SetActive(true);
    }

    private IEnumerator PlantTree()
    {
        // Simulate planting by pressing E several times
        for (int i = 0; i < 3; i++)
        {
            Instantiate(dirtParticleEffect, dirt.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }

        // Activate the small tree and deactivate the dirt object
        smallTree.SetActive(true);
        dirt.SetActive(false);
    }

    private IEnumerator WaterTree()
    {
        // Simulate watering by pressing E several times
        for (int i = 0; i < 3; i++)
        {
            Instantiate(waterParticleEffect, smallTree.transform.position, Quaternion.identity);
            yield return new WaitForSeconds(0.5f);
        }

        // Activate the final tree and deactivate the small tree
        finalTree.SetActive(true);
        smallTree.SetActive(false);
    }

    public void PickUpShovel()
    {
        hasShovel = true;
    }

    public void PickUpPlant()
    {
        hasPlant = true;
    }

    public void PickUpBucket()
    {
        hasBucket = true;
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTreePlantingArea = true;
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTreePlantingArea = false;
        }
    }
}
