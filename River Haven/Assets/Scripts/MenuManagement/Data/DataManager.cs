using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MenuManagement.Data
{
    public class DataManager : MonoBehaviour
    {
        private SaveData saveData;
        private JsonSaver jsonSaver;

        public float MasterVolume
        {
            get { return saveData.masterVolume; }
            set { saveData.masterVolume = value; }
        }

        public float MusicVolume
        {
            get { return saveData.musicVolume; }
            set { saveData.musicVolume = value; }
        }

        public float SfxVolume
        {
            get { return saveData.sfxVolume; }
            set { saveData.sfxVolume = value; }
        }

        public string PlayerName
        {
            get { return saveData.playerName; }
            set { saveData.playerName = value; }
        }
        

       

        private void Awake()
        {
            saveData = new SaveData();
            jsonSaver = new JsonSaver();
        }

        public void Save()
        {
            jsonSaver.Save(saveData);
        }

        public void Load()
        {
            jsonSaver.Load(saveData);
           
        }
    }    

        
}

