using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SimonTask : MonoBehaviour
{
    [SerializeField] private GameObject[] buttons;          // The button objects
    [SerializeField] private GameObject[] lightArray;       // The lights for showing the sequence to the player
    [SerializeField] private GameObject[] rowlights;        // The rowlights to indicate the progress (left = progress, right = status)
    [SerializeField] private GameObject[] completedRowlights; // New array for completed lights (right side)
    [SerializeField] private GameObject simonSaysGamePanel;
    [SerializeField] private GameObject closeTaskMarker; // Reference to the close task marker
    [SerializeField] int[] lightSequence;           // The sequence of lights that the player needs to follow
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
        // Reset game state
        ResetGame();
    }

    public void ButtonClickOrder(int button)
    {
        if (this == null || !gameObject.activeInHierarchy)
            return;

        Debug.Log($"Button clicked: {button}");
    
        // Check if we are within bounds for the sequence
        if (buttonsPressed >= lightSequence.Length)
        {
            Debug.LogError("Button clicked out of sequence bounds.");
            return;
        }

        // Play button click sound
        audioSource.PlayOneShot(buttonSound);
        buttonsPressed++;

        // Check if the button clicked matches the expected sequence
        if (button == lightSequence[buttonsPressed - 1])
        {
            passed = true;
            Debug.Log($"Correct button! Button {button} is correct.");

            // Indicate correct button press in the rowlights (on the left side)
            rowlights[buttonsPressed - 1].GetComponent<Image>().color = correctColor;

            // If we've pressed all buttons for the current level correctly
            if (buttonsPressed == lightSequence.Length)
            {
                Debug.Log($"Level {level + 1} complete!");
                // After all buttons are pressed correctly, move to the next level
                if (level == 4) // Assuming 5 is the max level
                {
                    // Set won to true after completing level 5
                    won = true;
                    closeTaskMarker.SetActive(false); // Enable the close task marker
                    audioSource.PlayOneShot(correctSound); // Play the correct sequence sound
                    StartCoroutine(ColorBlink(correctColor)); // Blink green to indicate the win
                    return; // Stop the game after winning the 5th level
                    
                }
                else
                {
                    level++;
                    StartCoroutine(ColorOrder()); // Show the next sequence for the next level
                }
            }
        }
        else
        {
            won = false;
            passed = false;
            // Play wrong sequence sound
            audioSource.PlayOneShot(wrongSound);
            StartCoroutine(ColorBlink(wrongColor)); // Indicate wrong attempt (red blink)
            Debug.Log($"Incorrect button! Expected {lightSequence[buttonsPressed - 1]}, but got {button}.");
        }
    }


    public void ClosePanel()
    {
        if (simonSaysGamePanel != null)
        {
            simonSaysGamePanel.SetActive(false); // Disable only the panel, not the entire GameObject
        }
    }

    public void OpenPanel()
    {
        if (simonSaysGamePanel != null)
        {
            simonSaysGamePanel.SetActive(true); // Enable the panel again when the task is restarted
        }
    }

    IEnumerator ColorBlink(Color colorToBlink)
    {
        // Prevent coroutine from running if SimonTask is deactivated
        if (!gameObject.activeInHierarchy || simonSaysGamePanel == null)
            yield break;

        DisableInteractableButtons(); // Disable buttons while blinking

        // Blink the lights for a few seconds
        for (int j = 0; j < 3; j++)
        {
            for (int i = 0; i < lightArray.Length; i++) // Blink the lightArray sequence
            {
                lightArray[i].GetComponent<Image>().color = colorToBlink;
            }
            yield return new WaitForSeconds(lightSpeed);

            for (int i = 0; i < lightArray.Length; i++) // Reset lights to neutral
            {
                lightArray[i].GetComponent<Image>().color = invisibleColor;
            }
            yield return new WaitForSeconds(lightSpeed);
        }

        // Check if the game was won and close the panel
        if (won)
        {
            ClosePanel(); // Close the panel if the game is won
        }
        EnableInteractableButtons();
        ResetGame(); // Restart the game after the blink
    }

    IEnumerator ColorOrder()
    {
        // Prevent running the coroutine if the GameObject is deactivated
        if (!gameObject.activeInHierarchy)
            yield break;

        buttonsPressed = 0; // Reset the button presses for the new level
        passed = false; // Reset the passed state
        DisableInteractableButtons(); // Disable the buttons while showing the sequence

        // Ensure lightSequence has a valid length before showing the sequence
        lightSequence = new int[level + 1]; 
        for (int i = 0; i <= level; i++)
        {
            lightSequence[i] = Random.Range(0, lightArray.Length); // Ensure valid index within lightArray length
        }

        for (int i = 0; i < lightSequence.Length; i++)
        {
            // Show the light sequence, one at a time using lightArray (for the player to memorize)
            audioSource.PlayOneShot(orderSound); // Play sound to indicate sequence

            // Show the light from lightArray in the sequence
            lightArray[lightSequence[i]].GetComponent<Image>().color = correctColor;
            yield return new WaitForSeconds(lightSpeed);
            lightArray[lightSequence[i]].GetComponent<Image>().color = invisibleColor; // Turn off the light

            // Update the row lights (for progress indication on the left side)
            for (int j = 0; j <= i; j++)
            {
                rowlights[j].GetComponent<Image>().color = correctColor; // Turn on progress indicator light
            }
            yield return new WaitForSeconds(lightSpeed);
        }

        // Update the completed row lights (for completed level indication on the right side)
        for (int i = 0; i <= level; i++)
        {
            completedRowlights[i].GetComponent<Image>().color = correctColor; // Turn on completed indicator light
        }

        EnableInteractableButtons(); // Re-enable the buttons after the sequence is shown
    }

    void DisableInteractableButtons()
    {
        if (buttons != null)
        {
            foreach (GameObject buttonObj in buttons)
            {
                if (buttonObj != null)
                {
                    buttonObj.SetActive(false); // Disable the button GameObject
                }
            }
        }
    }

    void EnableInteractableButtons()
    {
        if (buttons != null)
        {
            foreach (GameObject buttonObj in buttons)
            {
                if (buttonObj != null)
                {
                    buttonObj.SetActive(true); // Enable the button GameObject
                }
            }
        }
    }

    private void ResetGame()
    {
        // Reset all game states
        level = 0; // Ensure the game starts from level 1
        buttonsPressed = 0;
        passed = false;
        won = false;

        // Reset rowlights and completed rowlights
        if (rowlights != null)
        {
            for (int i = 0; i < rowlights.Length; i++)
            {
                if (rowlights[i] != null)
                    rowlights[i].GetComponent<Image>().color = neutralColor; // Reset rowlights to neutral color
            }
        }

        if (completedRowlights != null)
        {
            for (int i = 0; i < completedRowlights.Length; i++)
            {
                if (completedRowlights[i] != null)
                    completedRowlights[i].GetComponent<Image>().color = neutralColor; // Reset completed rowlights to neutral color
            }
        }

        // Only start ColorOrder if the GameObject is active
        if (gameObject.activeInHierarchy && simonSaysGamePanel != null)
        {
            StartCoroutine(ColorOrder()); // Start showing the color sequence for the first level
        }
    }
}
