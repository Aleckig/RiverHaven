using System.Collections;
using UnityEngine;

public class TextScroll : MonoBehaviour
{
    [SerializeField] private RectTransform textRect;
    [SerializeField] private float scrollSpeed = 35f;
    [SerializeField] private float cycleDuration = 60f;      // Time before fade starts
    [SerializeField] private float fadeDuration = 5f;        // Fade-out time

    private Vector2 startPos;
    private CanvasGroup canvasGroup;

    void Start()
    {
        if (textRect == null)
            textRect = GetComponent<RectTransform>();

        startPos = textRect.anchoredPosition;

        // Add or get CanvasGroup for fading
        canvasGroup = textRect.GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = textRect.gameObject.AddComponent<CanvasGroup>();
        }

        canvasGroup.alpha = 1f;
        StartCoroutine(TextScrollCycle());
    }

    void Update()
    {
        textRect.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
    }

    private IEnumerator TextScrollCycle()
    {
        while (true)
        {
            // Wait for 1 minute
            yield return new WaitForSeconds(cycleDuration);

            // Fade out over fadeDuration
            float t = 0;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(1f, 0f, t / fadeDuration);
                yield return null;
            }

            // Reset scroll and fade in
            textRect.anchoredPosition = startPos;

            t = 0;
            while (t < fadeDuration)
            {
                t += Time.deltaTime;
                canvasGroup.alpha = Mathf.Lerp(0f, 1f, t / fadeDuration);
                yield return null;
            }
        }
    }
}
