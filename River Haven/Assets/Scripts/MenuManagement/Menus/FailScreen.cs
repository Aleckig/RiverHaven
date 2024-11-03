using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace MenuManagement
{
    public class FailScreen : Menu<FailScreen>
    {

        public void OnRestartPressed()
        {
            base.OnBackPressed();
            LevelLoader.ReloadLevel();
        }

        public void OnMainMenuPressed()
        {
            LevelLoader.LoadMainMenuLevel();
            MainMenu.Open();
        }
    }
}