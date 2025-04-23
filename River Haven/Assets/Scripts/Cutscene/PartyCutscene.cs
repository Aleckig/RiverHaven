using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;
using TMPro; // Required for TextMeshPro

public class PartyCutscene : MonoBehaviour
{
    [Header("Player & Scene Objects")]
    [SerializeField] private Transform playerTransformation;
    [SerializeField] private Transform destination;
    [SerializeField] private GameObject playerObject;
    [SerializeField] private GameObject canvasObject;
    [SerializeField] private GameObject objectToDisable1;
    //[SerializeField] private GameObject objectToDisable2;
    [SerializeField] private GameObject objectToActivate;
    [SerializeField] private GameObject permanentlyDisabledObject1;
    [SerializeField] private GameObject permanentlyDisabledObject2;
    [SerializeField] private GameObject objectToEnable1;
    [SerializeField] private GameObject objectToEnable2;
    [SerializeField] private bool isParty;

    [Header("Cutscene Slides")]
    public Sprite[] pictures;
    [TextArea(2, 5)]
    public string[] slideTexts; // Custom text for each slide
    public Image pictureDisplay;
    public TextMeshProUGUI slideText; // TextMeshPro text display

    [Header("UI Buttons")]
    public UnityEngine.UI.Button nextButton;
    public UnityEngine.UI.Button backButton;
    public UnityEngine.UI.Button skipButton;

    [Header("Dialogue System")]
    public string variableName = "PartyFinished";

    private int currentIndex = 0;
    private bool isCutsceneActive = false;

    private void Start()
    {
        nextButton.onClick.AddListener(NextPicture);
        backButton.onClick.AddListener(PreviousPicture);
        skipButton.onClick.AddListener(SkipCutscene);
        canvasObject.SetActive(false); // Ensure the canvas is initially disabled
    }
    private void Update()
    {
        if (!isCutsceneActive) return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            PreviousPicture();
        }

        if (Input.GetKeyDown(KeyCode.D))
        {
            NextPicture();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SkipCutscene();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        Debug.Log("OnTriggerEnter called with " + other.gameObject.name);
        if (other.CompareTag("Player"))
        {
            StartCutscene();
        }
        else
        {
            Debug.Log("Collided object is not the player.");
        }
    }

    private void StartCutscene()
    {
        isCutsceneActive = true;
        playerObject.SetActive(false);
        playerTransformation.position = destination.position;
        playerObject.SetActive(true);
        if (isParty == true)
        {
            playerObject.GetComponent<IndoorTracker>().isIndoors = true;
            playerObject.GetComponent<IndoorTracker>().isInNGO = false;
            QuestLog.SetQuestState("Mystery Mail", QuestState.Active);
        }
        canvasObject.SetActive(true);

        objectToDisable1.SetActive(false);
        //objectToDisable2.SetActive(false);
        objectToActivate.SetActive(true);
        permanentlyDisabledObject1.SetActive(false);
        permanentlyDisabledObject2.SetActive(false);

        UpdatePicture();
        UpdateButtons();
        DisablePlayerControls();
    }

    private void UpdatePicture()
    {
        // Update the image
        pictureDisplay.sprite = pictures[currentIndex];
        pictureDisplay.SetNativeSize();

        // Update the text
        if (currentIndex < slideTexts.Length)
        {
            slideText.text = slideTexts[currentIndex];
        }
        else
        {
            slideText.text = ""; // Fallback in case text array is shorter
        }
    }

    private void UpdateButtons()
    {
        backButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < pictures.Length - 1;
    }

    private void NextPicture()
    {
        currentIndex++;
        if (currentIndex >= pictures.Length)
        {
            SkipCutscene();
            return;
        }
        UpdatePicture();
        UpdateButtons();
    }

    private void PreviousPicture()
    {
        if (currentIndex > 0)
        {
            currentIndex--;
            UpdatePicture();
            UpdateButtons();
        }
    }

    private void SkipCutscene()
    {
        isCutsceneActive = false;
        canvasObject.SetActive(false);

        objectToDisable1.SetActive(true);
        //objectToDisable2.SetActive(true);
        objectToActivate.SetActive(false);

        EnablePlayerControls();

        objectToEnable1.SetActive(true);
        objectToEnable2.SetActive(true);

        DialogueManager.ShowAlert("Go outside and check your mailbox.");
        ChangeBoolean(variableName, true);
    }

    private void DisablePlayerControls()
    {
        // Disable the player's movement and input
        // (You'll need to implement this based on your player controller setup)
    }

    private void EnablePlayerControls()
    {
        // Re-enable the player's movement and input
        // (You'll need to implement this based on your player controller setup)
    }

    private void ChangeBoolean(string varName, bool value)
    {
        DialogueLua.SetVariable(varName, value);
    }
}
