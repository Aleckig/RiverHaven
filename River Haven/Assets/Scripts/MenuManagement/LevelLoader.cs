using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

namespace MenuManagement
{
    public class LevelLoader : MonoBehaviour
    {

        private static int mainMenuIndex = 1;

        public static void LoadLevel(string levelName)
        {
            if (Application.CanStreamedLevelBeLoaded(levelName))
            {
                Time.timeScale = 1; // Set the time scale back to normal
                SceneManager.LoadScene(levelName);
            }
            else
            {
                Debug.LogWarning("GAMEMANAGER LoadLevel Error: invalid scene specified!");
            }
        }

        public static void LoadLevel(int levelIndex)
        {
            if (levelIndex >= 0 && levelIndex < SceneManager.sceneCountInBuildSettings)
            {
                Time.timeScale = 1;  // Set the time scale back to normal
                if (levelIndex == LevelLoader.mainMenuIndex)
                {
                    MainMenu.Open();
                }
                

                SceneManager.LoadScene(levelIndex);
            }
            else
            {
                Debug.LogWarning("LEVELLOADER LoadLevel Error: invalid scene specified!");
            }
        }

        public static void ReloadLevel()
        {
            Time.timeScale = 1;  // Set the time scale back to normal
            LoadLevel(SceneManager.GetActiveScene().name);
        }

        public static void LoadNextLevel()
        {
            int nextSceneIndex = (SceneManager.GetActiveScene().buildIndex + 1)
                % SceneManager.sceneCountInBuildSettings;
            nextSceneIndex = Mathf.Clamp(nextSceneIndex, mainMenuIndex, nextSceneIndex);
            Time.timeScale = 1;  // Set the time scale back to normal
            LoadLevel(nextSceneIndex);

        }

        public static void LoadMainMenuLevel()
        {
            Time.timeScale = 1;  // Set the time scale back to normal 
            LoadLevel(mainMenuIndex);
        }

        

    }
}

