using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using PixelCrushers.DialogueSystem; // For Dialogue System Lua scripting

public class ScreenFade : MonoBehaviour
{
    public Image fadeImage;       // Reference to the Image component used for the fade effect
    public float fadeDuration = 1f; // Duration of the fade in seconds
    public float stayBlackDuration = 1f; // Duration for how long the screen stays black
    //public string alertMessage = "This is an alert!"; // The message for the alert

    private bool isFading = false;

    // Call this function to trigger the fade effect, stay black, and fade back in
    public void TriggerFadeAndAlert()
    {
        if (!isFading)
        {
            StartCoroutine(FadeSequence());
        }
    }

    // Coroutine to handle the fade to black, stay black, and fade back in
    private IEnumerator FadeSequence()
    {
        isFading = true;

        // Fade to black
        yield return StartCoroutine(FadeToColor(new Color(0, 0, 0, 1))); // Fade to black (alpha = 1)

        // Wait for the specified time while the screen stays black
        yield return new WaitForSeconds(stayBlackDuration);

        // Trigger the alert using Lua scripting from the Dialogue System
        TriggerAlertLua();

        // Fade back to transparent (normal)
        yield return StartCoroutine(FadeToColor(new Color(0, 0, 0, 0))); // Fade back to transparent (alpha = 0)

        fadeImage.gameObject.SetActive(false);
        isFading = false;
    }

    // Coroutine to fade the image to the target color over time
    private IEnumerator FadeToColor(Color targetColor)
    {
        // Ensure the fadeImage is active before starting the fade
        fadeImage.gameObject.SetActive(true);

        // Get the current color of the fade image
        Color initialColor = fadeImage.color;
        float elapsedTime = 0f;

        // Gradually change the alpha value to create the fade effect
        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            fadeImage.color = Color.Lerp(initialColor, targetColor, elapsedTime / fadeDuration);
            yield return null;
        }

        // Ensure the color is exactly as intended
        fadeImage.color = targetColor;
    }

    // Function to trigger a new alert using the Unity Dialogue System's Lua scripting
    private void TriggerAlertLua()
    {
        // Assuming you have set up the Lua function in the Dialogue System
        // This will trigger a Lua statement that can be hooked into your alert system
        DialogueManager.ShowAlert("You feel rested. Go talk to Mai, she has some information for you.");
    }
}
