using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MenuManagement.Level
{
    [CreateAssetMenu(fileName = "MissionList", menuName = "Missions/Create MissionList", order = 1) ]
    public class MissionList : ScriptableObject
    {
        [SerializeField] private List<MissionSpecs> missions;   // List of missions

        public int totalMissions { get { return missions.Count; } }  // Total number of missions

        public MissionSpecs GetMission(int index)  // Get a mission by index
        {
            if (index < 0 || index >= missions.Count)
            {
                return null;
            }
            return missions[index];
        }
    }
}
