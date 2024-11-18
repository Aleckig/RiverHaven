using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeypadTask : MonoBehaviour
{
    [SerializeField] private TMP_Text cardCode; 
    [SerializeField] private TMP_Text inputCode;
    [SerializeField] private GameObject uiPanel; // Reference to the UI panel
    [SerializeField] private GameObject closeTaskMarker; // Reference to the close task marker
    [SerializeField] private Image lightImage; // Reference to the light image
    [SerializeField] private Color neutralColor = Color.white; // Default light color
    [SerializeField] private Color correctColor = Color.green; // Light color for correct input
    [SerializeField] private Color wrongColor = Color.red; // Light color for wrong input
    [SerializeField] private TMP_Text taskName;
    [SerializeField] private string taskNameText = "Keypad Task";
    [SerializeField] private int codeLenght = 5;
    [SerializeField] private float codeResetTimeInSeconds = 0.5f;
    [SerializeField] private float endTaskTime = 1.5f;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip wrongSound;
    [SerializeField] private AudioClip buttonSound;

    private bool isResetting = false;

    private void OnEnable()
    {
        // Set the task name
        if (taskName != null)
        {
            taskName.text = taskNameText;
        }
        
        // Generate a random code
        string code = string.Empty;
        for (int i = 0; i < codeLenght; i++)
        {
            code += Random.Range(1, 10).ToString();
        }

        cardCode.text = code;
        inputCode.text = string.Empty;

        if (lightImage != null)
        {
            lightImage.color = neutralColor; // Reset light to neutral
        }
    }

    public void ButtonClick(int number)
    {
        if (isResetting)
        {
            return;
        }

        // Play button click sound
        PlaySound(buttonSound);

        // Append the number to the input code
        inputCode.text += number;

        if (inputCode.text == cardCode.text)
        {
            SetLightColor(correctColor);
            PlaySound(correctSound); // Play correct sound
            
            StartCoroutine(ResetCode());
            StartCoroutine(EndTask()); // Handle delayed deactivation here
        }
        else if (inputCode.text.Length == codeLenght)
        {
            inputCode.text = "Wrong!";
            SetLightColor(wrongColor);
            PlaySound(wrongSound); // Play wrong sound
            StartCoroutine(ResetCode());
        }
    }

    private IEnumerator ResetCode()
    {
        isResetting = true;
        yield return new WaitForSeconds(codeResetTimeInSeconds);
        inputCode.text = string.Empty;
        SetLightColor(neutralColor); 
        isResetting = false;
    }

    private void SetLightColor(Color color)
    {
        if (lightImage != null)
        {
            lightImage.color = color;
        }
    }

    private IEnumerator EndTask()
    {
        yield return new WaitForSeconds(endTaskTime); // Delay before deactivating the UI
        CloseUI();
    }

    public void CloseUI()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false);
            closeTaskMarker.SetActive(false); //close task marker
        }
    }

    private void PlaySound(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip); // Play the sound once
        }
    }
}
