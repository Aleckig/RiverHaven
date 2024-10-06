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

        private AudioMixer volumeMixer;
        private DataManager dataManager;

        protected override void Awake()
        {
            base.Awake();
            dataManager = FindObjectOfType<DataManager>();
            if (dataManager == null)
            {
                Debug.LogError("DataManager not found in the scene!");
            }

            // Get the AudioMixer from MenuManager
            if (MenuManager.Instance != null)
            {
                volumeMixer = MenuManager.Instance.GetAudioMixer();
            }
            else
            {
                Debug.LogError("MenuManager instance not found!");
            }
        }

        private void Start()
        {
            LoadData();
            SetupSliders();
        }

        private void SetupSliders()
        {
            if (masterVolumeSlider != null) masterVolumeSlider.onValueChanged.AddListener(OnMasterVolumeChanged);
            if (sfxVolumeSlider != null) sfxVolumeSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
            if (musicVolumeSlider != null) musicVolumeSlider.onValueChanged.AddListener(OnMusicVolumeChanged);
        }

        public void OnMasterVolumeChanged(float volume)
        {
            SetVolume("Master", volume);
        }

        public void OnSFXVolumeChanged(float volume)
        {
            SetVolume("SFX", volume);
        }

        public void OnMusicVolumeChanged(float volume)
        {
            SetVolume("Music", volume);
        }

         private void SetVolume(string parameterName, float volume)
        {
            if (dataManager != null)
            {
                switch (parameterName)
                {
                    case "Master":
                        dataManager.MasterVolume = volume;
                        break;
                    case "SFX":
                        dataManager.SfxVolume = volume;
                        break;
                    case "Music":
                        dataManager.MusicVolume = volume;
                        break;
                }
            }

            if (volumeMixer != null)
            {
                float dbVolume = volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
                bool success = volumeMixer.SetFloat(parameterName, dbVolume);
                
                if (success)
                {
                    Debug.Log($"SettingsMenu: Set {parameterName} volume to {dbVolume} dB (linear value: {volume})");
                }
                else
                {
                    Debug.LogError($"SettingsMenu: Failed to set {parameterName} volume. Check if the AudioMixer parameter name is correct and exposed.");
                }
            }
            else
            {
                Debug.LogError("SettingsMenu: volumeMixer is null. Make sure MenuManager is initialized properly.");
            }
            
            SaveSettings();
        }

        public override void OnBackPressed()
        {
            base.OnBackPressed();
            SaveSettings();
        }

        private void SaveSettings()
        {
            if (dataManager != null)
            {
                dataManager.Save();
                Debug.Log("Settings saved.");
            }
            else
            {
                Debug.LogError("Failed to save settings: DataManager is null.");
            }
        }

        public void LoadData()
        {
            if (dataManager == null)
            {
                Debug.LogError("Cannot load data: DataManager is null.");
                return;
            }

            dataManager.Load();

            if (masterVolumeSlider != null) masterVolumeSlider.value = dataManager.MasterVolume;
            if (sfxVolumeSlider != null) sfxVolumeSlider.value = dataManager.SfxVolume;
            if (musicVolumeSlider != null) musicVolumeSlider.value = dataManager.MusicVolume;

            // Apply loaded values
            SetVolume("Master", dataManager.MasterVolume);
            SetVolume("SFX", dataManager.SfxVolume);
            SetVolume("Music", dataManager.MusicVolume);

            Debug.Log("Settings loaded and applied.");
        }
    }
}