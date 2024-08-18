using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Utilitary class used to inject data into strings
/// </summary>
public class TagManager
{
    /// <summary>
    /// Injects data into a string
    /// </summary>
    /// <param name="s">The string</param>
    /// <param name="speaker">Is the string showing a speaker ?</param>
    public static void Inject(ref string s, bool speaker = true)
    {
        if (speaker && s.Equals("narrator")) return;

        if (speaker && !s.Contains("[")) // If Speaker is not the player, attempt to load the locals for him
        {
            s = Locals.GetLocal("Char_" + s);
            return;
        }
        s = s.Replace("[MC]", GameManager.GetSaveManager().GetItem("playerName"));

    }

    /// <summary>
    /// Splits a string by tags
    /// </summary>
    /// <param name="targetText">The string to split</param>
    /// <returns>The splited string</returns>
    public static string[] SplitByTags(string targetText)
    {
        return targetText.Split(new char[2] { '<', '>' });
    }
}
