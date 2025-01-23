using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;

public class RouteSettings : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 5;
    [SerializeField] private List<RouteAction> Actions;
    private NPCAnimator animator;
    private NavMeshAgent navMeshAgent;
    public RouteManager routeManager;
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
        animator = GetComponent<NPCAnimator>();
        navMeshAgent = GetComponent<NavMeshAgent>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (pauseOnTrigger && other.gameObject == pauseOnTriggerObject)
        {
            pauseMovement = true;
            animator.StopWalking();
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

    public void FollowPathWithTp(int routeId)
    {
        List<string> waypoints = routeManager.GetRoute(routeId).Route;
        StartCoroutine(MoveTo(waypoints));
    }

    public void FollowPathWithoutTp(int routeId)
    {
        List<string> waypoints = routeManager.GetRoute(routeId).Route;

        StartCoroutine(MoveTo(waypoints, false));
    }

    private IEnumerator MoveTo(List<string> waypoints, bool tpStatus = true)
    {

        int actionId = SaveActionStatus(cMinutes);

        Transform objectToMove = this.gameObject.transform;

        List<Transform> waypointObjList = new();

        foreach (string name in waypoints)
        {
            // Get position of the waypoint
            waypointObjList.Add(routeManager.GetWaypointsList.Find(e => e.waypointName == name).waypointObj);
        }

        if (tpStatus)
        {
            navMeshAgent.enabled = false;
            objectToMove.position = waypointObjList[0].position;
            navMeshAgent.enabled = true;
        }
        else yield return new WaitForSeconds(.5f);


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
                    animator.StopWalking();
                    ClearActionStatus(actionId, cMinutes);
                    yield break;
                }

                // Start animation for walking
                animator.StartWalking();
                // Smoothly rotate towards the current waypoint
                // SmoothRotateTowards(waypointObj);
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
        animator.StopWalking();
        ClearActionStatus(actionId, cMinutes);
        yield break;
    }

    public void SmoothRotateTowardsSomeone(Transform aimObj)
    {
        StartCoroutine(SmoothRotateTowardsCourutine(aimObj));
    }

    private bool IsAimedAtTarget(Transform target)
    {
        // Calculate the direction vector from the object to the target
        Vector3 directionToTarget = target.position - transform.position;

        // Normalize the direction vector (optional but recommended)
        directionToTarget.Normalize();

        // Calculate the angle between the object's forward direction and the direction to the target
        float angle = Vector3.Angle(transform.forward, directionToTarget);

        // Check if the angle is within the aim threshold
        return angle <= 4f;
    }

    private IEnumerator SmoothRotateTowardsCourutine(Transform aimObj)
    {
        while (true)
        {
            Vector3 direction = (aimObj.position - transform.position).normalized;
            // Calculate the target rotation based on the direction
            Quaternion targetRotation = Quaternion.LookRotation(direction);

            // Smoothly interpolate towards the target rotation using Slerp with Time.deltaTime
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

            // Debug.Log(gameObject.name + " rotation " + transform.rotation.eulerAngles);

            if (IsAimedAtTarget(aimObj)) break;

            // Otherwise, continue next frame
            yield return new WaitForFixedUpdate();
        }

        Debug.Log("Courutine stopped");
        yield break;
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

    public void PauseMovementForSeconds(float seconds)
    {
        pauseMovement = true;
        animator.StopWalking();
        navMeshAgent.ResetPath();
        StartCoroutine(ContinueMovementAfterSeconds(seconds));
    }

    private IEnumerator ContinueMovementAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ResetState();
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
            if (item.CheckTime(minutes))
            {
                item.callBacks?.Invoke();
            }
        }
    }
    // End of Functions block for invoking callbacks

    public void HideNPC(Transform resetPoint)
    {
        navMeshAgent.enabled = false;
        transform.position = resetPoint.position;
        navMeshAgent.enabled = true;
    }
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