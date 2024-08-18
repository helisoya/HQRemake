using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Represents a text that is localized
/// </summary>
public class LocalizedText : MonoBehaviour
{
    [SerializeField] protected TextMeshProUGUI text;
    [SerializeField] protected string localKey;

    /// <summary>
    /// Changes the ID of the localized text
    /// </summary>
    /// <param name="key">The new ID</param>
    public void SetNewKey(string key)
    {
        localKey = key;
        ReloadText();
    }

    /// <summary>
    /// Reloads the localized text
    /// </summary>
    public virtual void ReloadText()
    {
        text.text = Locals.GetLocal(localKey);
    }


    protected void Start()
    {
        ReloadText();
    }

    /// <summary>
    /// Returns the text field
    /// </summary>
    /// <returns>The text field</returns>
    public TextMeshProUGUI GetText()
    {
        return text;
    }

    public string key { get { return localKey; } }
}

