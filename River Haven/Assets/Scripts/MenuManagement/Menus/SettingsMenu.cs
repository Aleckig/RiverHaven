using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using MenuManagement.Data;
using UnityEngine.Audio;

namespace MenuManagement
{
    
    public class SettingsMenu : Menu<SettingsMenu>
    {
        [Header("Volume")]
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private AudioMixer volumeMixer;

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

            volumeMixer.SetFloat("MASTER", Mathf.Log10(volume) * 20);
        }

        public void OnSFXVolumeChanged(float volume)
        {
            if (dataManager != null)
            {
                dataManager.SfxVolume = volume;
            }

            volumeMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        }

        public void OnMusicVolumeChanged(float volume)
        {
            if (dataManager != null)
            {
                dataManager.MusicVolume = volume;
            }

            volumeMixer.SetFloat("MUSIC", Mathf.Log10(volume) * 20);
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
            if (dataManager == null || masterVolumeSlider == null ||
                sfxVolumeSlider == null || musicVolumeSlider == null)
            {
                return;
            }

            dataManager.Load();

            masterVolumeSlider.value = dataManager.MasterVolume;
            sfxVolumeSlider.value = dataManager.SfxVolume;
            musicVolumeSlider.value = dataManager.MusicVolume;
        }
    }
            
    
        

}

