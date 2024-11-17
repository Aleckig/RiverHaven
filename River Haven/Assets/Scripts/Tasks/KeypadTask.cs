using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeypadTask : MonoBehaviour
{
    [SerializeField] private TMP_Text cardCode; 
    [SerializeField] private TMP_Text inputCode;
    [SerializeField] private GameObject uiPanel; // Reference to the UI panel
    [SerializeField] private Image lightImage; // Reference to the light image
    [SerializeField] private Color neutralColor = Color.white; // Default light color
    [SerializeField] private Color correctColor = Color.green; // Light color for correct input
    [SerializeField] private Color wrongColor = Color.red; // Light color for wrong input
    [SerializeField] private TMP_Text taskName;
    [SerializeField] private string taskNameText = "Keypad Task";
    [SerializeField] private int codeLenght = 5;
    [SerializeField] private float codeResetTimeInSeconds = 0.5f;

    private bool isResetting = false;
    

    private void OnEnable()
    {
        // Set the task name
        if (taskName != null)
        {
            taskName.text = taskNameText;
        }
        string code = string.Empty;
        for (int i = 0; i < codeLenght; i++)
        {
            code += Random.Range(0, 10).ToString();
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

        inputCode.text += number;

        if (inputCode.text == cardCode.text)
        {
            inputCode.text = "Correct!";
            SetLightColor(correctColor);
            StartCoroutine(ResetCode());
        }
        else if (inputCode.text.Length == codeLenght)
        {
            inputCode.text = "Incorrect!";
            SetLightColor(wrongColor);
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

    
    public void CloseUI()
    {
        if (uiPanel != null)
        {
            uiPanel.SetActive(false); 
        }
    }
}
