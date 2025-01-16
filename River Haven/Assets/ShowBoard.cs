using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShowBoard : MonoBehaviour
{

    public bool isActivatedBool = false;
    private CanvasGroup canvasGroup;
    public float fadeInDuration = 0.2f;

    void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
    }

    public void MakeVisible()
    {
        if (isActivatedBool == false)
        {
            Invoke("ShowBoardDelay", 1f);
            isActivatedBool = true;
        }
    }

    void ShowBoardDelay()
    {
        this.gameObject.SetActive(true);
        StartCoroutine(FadeInCoroutine());
    }

    public void DeactivateBool()
    {
        Invoke("DeActivate", 0.1f);
        this.gameObject.SetActive(false);
    }

    void DeActivate()
    {
        isActivatedBool = false;
    }

    private IEnumerator FadeInCoroutine()
    {
        float timeElapsed = 0f;

        while (timeElapsed < fadeInDuration)
        {
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, timeElapsed / fadeInDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 1f;
    }
}
