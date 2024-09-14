using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;  

namespace MenuManagement
{
    public class GameMenu : Menu<GameMenu>
    {
        private float normalSpeed = 1.0f;
        private float increasedSpeed = 2f;
        private bool isGameSpeedIncreased = false;

        public void OnPausePressed()
        {
            Time.timeScale = 0;
            PauseMenu.Open();
        }

        public void OnChangeGameSpeedPressed()
        {
            if (isGameSpeedIncreased)
            {
                Time.timeScale = normalSpeed; // Set back to normal speed
            }
            else
            {
                Time.timeScale = increasedSpeed; // Increase the game speed
            }

            isGameSpeedIncreased = !isGameSpeedIncreased;
        }
    }
    
    
}