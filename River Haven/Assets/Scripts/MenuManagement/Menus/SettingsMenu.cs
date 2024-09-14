using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MenuManagement.Data;

namespace MenuManagement
{
    
    public class SettingsMenu : Menu<SettingsMenu>
    {
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;

        private DataManager dataManager;

        protected override void Awake()
        {
            base.Awake();
            dataManager = Object.FindObjectOfType<DataManager>();                    
        }

        private void Start()
        {
            LoadData();
        }
        public void OnMasterVolumeChanged(float volume)
        {
            if (dataManager != null)
            {
                dataManager.MasterVolume = volume;
            }
            
        }
        
        
        public void OnMusicVolumeChanged(float volume)
        {
            if (dataManager != null)
            {
                dataManager.MusicVolume = volume;
            }
            
            
        }

        public void OnSfxVolumeChanged(float volume)
        {
            if (dataManager != null)
            {
                dataManager.SfxVolume = volume;
            }
            
            
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            if (dataManager != null)
            {
                dataManager.Save();
            }

           
        }

        public void LoadData()
        {
            if (dataManager == null || masterVolumeSlider == null || musicVolumeSlider == null || sfxVolumeSlider == null)
            {
                return;
            }

            dataManager.Load();

            masterVolumeSlider.value = dataManager.MasterVolume;
            musicVolumeSlider.value = dataManager.MusicVolume;
            sfxVolumeSlider.value = dataManager.SfxVolume;

        }
            
    }
        

}

