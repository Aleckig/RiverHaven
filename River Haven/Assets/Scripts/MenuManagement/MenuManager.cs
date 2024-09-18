using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using UnityEngine.SceneManagement;

namespace MenuManagement
{
    public class MenuManager : MonoBehaviour
    {
        [SerializeField] private MainMenu mainMenuPrefab;
        [SerializeField] private SettingsMenu settingsMenuPrefab;
        [SerializeField] private CreditsScreen creditsScreenPrefab;
        [SerializeField] private GameMenu gameMenuPrefab;
        [SerializeField] private PauseMenu pauseMenuPrefab;
        //[SerializeField] private WinScreen winScreenPrefab;
        //[SerializeField] private FailScreen failScreenPrefab;
        //[SerializeField] private LevelSelectMenu levelSelectMenuPrefab;
        
        [SerializeField]
        private Transform menuParent;
        private Stack<Menu> menuStack = new Stack<Menu>();
        private static MenuManager instance;

        public static MenuManager Instance { get { return instance; } }

        private void Awake()
        {
            if (instance != null)
            {
                Destroy(gameObject);
            }
            else
            {
                instance = this;
                InitializeMenus();
                DontDestroyOnLoad(gameObject);
            }
        }

        private void OnDestroy()
        {
            if (instance == this)
            {
                instance = null;
            }
        }

        // Listen to scene changes to close menus when switching to the game scene
        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnSceneLoaded;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // Close all menus when Scene 2 (the game scene) is loaded
            if (scene.buildIndex == 2) // Assuming Scene 2 is the game scene
            {
                CloseMenu();
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

            // Get the current scene to determine if we should open the main menu
            Scene currentScene = SceneManager.GetActiveScene();

            foreach (FieldInfo field in fields)
            {
                Menu prefab = field.GetValue(this) as Menu;
                if (prefab != null)
                {
                    Menu menuInstance = Instantiate(prefab, menuParent);
                    menuInstance.gameObject.SetActive(false); // Keep all menus inactive initially

                    // Only open the main menu in Scene 1
                    if (prefab == mainMenuPrefab && currentScene.buildIndex == 1) // Assuming Scene 1 is Main Menu
                    {
                        OpenMenu(menuInstance); // Open the Main Menu only in Scene 1
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
    }
}
