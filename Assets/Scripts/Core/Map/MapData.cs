using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Represents a map
/// </summary>
[System.Serializable]
public class MapData
{
    [SerializeField] private string mapID;
    [SerializeField] private Transform mapRoot;
    [SerializeField] private MapButton[] buttons;

    /// <summary>
    /// Returns the map's ID
    /// </summary>
    /// <returns>The map's ID</returns>
    public string GetID()
    {
        return mapID;
    }

    /// <summary>
    /// Show the map
    /// </summary>
    /// <param name="currentPoint">The current player's position</param>
    public void Show(string currentPoint)
    {
        mapRoot.gameObject.SetActive(true);

        foreach (MapButton button in buttons)
        {
            button.gameObject.SetActive(button.canBeSeen);
            button.SetColor(currentPoint.Equals(button.GetID()) ? Color.red : Color.white);
        }
    }

    /// <summary>
    /// Resets the map's buttons
    /// </summary>
    public void ResetAll()
    {
        foreach (MapButton button in buttons)
        {
            button.SetData("", "HIDDEN", 0);
        }
    }

    /// <summary>
    /// Changes a button's data
    /// </summary>
    /// <param name="id">The button's id</param>
    /// <param name="nextChapter">The next chapter to load when clicked</param>
    /// <param name="requirementName">The requirement's key</param>
    /// <param name="requirementValue">The requirement's value (actual value >= value)</param>
    public void SetData(string id, string nextChapter, string requirementName, int requirementValue)
    {
        MapButton button = buttons.First(x => x.GetID().Equals(id));
        if (button != null)
        {
            button.SetData(nextChapter, requirementName, requirementValue);
        }
    }

    /// <summary>
    /// Hides the map
    /// </summary>
    public void Hide()
    {
        mapRoot.gameObject.SetActive(false);
    }
}
