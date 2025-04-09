using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class TextScroll : MonoBehaviour
{
    [SerializeField] private RectTransform textRect;
    [SerializeField] private float scrollSpeed = 30f;
    [SerializeField] private float resetDelay = 30f;

    private Vector2 startPos;
    private float textHeight;
    private RectTransform parentRect;

    void Start()
    {
        if (textRect == null)
            textRect = GetComponent<RectTransform>();

        parentRect = textRect.parent.GetComponent<RectTransform>();
        startPos = textRect.anchoredPosition;

        // Force layout to update before measuring
        Canvas.ForceUpdateCanvases();
        textHeight = textRect.rect.height;
    }

    void Update()
    {
        textRect.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;

        // Check if text has completely scrolled past
        if (textRect.anchoredPosition.y > textHeight)
        {
            StartCoroutine(ResetScroll());
        }
    }

    private System.Collections.IEnumerator ResetScroll()
    {
        yield return new WaitForSeconds(resetDelay);
        textRect.anchoredPosition = startPos;
    }
   
}
