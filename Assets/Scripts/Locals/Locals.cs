using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the languages
/// </summary>
public class Locals
{
    private static Locals self;
    private string currentLanguage;

    public static string current
    {
        get
        {
            return self.currentLanguage;
        }
    }

    private Dictionary<string, string> locals;

    /// <summary>
    /// Initiliazes the Locals
    /// </summary>
    public static void Init()
    {
        new Locals();
    }

    public Locals()
    {
        self = this;
        locals = new Dictionary<string, string>();
        ChangeLanguage("eng");
    }

    /// <summary>
    /// Changes the current language
    /// </summary>
    /// <param name="newOne">The new language's code</param>
    public static void ChangeLanguage(string newOne)
    {
        if (newOne.Equals(self.currentLanguage)) return;

        self.currentLanguage = newOne;
        self.locals.Clear();
        self.LoadContent(newOne + "_system");
        self.LoadContent(newOne + "_story");
    }

    /// <summary>
    /// Gets a localized string
    /// </summary>
    /// <param name="key">The string's ID</param>
    /// <returns>The localized string</returns>
    public static string GetLocal(string key)
    {
        if (key != null && self.locals.ContainsKey(key)) return self.locals[key];
        return "LOCAL_ERROR";
    }

    /// <summary>
    /// Loads the content of a file
    /// </summary>
    /// <param name="fileName">The filename</param>
    void LoadContent(string fileName)
    {
        List<string> fileContent = FileManager.ReadTextAsset(Resources.Load<TextAsset>("Locals/" + fileName));
        string line;
        string[] split;

        for (int i = 0; i < fileContent.Count; i++)
        {
            line = fileContent[i];
            if (line.StartsWith("#") || string.IsNullOrWhiteSpace(line)) continue;

            split = line.Split(" = ");

            if (split.Length != 2)
            {
                Debug.Log("Error on line " + line + ". There should be only one = .");
                continue;
            }

            if (split[0].EndsWith(" "))
            {
                split[0] = split[0].Substring(0, split[0].Length - 1);
            }
            if (split[1].EndsWith(" "))
            {
                split[1] = split[1].Substring(0, split[1].Length - 1);
            }
            locals.Add(split[0], split[1]);
        }
    }
}
