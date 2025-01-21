/*
 * MenuManager Script
 * 
 * Description:
 * This script manages a stack-based menu system in Unity. It initializes and handles
 * the menus in the game, allowing menus to be opened and closed dynamically. The 
 * script ensures only one menu is visible at a time by deactivating other menus 
 * when a new one is opened. It also keeps track of the order in which menus are opened 
 * using a stack, enabling the correct behavior when closing menus.
 *
 * Key Features:
 * - Initializes menus from prefabs at runtime.
 * - Uses a stack data structure to manage the active menu hierarchy.
 * - Prevents duplicate MenuManager instances using a Singleton pattern.
 * - Provides methods for opening and closing menus.
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Reflection;
using RiverHaven;
using MenuManagement.Data;


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

        
    }

    
}