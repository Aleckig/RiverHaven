using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

public class RouteManager : MonoBehaviour
{
  [SerializeField] private string cTime;
  [OnValueChanged("UpdateStaticList")]
  [SerializeField] private List<Waypoint> WaypointsList = new();
  [SerializeField] private List<RouteWaypoints> RoutesList = new();
  public static List<Waypoint> SWaypointsList = new();
  private int cMinutes;
  private List<RouteSettings> NpcList = new();

  public List<Waypoint> GetWaypointsList => WaypointsList;

  private void Start()
  {
    cMinutes = int.Parse(cTime);
    NpcList = FindObjectsOfType<RouteSettings>().ToList<RouteSettings>();

    NpcList[0].RouteManager = GetComponent<RouteManager>();

    foreach (var npc in NpcList)
    {
      npc.ExecuteAction(cMinutes);
    }
  }
  public void ChangeMinutes(int value)
  {
    if (cMinutes + value >= 60 || cMinutes + value < 0)
    {
      cMinutes = 0;
      return;
    }

    cMinutes += value;
  }

  public RouteWaypoints GetRoute(int routeId)
  {
    return RoutesList.Find((e) => e.routeId == routeId);
  }

  public void UpdateStaticList()
  {
    SWaypointsList = WaypointsList;
  }
}

[Serializable]
public class RouteWaypoints
{
  public int routeId;
  [ValueDropdown("GetWaypointsNames")]
  public List<string> Route = new();

  private IEnumerable GetWaypointsNames()
  {
    ValueDropdownList<string> names = new();

    foreach (var item in RouteManager.SWaypointsList)
    {
      names.Add(item.waypointName);
    }

    return names;
  }
}

[Serializable]
public class Waypoint
{
  public string waypointName;
  public Transform waypointObj;
}

