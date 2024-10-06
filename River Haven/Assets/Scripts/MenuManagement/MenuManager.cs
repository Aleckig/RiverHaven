using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using RiverHaven;
using MenuManagement.Data;
using UnityEngine.Audio;

namespace MenuManagement
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private MainMenu mainMenuPrefab;
        [SerializeField] private SettingsMenu settingsMenuPrefab;
        [SerializeField] private CreditsScreen creditsScreenPrefab;
        [SerializeField] private GameMenu gameMenuPrefab;
        [SerializeField] private PauseMenu pauseMenuPrefab;
        [SerializeField] private LevelSelectMenu levelSelectMenuPrefab;
       
        [SerializeField] private Transform menuParent;
        [SerializeField] private AudioMixer audioMixer;

        private Stack<Menu> menuStack = new Stack<Menu>();
        private static MenuManager instance;
        public static MenuManager Instance { get { return instance; } }

        private DataManager dataManager;

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                dataManager = FindObjectOfType<DataManager>();
                if (dataManager == null)
                {
                    Debug.LogError("DataManager not found in the scene!");
                }
                InitializeMenus();
                InitializeAudioSettings();
                DontDestroyOnLoad(gameObject);
            }
        }

        private void InitializeMenus()
        {
            if (menuParent == null)
            {
                GameObject menuParentObject = new GameObject("Menus");
                menuParent = menuParentObject.transform;
            }

            DontDestroyOnLoad(menuParent.gameObject);

            BindingFlags myFlags = BindingFlags.Instance | BindingFlags.NonPublic | 
                                               BindingFlags.DeclaredOnly;
            FieldInfo[] fields = this.GetType().GetFields(myFlags);

            foreach (FieldInfo field in fields)
            {
                Menu prefab = field.GetValue(this) as Menu;
                if (prefab != null)
                {
                    Menu menuInstance = Instantiate(prefab, menuParent);
                    if (prefab != mainMenuPrefab)
                    {
                        menuInstance.gameObject.SetActive(false);
                    }
                    else
                    {
                        OpenMenu(menuInstance);
                    }
                }
            }
        }

        public void OpenMenu(Menu menuInstance)
        {
            if (menuInstance == null)
            {
                Debug.LogWarning("MENUMANAGER OpenMenu ERROR: invalid menu");
                return;
            }

            if (menuStack.Count > 0)
            {
                foreach (Menu menu in menuStack)
                {
                    menu.gameObject.SetActive(false);
                }
            }

            menuInstance.gameObject.SetActive(true);
            menuStack.Push(menuInstance);
        }

        public void CloseMenu()
        {
            if (menuStack.Count == 0)
            {
                Debug.LogWarning("MENUMANAGER CloseMenu ERROR: No menus in stack!");
                return;
            }

            Menu topMenu = menuStack.Pop();
            topMenu.gameObject.SetActive(false);

            if (menuStack.Count > 0)
            {
                Menu nextMenu = menuStack.Peek();
                nextMenu.gameObject.SetActive(true);
            }
        }

        private void InitializeAudioSettings()
        {
            if (dataManager != null && audioMixer != null)
            {
                dataManager.Load();
                SetVolume("Master", dataManager.MasterVolume);
                SetVolume("SFX", dataManager.SfxVolume);
                SetVolume("Music", dataManager.MusicVolume);
                Debug.Log("Audio settings loaded and applied at startup.");
            }
            else
            {
                Debug.LogError("Failed to initialize audio settings. DataManager or AudioMixer is missing.");
            }
        }

        private void SetVolume(string parameterName, float volume)
        {
            if (audioMixer != null)
            {
                float dbVolume = volume > 0 ? Mathf.Log10(volume) * 20 : -80f;
                bool success = audioMixer.SetFloat(parameterName, dbVolume);
                if (success)
                {
                    Debug.Log($"MenuManager: Set {parameterName} volume to {dbVolume} dB (linear value: {volume})");
                }
                else
                {
                    Debug.LogError($"MenuManager: Failed to set {parameterName} volume. Check if the AudioMixer parameter name is correct and exposed.");
                }
            }
            else
            {
                Debug.LogError("MenuManager: AudioMixer not assigned.");
            }
        }

        // Add this method to allow other scripts to access the AudioMixer
        public AudioMixer GetAudioMixer()
        {
            return audioMixer;
        }
    }

    
}