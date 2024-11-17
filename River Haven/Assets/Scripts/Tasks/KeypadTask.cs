using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class KeypadTask : MonoBehaviour
{
    [SerializeField] private TMP_Text cardCode; 
    [SerializeField] private TMP_Text inputCode;
    [SerializeField] private int codeLenght = 5;
    [SerializeField] private float codeResetTimeInSeconds = 0.5f;
    
    private bool isResetting = false;

    private void OnEnable()
    {
        string code = string.Empty;
        for (int i = 0; i < codeLenght; i++)
        {
            code += Random.Range(0, 10).ToString();
        }

        cardCode.text = code;
        inputCode.text = string.Empty;
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
            StartCoroutine(ResetCode());
            
        }
        else if (inputCode.text.Length == codeLenght)
        {
            inputCode.text = "Wrong!";
            StartCoroutine(ResetCode());
        }
         
    }

    private IEnumerator ResetCode()
    {
        isResetting = true;
        yield return new WaitForSeconds(codeResetTimeInSeconds);
        inputCode.text = string.Empty;
        isResetting = false;
    }


}
