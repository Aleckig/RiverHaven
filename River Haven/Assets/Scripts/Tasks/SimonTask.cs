using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem;

public class SimonTask : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;
    [SerializeField] private GameObject[] lightArray;
    [SerializeField] private GameObject[] rowlights;
    [SerializeField] private GameObject[] completedRowlights;
    [SerializeField] private GameObject simonSaysGamePanel;
    [SerializeField] private GameObject closeTaskMarker;
    [SerializeField] private int[] lightSequence;
    [SerializeField] private int maxLevel = 4;

    int level = 0;
    int buttonsPressed = 0;
    bool passed = false;
    bool won = false;

    [SerializeField] private Color neutralColor = Color.white;
    [SerializeField] private Color correctColor = Color.green;
    [SerializeField] private Color wrongColor = Color.red;
    [SerializeField] private Color invisibleColor = Color.clear;

    [SerializeField] private float lightSpeed;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip correctSound;
    [SerializeField] private AudioClip wrongSound;
    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioClip orderSound;

    private void OnEnable()
    {
        ResetGame();
    }

    public void ButtonClickOrder(int button)
    {
        if (this == null || !gameObject.activeInHierarchy)
            return;

        Debug.Log($"Button clicked: {button}");

        if (buttonsPressed >= lightSequence.Length)
        {
            Debug.LogError("Button clicked out of sequence bounds.");
            return;
        }

        audioSource.PlayOneShot(buttonSound);
        buttonsPressed++;

        if (button == lightSequence[buttonsPressed - 1])
        {
            passed = true;
            Debug.Log($"Correct button! Button {button} is correct.");
            rowlights[buttonsPressed - 1].GetComponent<Image>().color = correctColor;

            if (buttonsPressed == lightSequence.Length)
            {
                Debug.Log($"Level {level + 1} complete!");

                if (level == maxLevel)
                {
                    won = true;
                    QuestLog.SetQuestState("Fix The Radio Tower", QuestState.Success);
                    DialogueLua.SetVariable("ActivitiesCompleted", DialogueLua.GetVariable("ActivitiesCompleted").AsInt + 1);
                    closeTaskMarker.SetActive(false);
                    audioSource.PlayOneShot(correctSound);
                    StartCoroutine(ColorBlink(correctColor));
                    return;
                }
                else
                {
                    StartCoroutine(NextLevelAfterDelay(1.5f)); // ⏳ Added delay before next level
                }
            }
        }
        else
        {
            won = false;
            passed = false;
            audioSource.PlayOneShot(wrongSound);
            StartCoroutine(ColorBlink(wrongColor));
            Debug.Log($"Incorrect button! Expected {lightSequence[buttonsPressed - 1]}, but got {button}.");
        }
    }

    IEnumerator NextLevelAfterDelay(float delay) // ✅ NEW coroutine for delay
    {
        DisableInteractableButtons();
        yield return new WaitForSeconds(delay);
        level++;
        StartCoroutine(ColorOrder());
    }

    public void ClosePanel()
    {
        if (simonSaysGamePanel != null)
            simonSaysGamePanel.SetActive(false);
    }

    public void OpenPanel()
    {
        if (simonSaysGamePanel != null)
            simonSaysGamePanel.SetActive(true);
    }

    IEnumerator ColorBlink(Color colorToBlink)
    {
        if (!gameObject.activeInHierarchy || simonSaysGamePanel == null)
            yield break;

        DisableInteractableButtons();

        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < lightArray.Length; i++)
                lightArray[i].GetComponent<Image>().color = colorToBlink;

            yield return new WaitForSeconds(lightSpeed);

            for (int i = 0; i < lightArray.Length; i++)
                lightArray[i].GetComponent<Image>().color = invisibleColor;

            yield return new WaitForSeconds(lightSpeed);
        }

        if (won)
        {
            ClosePanel();
            DialogueManager.ShowAlert("You fixed the radio tower!");
        }

        EnableInteractableButtons();
        ResetGame();
    }

    IEnumerator ColorOrder()
    {
        if (!gameObject.activeInHierarchy)
            yield break;

        buttonsPressed = 0;
        passed = false;
        DisableInteractableButtons();

        lightSequence = new int[level + 1];
        for (int i = 0; i <= level; i++)
        {
            lightSequence[i] = Random.Range(0, lightArray.Length);
        }

        for (int i = 0; i < lightSequence.Length; i++)
        {
            audioSource.PlayOneShot(orderSound);
            lightArray[lightSequence[i]].GetComponent<Image>().color = correctColor;
            yield return new WaitForSeconds(lightSpeed);
            lightArray[lightSequence[i]].GetComponent<Image>().color = invisibleColor;

            for (int j = 0; j <= i; j++)
                rowlights[j].GetComponent<Image>().color = correctColor;

            yield return new WaitForSeconds(lightSpeed);
        }

        for (int i = 0; i <= level; i++)
            completedRowlights[i].GetComponent<Image>().color = correctColor;

        EnableInteractableButtons();
    }

    void DisableInteractableButtons()
    {
        if (buttons != null)
        {
            foreach (GameObject buttonObj in buttons)
                if (buttonObj != null)
                    buttonObj.SetActive(false);
        }
    }

    void EnableInteractableButtons()
    {
        if (buttons != null)
        {
            foreach (GameObject buttonObj in buttons)
                if (buttonObj != null)
                    buttonObj.SetActive(true);
        }
    }

    private void ResetGame()
    {
        level = 0;
        buttonsPressed = 0;
        passed = false;
        won = false;

        if (rowlights != null)
        {
            for (int i = 0; i < rowlights.Length; i++)
                if (rowlights[i] != null)
                    rowlights[i].GetComponent<Image>().color = neutralColor;
        }

        if (completedRowlights != null)
        {
            for (int i = 0; i < completedRowlights.Length; i++)
                if (completedRowlights[i] != null)
                    completedRowlights[i].GetComponent<Image>().color = neutralColor;
        }

        if (gameObject.activeInHierarchy && simonSaysGamePanel != null)
            StartCoroutine(ColorOrder());
    }
}
