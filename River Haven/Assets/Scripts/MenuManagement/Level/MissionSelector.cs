using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace MenuManagement.Level
{
    public class MissionSelector : MonoBehaviour
    {
        [SerializeField] protected MissionList missionList;
        protected int currentIndex = 0; 

        public int CurrentIndex => currentIndex;    

        public void ClampIndex()
        {
            if(missionList.totalMissions == 0)
            {
                Debug.LogError("Mission List is empty");
                return;
            }
            if(currentIndex >= missionList.totalMissions)
            {
                currentIndex = 0;
            }
            if(currentIndex < 0)
            {
                currentIndex = missionList.totalMissions - 1;
            }
        }

        public void SetIndex(int index)
        {
            currentIndex = index;
            ClampIndex();
        }

        public void IncrementIndex()
        {
            SetIndex(currentIndex + 1);
        }

        public void DecrementIndex()
        {
            SetIndex(currentIndex - 1);
        }

        public MissionSpecs GetMission(int index)
        {
            return missionList.GetMission(index);
        }

        public MissionSpecs GetCurrentMission()
        {
            return GetMission(currentIndex);
        }   


    }
}


