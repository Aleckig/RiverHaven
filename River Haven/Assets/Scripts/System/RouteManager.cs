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
    cMinutes = ConvTimeStrToInt(cTime);
    NpcList = FindObjectsOfType<RouteSettings>().ToList<RouteSettings>();

    NpcList[0].RouteManager = GetComponent<RouteManager>();

    CheckExecution();
  }

  private void CheckExecution()
  {
    foreach (var npc in NpcList)
    {
      npc.ExecuteAction(cMinutes);
    }
  }

  private void ChangeMinutes(int value)
  {
    if (cMinutes + value >= 60 || cMinutes + value < 0)
    {
      cMinutes = 0;
      return;
    }

    cMinutes += value;

    cTime = ConvTimeIntToStr(cMinutes);

    CheckExecution();
  }

  public void AddHalfHour()
  {
    ChangeMinutes(30);
  }

  public void AddHour()
  {
    ChangeMinutes(60);
  }

  public void AddFewHours(int numberOfHours)
  {
    ChangeMinutes(60 * numberOfHours);
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

    return _hours + ":" + _minutes;
  }

  private int ConvTimeStrToInt(string time)
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

