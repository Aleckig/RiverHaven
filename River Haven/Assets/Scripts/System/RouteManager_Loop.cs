using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;

[ExecuteInEditMode]
public class RouteManager_Loop : MonoBehaviour
{
  [SerializeField] private string cTime;
  [OnValueChanged("UpdateStaticList")]
  [SerializeField] private List<Waypoint> WaypointsList = new();
  [SerializeField] private List<RouteWaypoints> RoutesList = new();
  public static List<Waypoint> SWaypointsList = new();
  private int cMinutes;
  private List<RouteSettings_Loop> NpcList = new();
  public List<Waypoint> GetWaypointsList => WaypointsList;
  private static RouteManager_Loop Manager;

  private void Update()
  {
    // For correct displayment of values for dropdown option
    if (Application.isPlaying) return;
    // Debug.Log("Updated list");
    UpdateStaticList();
    //
  }
  private void Awake()
  {
    Manager = GetComponent<RouteManager_Loop>();
  }
  public RouteWaypoints GetRoute(int routeId)
  {
    return RoutesList.Find((e) => e.routeId == routeId);
  }

  public void UpdateStaticList()
  {
    SWaypointsList = WaypointsList;
  }

  private string ConvTimeIntToStr(int minutes)
  {
    int _hours = minutes / 60;
    int _minutes = minutes - (_hours * 60);

    return _hours + ":" + (_minutes > 10 ? _minutes.ToString() : "0" + _minutes);
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

      foreach (var item in SWaypointsList)
      {
        if (item.waypointName == null) continue;
        names.Add(item.waypointName);
      }

      return names;
    }
  }
}
