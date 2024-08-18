using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a PPR Minigame's button
/// </summary>
public class PPRButton : MonoBehaviour
{
    private int val = 0;

    /// <summary>
    /// Updates the button's values
    /// </summary>
    /// <param name="word">The linked word</param>
    public void UpdateValues(PPRWord word)
    {
        val = word.value;
        GetComponent<LocalizedText>().SetNewKey("PPR_" + word.id);
    }

    /// <summary>
    /// OnClick event
    /// </summary>
    public void MakeChoice()
    {
        ((PPRMiniGame)(MiniGame.instance)).AddScore(val);
    }
}


