using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;


public class Intro : MonoBehaviour
{
    // Array of pictures to cycle through
    public Sprite[] pictures;
    // Reference to the UI image component that displays the picture
    public Image pictureDisplay;
    // References to the buttons
    public UnityEngine.UI.Button nextButton;
    public UnityEngine.UI.Button  backButton;
    public UnityEngine.UI.Button skipButton;

    // Loading screen references
    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingBarFill;
    [SerializeField] private float loadingSpeed = 0.9f;

    // Index of the currently displayed picture
    private int currentIndex = 0;
    

    private void Start()
    {
        // Set the initial picture and button states
        UpdatePicture();
        UpdateButtons();

        // Add click listeners to the buttons
        nextButton.onClick.AddListener(NextPicture);
        backButton.onClick.AddListener(PreviousPicture);
        skipButton.onClick.AddListener(SkipScene);
    }

    private void NextPicture()
    {
        // Increment the picture index
        currentIndex++;
        UpdatePicture();
        UpdateButtons();

        // Automatically load the next scene if it's the last picture
        if (currentIndex == pictures.Length - 1)
        {
            SkipScene();
        }
    }

    private void PreviousPicture()
    {
        // Decrement the picture index
        currentIndex--;
        UpdatePicture();
        UpdateButtons();
    }

    private void UpdatePicture()
    {
        // Update the displayed picture and set its native size
        pictureDisplay.sprite = pictures[currentIndex];
        pictureDisplay.SetNativeSize();
    }

    private void UpdateButtons()
    {
        // Enable or disable the back button based on the current picture index
        backButton.interactable = currentIndex > 0;
        // Disable the next button if it’s the last picture
        nextButton.interactable = currentIndex < pictures.Length - 1;
    }

    private void SkipScene()
    {
        // Start loading the next scene
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        StartCoroutine(LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        // Show the loading screen
        loadingScreen.SetActive(true);

        // Start loading the scene asynchronously
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        // Update the loading bar with progress
        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / loadingSpeed);
            loadingBarFill.value = progressValue;
            yield return null;
        }

        // Set loading bar to full and hide the loading screen
        loadingBarFill.value = 1f;
        yield return new WaitForSeconds(0.2f);
        loadingScreen.SetActive(false);
    }
}