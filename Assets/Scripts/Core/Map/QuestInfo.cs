using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a quest on the Map GUI
/// </summary>
public class QuestInfo : MonoBehaviour
{
    [SerializeField] private LocalizedText questName;
    [SerializeField] private LocalizedText questObjective;

    /// <summary>
    /// Initiliaze the component
    /// </summary>
    /// <param name="id">The quest's ID</param>
    /// <param name="value">The quest's currrent value</param>
    public void Init(string id, string value)
    {
        questName.SetNewKey(id);
        questObjective.SetNewKey(id + "_" + value);
    }
}
