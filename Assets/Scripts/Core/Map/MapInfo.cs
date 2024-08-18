using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the map's tooltip
/// </summary>
public class MapInfo : MonoBehaviour
{
    [SerializeField] private LocalizedText text;

    /// <summary>
    /// Updates the tooltip's informations
    /// </summary>
    /// <param name="show">Is the tooltip visible ?</param>
    /// <param name="key">The tooltip's key</param>
    public void UpdateInfo(bool show, string key)
    {
        gameObject.SetActive(show);
        text.SetNewKey(key);

        if (show == true) transform.position = Input.mousePosition;
    }

    void Update()
    {
        if (gameObject.activeInHierarchy)
        {
            transform.position = Input.mousePosition;
        }
    }
}
