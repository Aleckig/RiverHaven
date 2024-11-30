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
        public Slider slider;
        public UnityEngine.UI.Button increaseButton;
        public UnityEngine.UI.Button decreaseButton;
        public TMP_Text targetValueText;
        public Image targetImage;
        public Image completionImage;
        [HideInInspector] public int targetValue;
        [HideInInspector] public bool canPress = true;
        public float buttonCooldown = 0.4f;
    }

    [Header("UI Elements")]
    public List<SliderControl> sliders;
    public GameObject completionPanel;
    public UnityEngine.UI.Button brewButton;
    public UnityEngine.UI.Button closeButton;

    [Header("Audio")]
    public AudioSource audioSource;
    public AudioClip buttonClickSound;
    public AudioClip targetReachedSound;
    public AudioClip brewingSound;

    [Header("CloseTask")]
    [SerializeField] private GameObject closeTask;

    private int completedSliders = 0;

    void Start()
    {
        // Initialize audio source if not already assigned
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }

        // Hide brew button at start
        brewButton.gameObject.SetActive(false);

        foreach (var sliderControl in sliders)
        {
            // Set slider properties
            sliderControl.slider.minValue = 0;
            sliderControl.slider.maxValue = 10;
            sliderControl.slider.wholeNumbers = true;
            sliderControl.slider.value = 0;

            sliderControl.targetValue = Random.Range(0, 11);
            sliderControl.targetValueText.text = sliderControl.targetValue.ToString();

            UpdateTargetImagePosition(sliderControl);

            var currentSliderControl = sliderControl;
            sliderControl.increaseButton.onClick.AddListener(() => {
                PlayButtonClickSound();
                StartCoroutine(ChangeSliderValueWithDelay(currentSliderControl, 1));
            });
            sliderControl.decreaseButton.onClick.AddListener(() => {
                PlayButtonClickSound();
                StartCoroutine(ChangeSliderValueWithDelay(currentSliderControl, -1));
            });

            sliderControl.completionImage.color = Color.white;
        }

        // Add listeners for brew and close buttons
        brewButton.onClick.AddListener(BrewCoffee);
        closeButton.onClick.AddListener(ClosePanel);

        completionPanel.SetActive(false);
    }

    void PlayButtonClickSound()
    {
        if (buttonClickSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(buttonClickSound);
        }
    }

    void PlayTargetReachedSound()
    {
        if (targetReachedSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(targetReachedSound);
        }
    }

    IEnumerator ChangeSliderValueWithDelay(SliderControl sliderControl, int change)
    {
        if (!sliderControl.canPress) yield break;

        sliderControl.canPress = false;
        sliderControl.increaseButton.interactable = false;
        sliderControl.decreaseButton.interactable = false;

        ChangeSliderValue(sliderControl.slider, change);

        yield return new WaitForSeconds(sliderControl.buttonCooldown);

        sliderControl.canPress = true;
        sliderControl.increaseButton.interactable = true;
        sliderControl.decreaseButton.interactable = true;
    }

    void ChangeSliderValue(Slider slider, int change)
    {
        float newValue = slider.value + change;
        newValue = Mathf.Clamp(newValue, slider.minValue, slider.maxValue);
        slider.value = newValue;

        SliderControl sliderControl = sliders.Find(sc => sc.slider == slider);
        if (sliderControl != null)
        {
            bool wasCompleted = sliderControl.completionImage.color == Color.green;
            UpdateCompletionImage(sliderControl);
            
            // Check if this slider just reached its target
            if (!wasCompleted && sliderControl.completionImage.color == Color.green)
            {
                PlayTargetReachedSound();
                completedSliders++;
                CheckBrewButtonStatus();
            }
            // Check if slider moved away from target
            else if (wasCompleted && sliderControl.completionImage.color != Color.green)
            {
                completedSliders--;
                CheckBrewButtonStatus();
            }
        }
    }

    void CheckBrewButtonStatus()
    {
        brewButton.gameObject.SetActive(completedSliders >= 4);
    }

    void UpdateTargetImagePosition(SliderControl sliderControl)
    {
        float targetNormalized = Mathf.InverseLerp(sliderControl.slider.minValue, sliderControl.slider.maxValue, sliderControl.targetValue);
        RectTransform sliderRect = sliderControl.slider.GetComponent<RectTransform>();
        RectTransform targetRect = sliderControl.targetImage.GetComponent<RectTransform>();
        float newX = sliderRect.rect.xMin + (targetNormalized * sliderRect.rect.width);
        targetRect.anchoredPosition = new Vector2(newX, targetRect.anchoredPosition.y);
    }

    void UpdateCompletionImage(SliderControl sliderControl)
    {
        if (Mathf.RoundToInt(sliderControl.slider.value) == sliderControl.targetValue)
        {
            sliderControl.completionImage.color = Color.green;
        }
        else
        {
            sliderControl.completionImage.color = Color.white;
        }
    }

    void BrewCoffee()
    {
        if (brewingSound != null && audioSource != null)
        {
            audioSource.PlayOneShot(brewingSound);
            StartCoroutine(ClosePanelAfterBrewSound());
        }
        else
        {
            ClosePanel();
        }
    }

    IEnumerator ClosePanelAfterBrewSound()
    {
        // Wait for the brewing sound to finish
        yield return new WaitForSeconds(brewingSound.length);
        ClosePanel();
    }

    void ClosePanel()
    {
        completionPanel.SetActive(false);
        closeTask.SetActive(false);
    }
}