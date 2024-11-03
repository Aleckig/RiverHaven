using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace MenuManagement
{
    public class PauseMenu : Menu<PauseMenu>
    {
        private PlayerController playerController;

        private void Start()
        {
            playerController = FindObjectOfType<PlayerController>();
        }

        public void OnResumePressed()
        {
            if (playerController != null)
            {
                playerController.ResumeGame();
            }
            else
            {
                Time.timeScale = 1; // Ensure the game is unpaused
                MenuManager.Instance.CloseMenu(); // Close the pause menu using MenuManager
            }
        }

        public void OnRestartPressed()
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            MenuManager.Instance.CloseMenu(); // Close all menus before restarting
        }

        public void OnMainMenuPressed()
        {
            Time.timeScale = 1;
            LevelLoader.LoadMainMenuLevel();
            MenuManager.Instance.CloseMenu(); // Close the pause menu
            MainMenu.Open();    
        }

        public void OnSettingsPressed()
        {
            SettingsMenu.Open();
        }

        public void OnQuitPressed()
        {
            Application.Quit();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                OnResumePressed();
            }
        }
    }
}