using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimonTask : MonoBehaviour
{
    [SerializeField] GameObject[] buttons;
    [SerializeField] GameObject[] lightArray;
    [SerializeField] GameObject[] rowlights;
    [SerializeField] GameObject simonSaysGamePanel;
    [SerializeField] int[] lightSequence;
    int level = 0;
    int buttonsPressed = 0;
    int colorOrderRunCount = 0;
    bool passed = false;
    bool won = false;
    [SerializeField] private Color neutralColor = Color.white;
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color wrongColor = Color.red;
    [SerializeField] private Color invisibleColor = Color.clear;
    [SerializeField] private float lightSpeed;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip greenSound;    // Sound for correct sequence
    [SerializeField] private AudioClip wrongSound;    // Sound for wrong sequence
    [SerializeField] private AudioClip buttonSound;   // Sound for button press
    [SerializeField] private AudioClip orderSound;    // Sound for showing sequence

    void Start()
    {
    }

    void Update()
    {
    }

    private void OnEnable()
    {
        level = 0;
        buttonsPressed = 0;
        colorOrderRunCount = -1;
        won = false;
        for(int i = 0; i < lightSequence.Length; i++)
        {
            lightSequence[i] = (Random.Range(0, 8));
        }
        for(int i = 0; i < rowlights.Length; i++)
        {
            rowlights[i].GetComponent<Image>().color = neutralColor;
        }
        level = 1;
        StartCoroutine(ColorOrder());
    }

    public void ButtonClickOrder(int button)
    {
        // Play button click sound
        audioSource.PlayOneShot(buttonSound);
        
        buttonsPressed++;
        if(button == lightSequence[buttonsPressed - 1])
        {
            passed = true;
        }
        else
        {
            won = false;
            passed = false;
            // Play wrong sequence sound
            audioSource.PlayOneShot(wrongSound);
            StartCoroutine(ColorBlink(wrongColor));
        }
        if(buttonsPressed == level && passed && buttonsPressed != 5)
        {
            level++;
            passed = false;
            // Play correct sequence sound
            audioSource.PlayOneShot(greenSound);
            StartCoroutine(ColorOrder());   
        }
        if(buttonsPressed == level && passed == true && buttonsPressed == 5)
        {
            won = true;
            // Play correct sequence sound for winning
            audioSource.PlayOneShot(greenSound);
            StartCoroutine(ColorBlink(correctColor));
        }
    }

    public void ClosePanel()
    {
        simonSaysGamePanel.SetActive(false);
    }

    public void OpenPanel()
    {
        simonSaysGamePanel.SetActive(true);
    }

    IEnumerator ColorBlink(Color colorToBlink)
    {
        DisableInteractableButtons();
        for(int j = 0; j < 3; j++)
        {
            for(int i = 0; i < rowlights.Length; i++)
            {
                rowlights[i].GetComponent<Image>().color = colorToBlink;
            }
            for(int i = 5; i < rowlights.Length; i++)
            {
                rowlights[i].GetComponent<Image>().color = colorToBlink;
            }
            yield return new WaitForSeconds(lightSpeed);
            for(int i = 0; i < buttons.Length; i++)
            {
                rowlights[i].GetComponent<Image>().color = neutralColor;
            }
            for(int i = 5; i < rowlights.Length; i++)
            {
                rowlights[i].GetComponent<Image>().color = colorToBlink;
            }
        }
        if(won == true)
        {
            ClosePanel();
        }
        EnableInteractableButtons();
        OnEnable();
    }

    IEnumerator ColorOrder()
    {
        buttonsPressed = 0;
        colorOrderRunCount++;
        DisableInteractableButtons();
        for (int i = 0; i < level; i++)
        {
            if (level >= colorOrderRunCount)
            {
                // Play order sound when showing sequence
                audioSource.PlayOneShot(orderSound);
                
                rowlights[lightSequence[i]].GetComponent<Image>().color = invisibleColor;
                yield return new WaitForSeconds(lightSpeed);
                rowlights[lightSequence[i]].GetComponent<Image>().color = correctColor;
                yield return new WaitForSeconds(lightSpeed);
                rowlights[lightSequence[i]].GetComponent<Image>().color = invisibleColor;
                rowlights[i].GetComponent<Image>().color = correctColor;
            }
        }
        EnableInteractableButtons();
    }

    void DisableInteractableButtons()
    {
        foreach(GameObject buttonObj in buttons)
        {
            UnityEngine.UI.Button btn = buttonObj.GetComponent<UnityEngine.UI.Button>();
            if (btn != null)
            {
                btn.interactable = false;
            }
        }
    }

    void EnableInteractableButtons()
    {
        foreach(GameObject buttonObj in buttons)
        {
            UnityEngine.UI.Button btn = buttonObj.GetComponent<UnityEngine.UI.Button>();
            if (btn != null)
            {
                btn.interactable = true;
            }
        }
    }
}