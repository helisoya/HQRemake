using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a fadable image
/// </summary>
public class Fade : MonoBehaviour
{
    [Header("Infos")]
    [SerializeField] private CanvasGroup canvaGroup;
    [SerializeField] private float transitionSpeed = 1f;
    public float currentAlpha
    {
        get { return canvaGroup.alpha; }
    }
    public bool fading { get { return routineFading != null; } }
    private Coroutine routineFading;

    /// <summary>
    /// Fades the image
    /// <param name="alpha"/>The alpha target</param>
    /// </summary>
    public void FadeTo(float alpha)
    {
        if (routineFading != null) StopCoroutine(routineFading);
        routineFading = StartCoroutine(RoutineFading(alpha));
    }

    /// <summary>
    /// Forces the alpha value
    /// </summary>
    /// <param name="alpha">The new alpha value</param>
    public void ForceAlphaTo(float alpha)
    {
        canvaGroup.alpha = alpha;
        if (routineFading != null)
        {
            StopCoroutine(routineFading);
            routineFading = null;
        }
    }


    /// <summary>
    /// IEnumerator for the image's fading
    /// </summary>
    /// <param name="target">The target alpha</param>
    /// <returns>IEnumerator</returns>
    IEnumerator RoutineFading(float target)
    {
        float currentAlpha = canvaGroup.alpha;
        int side = target > currentAlpha ? 1 : -1;
        float max = side == 1 ? target : 1f;
        float min = side == 1 ? currentAlpha : target;

        while (currentAlpha != target)
        {
            currentAlpha = Mathf.Clamp(currentAlpha + transitionSpeed * side * Time.deltaTime, min, max);
            canvaGroup.alpha = currentAlpha;
            yield return new WaitForEndOfFrame();
        }

        routineFading = null;
    }
}
