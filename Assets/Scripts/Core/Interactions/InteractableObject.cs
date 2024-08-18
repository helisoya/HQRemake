using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

/// <summary>
/// Represents an object that can be interacted with
/// </summary>
public class InteractableObject : MonoBehaviour
{
    [Header("General Informations")]
    [SerializeField] private string ID;
    [SerializeField] private string nextChapter;
    [SerializeField] private Texture2D icon;
    [SerializeField] private float glowFadeSpeed = 2;

    [Header("Components")]
    [SerializeField] private Renderer[] objectRenderers;
    private Coroutine routineColor;
    private bool active;
    public bool hidden { get; private set; }

    void Awake()
    {
        InteractionManager.instance.RegisterObject(ID, this);
    }

    /// <summary>
    /// Changes if the interaction is active or not
    /// </summary>
    /// <param name="active">Is the interaction active ?</param>
    public void SetActive(bool active)
    {
        this.active = active;

        SetRendererValue(active ? 1 : 0);
    }

    /// <summary>
    /// Changes the renderer's value
    /// </summary>
    /// <param name="value">The new value</param>
    private void SetRendererValue(float value)
    {
        if (routineColor != null)
        {
            StopCoroutine(routineColor);
        }
        routineColor = StartCoroutine(Routine_Color(value));
    }

    /// <summary>
    /// Changes if the object is hidden or not
    /// </summary>
    /// <param name="value">IS the object hidden ?</param>
    public void SetHidden(bool value)
    {
        hidden = value;
        gameObject.SetActive(!value);
    }

    /// <summary>
    /// Event for interacting with the object
    /// </summary>
    public void OnInterraction()
    {
        if (active && !hidden)
        {
            if (nextChapter.StartsWith("MAP"))
            {
                string[] parameters = nextChapter.Split('-');
                Map.instance.OpenMap(parameters[1], parameters[2]);
            }
            else
            {
                NovelController.instance.LoadChapterFile(nextChapter);
            }

        }
    }

    /// <summary>
    /// Changes the chapter to be loaded when interacted with
    /// </summary>
    /// <param name="chapter">The new chapter</param>
    public void ChangeChapter(string chapter)
    {
        nextChapter = chapter;
    }

    /// <summary>
    /// Gets the interactable's icon
    /// </summary>
    /// <returns>The icon</returns>
    public Texture2D GetIcon()
    {
        return icon;
    }

    /// <summary>
    /// Gets the interactable's ID
    /// </summary>
    /// <returns>The ID</returns>
    public string GetID()
    {
        return ID;
    }

    /// <summary>
    /// Gets the interactable's next chapter
    /// </summary>
    /// <returns>The next chapter</returns>
    public string GetNextChapter()
    {
        return nextChapter;
    }

    /// <summary>
    /// Unregisters the object
    /// </summary>
    public void Unregister()
    {
        InteractionManager.instance.UnregisterObject(ID);
    }


    IEnumerator Routine_Color(float target)
    {
        if (objectRenderers == null || objectRenderers.Length == 0) yield break;

        float alpha = objectRenderers[0].material.GetFloat("_FresnelPower");
        float side = alpha > target ? -1 : 1;
        float min = alpha > target ? target : 0;
        float max = alpha > target ? 1 : target;

        while (alpha != target)
        {
            alpha = Mathf.Clamp(alpha + glowFadeSpeed * Time.deltaTime * side, min, max);

            foreach (Renderer renderer in objectRenderers)
            {
                foreach (Material material in renderer.materials)
                {
                    material.SetFloat("_FresnelPower", alpha);
                }
            }

            yield return new WaitForEndOfFrame();
        }
    }
}
