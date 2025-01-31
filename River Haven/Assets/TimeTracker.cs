using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class TimeTracker : MonoBehaviour
{
    [SerializeField] private RouteManager routeManager;
    public int hours;
    public int minutes;
    public int day;

    void Start()
    {
        hours = 6;
        minutes = 0;
        day = 1;

        routeManager.SetTime(hours * 60 + minutes);
    }

    // Update is called once per frame
    void Update()
    {
        if (day == 1 && hours > 10)
        {
            day++;
            FirstDayEnds();

            routeManager.SetTime(hours * 60 + minutes);
        }
    }

    public void AddTime(int addHours, int addMinutes)
    {
        hours += addHours;
        minutes += addMinutes;

        if (minutes >= 60)
        {
            minutes -= 60;
            hours++;
        }

        if (hours > 24)
        {
            hours -= 24;
        }

        routeManager.SetTime(hours * 60 + minutes);
    }

    public void AddOneHour()
    {
        hours++;

        routeManager.SetTime(hours * 60 + minutes);
    }

    public void AddHalfHour()
    {
        minutes += 30;
        if (minutes >= 60)
        {
            minutes -= 60;
            hours++;
        }

        routeManager.SetTime(hours * 60 + minutes);
    }

    public void AddTwoHours()
    {
        hours = hours + 2;

        routeManager.SetTime(hours * 60 + minutes);
    }

    public void CheckIfDayEnds()
    {
        if (hours >= 18)
        {
            hours = 7;
            minutes = 0;
            day++;
        }
    }

    public void FirstDayEnds()
    {
        DialogueLua.SetVariable("firstDayEnds", true);
    }
}
