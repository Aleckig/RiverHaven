using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PixelCrushers.DialogueSystem;

public class TimeTracker : MonoBehaviour
{
    public int hours;
    public int minutes;
    public int day;
    private bool questLogAlertShown = false;
    private bool mapAlertShown = false;

    void Start()
    {
        hours = 7;
        minutes = 0;
        day = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (day == 1 && hours > 13)
        {
            day++;
            FirstDayEnds();
        }

        if (day == 1 && hours == 10 && questLogAlertShown == false)
        {
            Invoke("ShowQuestLogAlert", 2f);
            questLogAlertShown = true;
        }

        if (day == 1 && hours == 10 && mapAlertShown == false)
        {
            Invoke("ShowMapAlert", 4f);
            mapAlertShown = true;
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
    }

    public void AddOneHour()
    {
        hours++;
    }

    public void AddHalfHour()
    {
        minutes += 30;
        if (minutes >= 60)
        {
            minutes -= 60;
            hours++;
        }
    }

    public void AddTwoHours()
    {
        hours = hours + 2;
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

    void ShowQuestLogAlert()
    {
        DialogueManager.ShowAlert("Press 'J' to open quest log");
    }

    void ShowMapAlert()
    {
        DialogueManager.ShowAlert("Press 'M' to open map");
    }
}
