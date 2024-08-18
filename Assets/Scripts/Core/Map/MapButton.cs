using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.EventSystems;

/// <summary>
/// Represents a clickable button on the map
/// </summary>
[RequireComponent(typeof(Image))]
public class MapButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("Informations")]
    [SerializeField] private string locationID;
    private string nextChapter;
    private string requirementName;
    private int requirementValue;
    private Image button;

    public bool canBeSeen
    {
        get
        {
            if (requirementName.Equals("HIDDEN")) return false;
            if (requirementName.Equals("ALWAYS")) return true;
            return int.Parse(GameManager.GetSaveManager().GetItem(requirementName)) >= requirementValue;
        }
    }

    void Awake()
    {
        button = GetComponent<Image>();
    }

    /// <summary>
    /// Sets the button's data
    /// </summary>
    /// <param name="nextChapter">The next chapter to load when clicked</param>
    /// <param name="requirementName">The requirement's key</param>
    /// <param name="requirementValue">The requirement's value (actual value >= value)</param>
    public void SetData(string nextChapter, string requirementName, int requirementValue)
    {
        this.nextChapter = nextChapter;
        this.requirementName = requirementName;
        this.requirementValue = requirementValue;
    }

    /// <summary>
    /// Returns the ID of the button
    /// </summary>
    /// <returns>The button's ID</returns>
    public string GetID()
    {
        return locationID;
    }

    /// <summary>
    /// Changes the color of the button
    /// </summary>
    /// <param name="color">The new color</param>
    public void SetColor(Color color)
    {
        if (button)
        {
            button.color = color;
        }
    }

    public void OnClick()
    {
        Map.instance.GoToChapter(nextChapter);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Map.instance.ShowInfo(true, locationID);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Map.instance.ShowInfo(false, "");
    }
}
