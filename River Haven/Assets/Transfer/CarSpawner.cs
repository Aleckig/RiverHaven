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
        if (randomIndex >= closestSpotID) return;

        closestSpotID = randomIndex;
        GameObject car = carList[Random.Range(0, carList.Count)];
        car.SetActive(true);
        CarBehaviour carBehaviour = car.GetComponent<CarBehaviour>();
        carBehaviour.SetStopPointID = closestSpotID;
    }

    static public void ResetClosestSpotID(int id)
    {
        if (id == closestSpotID)
            closestSpotID = 6;
    }
}
