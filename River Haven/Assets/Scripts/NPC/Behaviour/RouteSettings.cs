using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class RouteSettings : MonoBehaviour
{
    [SerializeField] private float speed = 2;
    [SerializeField] private float rotationSpeed = 5;
    [SerializeField] private List<RouteAction> Actions;
    private static RouteManager routeManager;
    private int routeId = -1;

    public RouteManager RouteManager { set { routeManager = value; } }
    public int RouteId { set { routeId = value; } }

    // private void Awake()
    // {
    //     routeManager = FindObjectOfType<RouteManager>();
    // }

    public void ExecuteAction(int minutes)
    {
        foreach (var item in Actions)
        {
            if (item.CheckTime(minutes))
            {
                item.events?.Invoke();

                item.callBack?.Invoke();
            }
        }
    }

    public void FollowPath(Transform objectToMove)
    {
        List<string> waypoints = routeManager.GetRoute(routeId).Route;

        StartCoroutine(MoveTo(objectToMove, waypoints));
    }

    private IEnumerator MoveTo(Transform objectToMove, List<string> waypoints)
    {
        List<Transform> waypointObjList = new();

        foreach (string name in waypoints)
        {
            // Get the target waypoints
            waypointObjList.Add(routeManager.GetWaypointsList.Find(e => e.waypointName == name).waypointObj);
        }

        objectToMove.position = waypointObjList[0].position;

        foreach (Transform waypointObj in waypointObjList)
        {
            if (waypointObj == null) yield break;

            // Start moving
            while (true)
            {
                // Move towards the current waypoint
                Vector3 direction = (waypointObj.position - transform.position).normalized;
                objectToMove.transform.position += direction * speed * Time.deltaTime;

                // Smoothly rotate towards the current waypoint
                SmoothRotateTowards(direction);

                // Check if the NPC has reached the waypoint
                if (Vector3.Distance(transform.position, waypointObj.position) < 0.1f)
                {

                    break;
                }

                yield return new WaitForFixedUpdate();
                // Leave the routine and return here in the next frame
            }

            objectToMove.position = waypointObj.position;
        }

        yield break;
    }

    void SmoothRotateTowards(Vector3 direction)
    {
        // Calculate the target rotation based on the direction
        Quaternion targetRotation = Quaternion.LookRotation(direction);

        // Smoothly interpolate towards the target rotation using Slerp with Time.deltaTime
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
    }
}

[Serializable]
public class RouteAction
{
    [Tooltip("Set time if you don't want to convert it to minutes. Example - 11:30")]
    public string time = "6:00";
    public UnityEvent events;
    public UnityEvent callBack;
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