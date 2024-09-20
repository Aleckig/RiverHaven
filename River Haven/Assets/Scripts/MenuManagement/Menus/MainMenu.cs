using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MenuManagement.Data; 
using TMPro;
using RiverHaven;

namespace MenuManagement
{
    

    public class MainMenu : Menu<MainMenu>
    {
        

        private DataManager dataManager;

        [SerializeField] private TMP_InputField playerNameInputField;

        protected override void Awake()
        {
            base.Awake();
            dataManager = Object.FindObjectOfType<DataManager>();
        }   
        
        void Start()
        {
            LoadData();
            //AudioManager.instance.PlayMenuMusic();
        }

        private void LoadData()
        {
            if (dataManager != null && playerNameInputField != null)
            {
                dataManager.Load();
                playerNameInputField.text = dataManager.PlayerName; 
            }
        }

        public void OnPlayerNameValueChanged(string name)
        {
            if (dataManager != null)
            {
                dataManager.PlayerName = name;
            }
        }

        public void OnPlayerNameEndEdit()
        {
            if (dataManager != null)
            {          
                dataManager.Save();
            }
        }   

            

        public void OnPlayPressed()
        {
            LevelLoader.LoadNextLevel();     
        }
        

        

        public void OnSettingsPressed()
        {
                
            SettingsMenu.Open();
        }

        public void OnCreditsPressed()
        {

            CreditsScreen.Open();
        }

        public override void OnBackPressed()
        {
                Application.Quit();
        }
        
    }
    
}
