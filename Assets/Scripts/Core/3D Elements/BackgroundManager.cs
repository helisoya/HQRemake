using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Manager for the backgrounds
/// </summary>
public class BackgroundManager : MonoBehaviour
{
    private string pathToBackgrounds = "Backgrounds/";
    public Background currentBackground { get; private set; }
    public static BackgroundManager instance;

    void Awake()
    {
        instance = this;
    }

    /// <summary>
    /// Replaces the current background
    /// </summary>
    /// <param name="newBackground">The new background's name</param>
    public void ReplaceBackground(string newBackground)
    {
        if (currentBackground != null && currentBackground.GetBackgroundName().Equals(newBackground)) return;

        GameObject obj = Resources.Load<GameObject>(pathToBackgrounds + newBackground);
        if (!obj) return;

        if (currentBackground)
        {
            currentBackground.UnregisterInteractables();
            Destroy(currentBackground.gameObject);
        }

        currentBackground = Instantiate(obj, transform).GetComponent<Background>();
    }

    /// <summary>
    /// Returns a background's marker position
    /// </summary>
    /// <param name="marker">The marker's name</param>
    /// <returns>The marker's position</returns>
    public Vector3 GetMarkerPosition(string marker)
    {
        if (currentBackground)
        {
            return currentBackground.GetMarkerPosition(marker);
        }
        return Vector3.zero;
    }

    /// <summary>
    /// Returns a background's marker rotation
    /// </summary>
    /// <param name="marker">The marker's name</param>
    /// <returns>The marker's rotation</returns>
    public float GetMarkerRotation(string marker)
    {
        if (currentBackground)
        {
            return currentBackground.GetMarkerRotation(marker);
        }
        return 0;
    }
}
