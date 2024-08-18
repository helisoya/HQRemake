using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TtileHeader : MonoBehaviour
{
    [Header("Title Header")]
    [SerializeField] private Image banner;
    [SerializeField] private LocalizedText titleText;
    [SerializeField] private CanvasGroup canvasGroup;
    [SerializeField] private float fadeSpeed = 1;

    public void Show(string displayTitle)
    {
        titleText.SetNewKey(displayTitle);

        if (isRevealing)
        {
            StopCoroutine(revealing);
        }
        revealing = StartCoroutine(Revealing());
    }

    public void Hide()
    {
        if (isRevealing)
        {
            StopCoroutine(revealing);
        }
        revealing = null;
        canvasGroup.alpha = 0f;
    }

    public bool isRevealing { get { return revealing != null; } }

    Coroutine revealing = null;

    IEnumerator Revealing()
    {
        canvasGroup.alpha = 0f;

        while (canvasGroup.alpha < 1f)
        {
            canvasGroup.alpha += fadeSpeed * Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        revealing = null;
    }
}
