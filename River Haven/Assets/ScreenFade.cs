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
    private bool showTheAlert = true;
    private bool teleportPlayer = false;
    private string alertMessage;
    [SerializeField] private Transform playerTransform;
    [SerializeField] private Transform destination;
    [SerializeField] private Transform homeTransform;
    [SerializeField] private Transform NGOTransform;
    [SerializeField] private Transform CommunityBoardTransform;
    [SerializeField] private Transform StoreTransform;
    [SerializeField] private Transform RyanTransform;
    [SerializeField] private Transform speechTransform;
    [SerializeField] private GameObject truckPainting;
    private bool isRyanQuest = false;
    [SerializeField] private GameObject player;

    // Call this function to trigger the fade effect, stay black, and fade back in
    public void TriggerFadeAndAlert()
    {
        alertMessage = "You feel rested. Go talk to Mai, she has some information for you.";
        if (!isFading)
        {
            StartCoroutine(FadeSequence());
        }
    }

    public void TriggerFadeRyanVersion()
    {
        showTheAlert = true;
        isRyanQuest = true;
        alertMessage = "Ryan finished painting the truck. Go talk to him.";
        if (!isFading)
        {
            StartCoroutine(FadeSequence());
        }
    }

    public void TriggerFadeAndTeleportHome()
    {
        showTheAlert = false;
        teleportPlayer = true;
        destination = homeTransform;
        if (!isFading)
        {
            StartCoroutine(FadeSequence());
        }
        if (player != null)
        {
            player.GetComponent<IndoorTracker>().isIndoors = false;
            player.GetComponent<IndoorTracker>().isInNGO = false;
        }
    }

    public void TriggerFadeAndTeleportToNGO()
    {
        showTheAlert = false;
        teleportPlayer = true;
        destination = NGOTransform;
        if (!isFading)
        {
            StartCoroutine(FadeSequence());
        }
        if (player != null)
        {
            player.GetComponent<IndoorTracker>().isIndoors = true;
            player.GetComponent<IndoorTracker>().isInNGO = true;
        }
    }

    public void TriggerFadeAndTeleportToRyan()
    {
        showTheAlert = false;
        teleportPlayer = true;
        destination = RyanTransform;
        if (!isFading)
        {
            StartCoroutine(FadeSequence());
        }
        if (player != null)
        {
            player.GetComponent<IndoorTracker>().isIndoors = false;
            player.GetComponent<IndoorTracker>().isInNGO = false;
        }
    }

    public void TriggerFadeAndTeleportToCommunityBoard()
    {
        showTheAlert = true;
        teleportPlayer = true;
        destination = CommunityBoardTransform;
        alertMessage = "Put the last poster on the community board.";
        if (!isFading)
        {
            StartCoroutine(FadeSequence());
        }
        if (player != null)
        {
            player.GetComponent<IndoorTracker>().isIndoors = false;
            player.GetComponent<IndoorTracker>().isInNGO = false;
        }
    }

    public void TriggerFadeAndTeleportToStore()
    {
        showTheAlert = true;
        teleportPlayer = true;
        destination = StoreTransform;
        alertMessage = "Go to the store and find the ingredients";
        if (!isFading)
        {
            StartCoroutine(FadeSequence());
        }
        if (player != null)
        {
            player.GetComponent<IndoorTracker>().isIndoors = true;
            player.GetComponent<IndoorTracker>().isInNGO = false;
        }
    }

    public void TriggerFadeAndTeleportSpeech()
    {
        showTheAlert = false;
        teleportPlayer = true;
        destination = speechTransform;
        if (!isFading)
        {
            StartCoroutine(FadeSequence());
        }
        if (player != null)
        {
            player.GetComponent<IndoorTracker>().isIndoors = false;
            player.GetComponent<IndoorTracker>().isInNGO = false;
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
        TeleportPlayer();
        if (isRyanQuest == true)
        {
            truckPainting.gameObject.SetActive(true);
            isRyanQuest = false;
        }

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
        if (showTheAlert == true)
        {
            DialogueManager.ShowAlert(alertMessage);
        }
    }

    private void TeleportPlayer()
    {
        if (teleportPlayer == true)
        {
            playerTransform.position = destination.position;
        }
    }
}
