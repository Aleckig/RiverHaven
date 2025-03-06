using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

public class PartyCutscene : MonoBehaviour
{
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

    public Sprite[] pictures;
    public Image pictureDisplay;
    public UnityEngine.UI.Button nextButton;
    public UnityEngine.UI.Button backButton;
    public UnityEngine.UI.Button skipButton;

    private int currentIndex = 0;
    private bool isCutsceneActive = false;

    public string variableName = "PartyFinished";

    private void Start()
    {
        nextButton.onClick.AddListener(NextPicture);
        backButton.onClick.AddListener(PreviousPicture);
        skipButton.onClick.AddListener(SkipCutscene);
        canvasObject.SetActive(false); // Ensure the canvas is initially disabled
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
        pictureDisplay.sprite = pictures[currentIndex];
        pictureDisplay.SetNativeSize();
    }

    private void UpdateButtons()
    {
        backButton.interactable = currentIndex > 0;
        nextButton.interactable = currentIndex < pictures.Length - 1;
    }

    private void NextPicture()
    {
        currentIndex++;
        UpdatePicture();
        UpdateButtons();
        // If we've reached the last picture, skip the cutscene
        if (currentIndex == pictures.Length)
        {
            SkipCutscene();
        }
    }

    private void PreviousPicture()
    {
        currentIndex--;
        UpdatePicture();
        UpdateButtons();
    }

    private void SkipCutscene()
    {
        isCutsceneActive = false;
        canvasObject.SetActive(false);
        objectToDisable1.SetActive(true);
        //objectToDisable2.SetActive(true);
        objectToActivate.SetActive(false);
        //permanentlyDisabledObject1.SetActive(false);
        //permanentlyDisabledObject2.SetActive(false);
        EnablePlayerControls();
        objectToEnable1.SetActive(true);
        objectToEnable2.SetActive(true);
        DialogueManager.ShowAlert("Go talk to Layla at the party scene");
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

    void ChangeBoolean(string varName, bool value)
    {
        DialogueLua.SetVariable(varName, true);
    }
}