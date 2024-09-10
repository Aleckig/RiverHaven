using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;


public class Button : MonoBehaviour
{
    // Reference to your animator component (if you're using animations)
    // public Animator transitionAnimator;

    // Name of the animation trigger for scene transition (if applicable)
    // public string transitionTriggerName = "StartTransition";

    // Duration of the transition animation (if not using an Animator)
    // public float transitionDuration = 1f;

    // Function to change scene
    public void ChangeScene(string sceneName, bool waitForAnimation = false)
    {
        Debug.Log($"ChangeScene called. Scene: {sceneName}, Wait for animation: {waitForAnimation}");
        
        if (waitForAnimation)
        {
            StartCoroutine(ChangeSceneWithTransition(sceneName));
        }
        else
        {
            Debug.Log($"Loading scene immediately: {sceneName}");
            SceneManager.LoadScene(sceneName);
        }
    }

    public void StartGame()
    {
        Debug.Log("StartGame called");
        SceneManager.LoadScene(1);
    }

    public void ShowOptions()
    {
        Debug.Log("ShowOptions called");
    }

    public void HideOptions()
    {
        Debug.Log("HideOptions called");
    }

    // Coroutine to handle scene transition with animation
    private IEnumerator ChangeSceneWithTransition(string sceneName)
    {
        Debug.Log($"Starting scene transition to: {sceneName}");

        // if (transitionAnimator != null)
        // {
        //     // Trigger the transition animation
        //     transitionAnimator.SetTrigger(transitionTriggerName);
        //
        //     // Wait for the animation to finish
        //     yield return new WaitForSeconds(transitionAnimator.GetCurrentAnimatorStateInfo(0).length);
        // }
        // else
        // {
        //     // If no animator is assigned, use a simple delay
        //     yield return new WaitForSeconds(transitionDuration);
        // }

        // For testing, we'll use a short delay
        yield return new WaitForSeconds(1f);

        Debug.Log($"Transition complete. Loading scene: {sceneName}");
        SceneManager.LoadScene(sceneName);
    }

    // Function to quit the game
    public void QuitGame()
    {
        Debug.Log("QuitGame called");
        
        /*
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif*/
    }

    // Function to toggle a GameObject (can be used for panels, options, etc.)
    public void ToggleGameObject(GameObject obj)
    {
        if (obj != null)
        {
            bool newState = !obj.activeSelf;
            obj.SetActive(newState);
            Debug.Log($"ToggleGameObject: {obj.name} is now {(newState ? "active" : "inactive")}");
        }
        else
        {
            Debug.LogWarning("ToggleGameObject: GameObject is null");
        }
    }

    // Function to play a sound effect (add AudioSource as needed)
    public void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            AudioSource.PlayClipAtPoint(clip, Camera.main.transform.position);
            Debug.Log($"PlaySound: Playing clip {clip.name}");
        }
        else
        {
            Debug.LogWarning("PlaySound: AudioClip is null");
        }
    }
}