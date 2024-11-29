using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CoffeeTask : MonoBehaviour
{
    [System.Serializable]
    public class SliderControl
    {
        public Slider slider; // Slider UI element
        public UnityEngine.UI.Button increaseButton; // Button to increase value
        public UnityEngine.UI.Button decreaseButton; // Button to decrease value
        public TMP_Text targetValueText; // TextMeshPro Text displaying the target value
        public Image targetImage; // Visual indicator (e.g., arrow image pointing to the target)
        public Image completionImage; // Image to change color when the target is reached
        [HideInInspector] public int targetValue; // Random target value
        [HideInInspector] public bool canPress = true; // Flag to prevent rapid button presses
        public float buttonCooldown = 0.4f; // Cooldown time between button presses
    }

    public List<SliderControl> sliders; // List of sliders and their controls
    public GameObject completionPanel; // Panel to display when the task is complete

    void Start()
    {
        foreach (var sliderControl in sliders)
        {
            // Set slider properties
            sliderControl.slider.minValue = 0;
            sliderControl.slider.maxValue = 10;
            sliderControl.slider.wholeNumbers = true;
            sliderControl.slider.value = 0; // Start slider at 0

            // Assign random target value and update the UI
            sliderControl.targetValue = Random.Range(0, 11); // Target value between 0 and 10
            sliderControl.targetValueText.text = sliderControl.targetValue.ToString();

            // Position the target image
            UpdateTargetImagePosition(sliderControl);

            // Add button listeners
            var currentSliderControl = sliderControl;
            sliderControl.increaseButton.onClick.AddListener(() => StartCoroutine(ChangeSliderValueWithDelay(currentSliderControl, 1)));
            sliderControl.decreaseButton.onClick.AddListener(() => StartCoroutine(ChangeSliderValueWithDelay(currentSliderControl, -1)));

            // Initialize completion image color
            sliderControl.completionImage.color = Color.white;
        }

        // Ensure the completion panel is hidden at the start
        completionPanel.SetActive(false);
    }

    // Coroutine to handle delayed slider changes
    IEnumerator ChangeSliderValueWithDelay(SliderControl sliderControl, int change)
    {
        if (!sliderControl.canPress) yield break; // Prevent rapid presses

        sliderControl.canPress = false; // Disable further presses
        sliderControl.increaseButton.interactable = false;
        sliderControl.decreaseButton.interactable = false;

        ChangeSliderValue(sliderControl.slider, change);

        yield return new WaitForSeconds(sliderControl.buttonCooldown); // Wait for cooldown

        sliderControl.canPress = true; // Re-enable presses
        sliderControl.increaseButton.interactable = true;
        sliderControl.decreaseButton.interactable = true;
    }

    // Method to change slider value
    void ChangeSliderValue(Slider slider, int change)
    {
        float newValue = slider.value + change; // Add the change
        newValue = Mathf.Clamp(newValue, slider.minValue, slider.maxValue); // Clamp the value
        slider.value = newValue; // Set the new value

        // Update the completion image and check for task completion
        SliderControl sliderControl = sliders.Find(sc => sc.slider == slider);
        if (sliderControl != null)
        {
            UpdateCompletionImage(sliderControl);
        }

        CheckSliderCompletion();
    }

    // Update the position of the target image
    void UpdateTargetImagePosition(SliderControl sliderControl)
    {
        // Calculate normalized target position (0-1)
        float targetNormalized = Mathf.InverseLerp(sliderControl.slider.minValue, sliderControl.slider.maxValue, sliderControl.targetValue);

        // Get RectTransform components
        RectTransform sliderRect = sliderControl.slider.GetComponent<RectTransform>();
        RectTransform targetRect = sliderControl.targetImage.GetComponent<RectTransform>();

        // Position the target image based on normalized target value
        float newX = sliderRect.rect.xMin + (targetNormalized * sliderRect.rect.width);
        targetRect.anchoredPosition = new Vector2(newX, targetRect.anchoredPosition.y);
    }

    // Check if all sliders have reached their target values
    void CheckSliderCompletion()
    {
        foreach (var sliderControl in sliders)
        {
            if (Mathf.RoundToInt(sliderControl.slider.value) != sliderControl.targetValue)
            {
                return; // Exit if any slider is not at the target
            }
        }

        // All sliders are at their target values
        Debug.Log("Task Complete!");
        completionPanel.SetActive(true); // Show completion panel
    }

    // Update the completion image color
    void UpdateCompletionImage(SliderControl sliderControl)
    {
        if (Mathf.RoundToInt(sliderControl.slider.value) == sliderControl.targetValue)
        {
            sliderControl.completionImage.color = Color.green; // Change to green if target is reached
        }
        else
        {
            sliderControl.completionImage.color = Color.white; // Reset to white otherwise
        }
    }
}
