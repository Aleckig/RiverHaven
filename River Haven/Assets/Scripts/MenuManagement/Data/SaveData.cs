using System.Collections;
using System.Collections.Generic;
using System;


namespace MenuManagement.Data
{
    [Serializable]
    public class SaveData
    {
        public string playerName;
        private readonly string defaultName = "Minecraft Steve";
        public string hashValue;
        
        public float masterVolume;
        public float musicVolume;
        public float sfxVolume;

        public SaveData()
        {
            hashValue = string.Empty;
            playerName = defaultName;
            masterVolume = 0.5f;
            musicVolume = 0.5f;
            sfxVolume = 0.5f;
        }
    }
}

