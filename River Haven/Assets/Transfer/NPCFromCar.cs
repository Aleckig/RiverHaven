using System.Collections;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.AI;

public class NPCFromCar : MonoBehaviour
{
    [SerializeField] private List<Transform> destShopList = new();
    [SerializeField][ReadOnly] private int destinationShopIndex = 0;
    [SerializeField] private Transform hidePoint;
    [SerializeField] private float minDelay = 5f;
    [SerializeField] private float maxDelay = 15f;
    private NavMeshAgent navMeshAgent;
    private NPCAnimator npcAnimator;
    // Start is called before the first frame update
    void Start()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
        npcAnimator = GetComponent<NPCAnimator>();
    }

    public void StartMovement(int desShopIndex, Transform destCar, GameObject car)
    {
        destinationShopIndex = desShopIndex;
        float hideTime = Random.Range(minDelay, maxDelay);
        Transform destShop = destShopList[destinationShopIndex - 1];
        StartCoroutine(StartMovement(destShop, destCar, hideTime, car));
    }

    private IEnumerator StartMovement(Transform destinationShop, Transform destinationCar, float hideTime, GameObject car)
    {
        GameObject childObj = transform.GetChild(0).gameObject;
        CarBehaviour carBehaviour = car.GetComponent<CarBehaviour>();

        yield return new WaitForSeconds(1f);
        navMeshAgent.enabled = false;
        transform.position = destinationCar.position;
        navMeshAgent.enabled = true;
        navMeshAgent.SetDestination(destinationShop.position);
        npcAnimator.StartWalking();
        yield return new WaitUntil(() => CalcDistance(destinationShop) <= .3f);
        npcAnimator.StopWalking();
        yield return new WaitForSeconds(.2f);
        childObj.SetActive(false);
        //
        Vector3 directionToTarget = destinationCar.position - transform.position;
        transform.rotation = Quaternion.LookRotation(directionToTarget, Vector3.up);
        //
        yield return new WaitForSeconds(hideTime);
        //
        childObj.SetActive(true);
        npcAnimator.StartWalking();
        navMeshAgent.SetDestination(destinationCar.position);
        yield return new WaitUntil(() => CalcDistance(destinationCar) <= .3f);
        npcAnimator.StopWalking();
        yield return new WaitForSeconds(.2f);
        childObj.SetActive(false);
        navMeshAgent.enabled = false;
        transform.position = hidePoint.position;
        navMeshAgent.enabled = true;
        childObj.SetActive(true);
        yield return new WaitForSeconds(1f);
        carBehaviour.NPCGetInCar();
        CarSpawner.ResetClosestSpotID(destinationShopIndex);
        yield break;
    }

    private float CalcDistance(Transform dest)
    {
        Vector3 npcPos = new(transform.position.x, 0, transform.position.z);
        Vector3 waypointPos = new(dest.position.x, 0, dest.position.z);
        return (npcPos - waypointPos).magnitude;
    }
}