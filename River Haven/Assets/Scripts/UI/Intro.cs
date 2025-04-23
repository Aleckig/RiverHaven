using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Intro : MonoBehaviour
{
    public Sprite[] pictures;
    public Image pictureDisplay;
    public UnityEngine.UI.Button nextButton;
    public UnityEngine.UI.Button backButton;
    public UnityEngine.UI.Button skipButton;

    [SerializeField] private GameObject loadingScreen;
    [SerializeField] private Slider loadingBarFill;
    [SerializeField] private float loadingSpeed = 0.9f;

    private int currentIndex = 0;

    private void Start()
    {
        UpdatePicture();
        UpdateButtons();

        nextButton.onClick.AddListener(NextPicture);
        backButton.onClick.AddListener(PreviousPicture);
        skipButton.onClick.AddListener(SkipScene);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.A))
        {
            if (currentIndex > 0)
                PreviousPicture();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            if (currentIndex < pictures.Length - 1)
                NextPicture();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkipScene();
        }
    }

    private void NextPicture()
    {
        currentIndex++;
        UpdatePicture();
        UpdateButtons();

        if (currentIndex == pictures.Length - 1)
        {
            SkipScene();
        }
    }

    private void PreviousPicture()
    {
        currentIndex--;
        UpdatePicture();
        UpdateButtons();
    }

    private void UpdatePicture()
    {
        pictureDisplay.sprite = pictures[currentIndex];
        pictureDisplay.SetNativeSize();
    }

    private void UpdateButtons()
    {
        backButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < pictures.Length - 1;
    }

    private void SkipScene()
    {
        LoadNextScene();
    }

    private void LoadNextScene()
    {
        StartCoroutine(LoadSceneAsync(SceneManager.GetActiveScene().buildIndex + 1));
    }

    private IEnumerator LoadSceneAsync(int sceneIndex)
    {
        loadingScreen.SetActive(true);
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            float progressValue = Mathf.Clamp01(operation.progress / loadingSpeed);
            loadingBarFill.value = progressValue;
            yield return null;
        }

        loadingBarFill.value = 1f;
        yield return new WaitForSeconds(0.2f);
        loadingScreen.SetActive(false);
    }
}
