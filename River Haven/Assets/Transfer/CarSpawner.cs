using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSpawner : MonoBehaviour
{
    [SerializeField] private List<GameObject> carList = new();
    [SerializeField] private float minDelay = 5f;
    [SerializeField] private float maxDelay = 20f;
    static private int closestSpotID = 6;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(Spawner());
    }

    private IEnumerator Spawner()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(minDelay, maxDelay));
            SpawnCar();
        }
    }
    private void SpawnCar()
    {
        int randomIndex = Random.Range(1, 6);
        Debug.Log($"SpawnCar() called closetestSpotID:{closestSpotID} and randomindex:{randomIndex} ");
        if (randomIndex >= closestSpotID) return;
        closestSpotID = randomIndex;
        //Select random car from the list
        GameObject car = GetRandomCar();
        car.SetActive(true);
        CarBehaviour carBehaviour = car.GetComponent<CarBehaviour>();
        carBehaviour.SetStopPointID = closestSpotID;
    }

    static public void ResetClosestSpotID(int id)
    {
        if (id == closestSpotID)
            closestSpotID = 6;
    }

    private GameObject GetRandomCar()
    {
        int randomIndex = Random.Range(0, carList.Count);
        if (carList[randomIndex].activeSelf)
        {
            return GetRandomCar();
        }
        return carList[randomIndex];
    }
}
