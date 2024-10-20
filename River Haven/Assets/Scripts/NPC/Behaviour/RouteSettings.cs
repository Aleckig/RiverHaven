using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class RouteSettings : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5;
    [SerializeField] private List<RouteAction> Actions;
    [SerializeField] private Animator animator;
    private NavMeshAgent navMeshAgent;
    private RouteManager routeManager;
    private Dictionary<int, int> actionsInProg = new();
    private int idCount = 0;
    private int cMinutes = 0;
    // For pausing or finishing of the movement
    private bool finishMovement = false;
    private bool pauseMovement = false;
    // Conditions for pausing of the
    private bool pauseOnTrigger = false;
    private GameObject pauseOnTriggerObject;

    private void Awake()
    {
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void Start()
    {
        routeManager = RouteManager.Manager;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pauseOnTrigger && other.gameObject == pauseOnTriggerObject)
        {
            pauseMovement = true;
            animator.SetBool("Walking", false);
            navMeshAgent.ResetPath();
        }
    }

    public void ExecuteAction(int minutes)
    {
        foreach (var item in Actions)
        {
            if (item.CheckTime(minutes))
            {
                cMinutes = minutes;

                item.events?.Invoke();
            }
        }
    }

    public void FollowPath(int routeId)
    {
        List<string> waypoints = routeManager.GetRoute(routeId).Route;

        StartCoroutine(MoveTo(waypoints));
    }

    private IEnumerator MoveTo(List<string> waypoints)
    {
        int actionId = SaveActionStatus(cMinutes);

        Transform objectToMove = this.gameObject.transform;

        List<Transform> waypointObjList = new();

        foreach (string name in waypoints)
        {
            // Get position of the waypoint
            waypointObjList.Add(routeManager.GetWaypointsList.Find(e => e.waypointName == name).waypointObj);
        }

        objectToMove.position = waypointObjList[0].position;

        foreach (Transform waypointObj in waypointObjList)
        {
            if (waypointObj == null)
            {
                ClearActionStatus(actionId, cMinutes);
                yield break;
            }

            // Start moving
            while (true)
            {
                //Pause movement
                while (pauseMovement)
                {
                    if (finishMovement) break;
                    yield return new WaitForSeconds(1f);
                }
                //Statement for checking if movement ended but coroutine not finished
                if (finishMovement)
                {
                    navMeshAgent.ResetPath();
                    animator.SetBool("Walking", false);
                    ClearActionStatus(actionId, cMinutes);
                    yield break;
                }

                // Start animation for walking
                animator.SetBool("Walking", true);
                // Smoothly rotate towards the current waypoint
                Vector3 direction = (waypointObj.position - transform.position).normalized;
                SmoothRotateTowards(direction);
                // Move towards the current waypoint
                navMeshAgent.SetDestination(waypointObj.position);

                // Check if the NPC has reached the waypoint
                Vector3 npcPos = new(transform.position.x, 0, transform.position.z);
                Vector3 waypointPos = new(waypointObj.position.x, 0, waypointObj.position.z);
                float distance = (npcPos - waypointPos).magnitude;

                if (distance <= .3f)
                {
                    break;
                }

                yield return new WaitForFixedUpdate();
                // Leave the routine and return here in the next frame
            }
        }

        navMeshAgent.ResetPath();
        animator.SetBool("Walking", false);
        ClearActionStatus(actionId, cMinutes);
        yield break;
    }

    void SmoothRotateTowards(Vector3 direction)
    {
        // Calculate the target rotation based on the direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly interpolate towards the target rotation using Slerp with Time.deltaTime
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }

    private void ResetState()
    {
        finishMovement = false;
        pauseMovement = false;
        pauseOnTrigger = false;
    }
    public void ContinueMovement()
    {
        ResetState();
    }

    public void PauseMovementOnTrigger(GameObject _gameObject)
    {
        pauseOnTrigger = true;
        pauseOnTriggerObject = _gameObject;
    }

    //Functions block for invoking callbacks
    //--
    //First of all, that functions for actions that could take some time for execution.
    //But still better to add that for each "Actions" function in case of proper execution of callbacks.
    private int SaveActionStatus(int minutes)
    {
        idCount++;

        actionsInProg.Add(idCount, minutes);

        return idCount;
    }

    private void ClearActionStatus(int actionId, int minutes)
    {
        if (actionsInProg.ContainsKey(actionId))
        {
            actionsInProg.Remove(actionId);
        }
        else Debug.Log("RouteSettings Component: Error; Action id wasn't found.");

        InvokeCallBacks(minutes);
    }

    private void InvokeCallBacks(int minutes)
    {
        //Check if list contain some action in progress
        if (actionsInProg.ContainsValue(minutes)) return;

        //If no action found for proper item from list,
        //then execute callbacks
        foreach (var item in Actions)
        {
            Debug.Log(item);
            if (item.CheckTime(minutes))
            {
                item.callBacks?.Invoke();
            }
        }
    }
    // End of Functions block for invoking callbacks
}

internal class navMeshAgent
{
}

[Serializable]
public class RouteAction
{
    [Tooltip("Set time if you don't want to convert it to minutes. Example - 11:30")]
    public string time = "6:00";
    public UnityEvent events;
    public UnityEvent callBacks;
    private int minutes;

    public bool CheckTime(int _minutes)
    {
        minutes = ConvTimeStrToInt();

        if (minutes == _minutes) return true;

        return false;
    }

    private int ConvTimeStrToInt()
    {
        int _hours = 6;
        int _minutes = 0;

        string[] splited = time.Split(":");

        if (int.TryParse(splited[0], out _hours) && int.TryParse(splited[1], out _minutes))
        {
            Debug.Log("Successfully parsed time");
            _minutes = _minutes >= 60 || _minutes <= 0 ? 0 : _minutes;
        }
        else
        {
            Debug.Log("Failed to parse time");
            return 360;
        }

        return _hours * 60 + _minutes;
    }
}