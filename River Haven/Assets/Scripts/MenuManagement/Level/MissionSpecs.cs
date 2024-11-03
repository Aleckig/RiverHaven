using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

namespace MenuManagement.Level
{
    [Serializable]
    public class MissionSpecs 
    {
        [SerializeField] protected string name;
        [SerializeField] [Multiline] protected string description;
        [SerializeField] protected string sceneName;
        [SerializeField] protected string id;
        [SerializeField] protected Sprite image;

        public string Name { get { return name; } }
        public string Description { get { return description; } }
        public string SceneName { get { return sceneName; } }
        public string Id { get { return id; } }
        public Sprite Image { get { return image; } }
        
    }
}
