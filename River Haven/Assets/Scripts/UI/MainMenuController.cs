using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Tymski;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    [SerializeField] private SceneReference StartScene;

    public void LoadGame()
    {
        SceneManager.LoadScene(StartScene);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
