using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

/// <summary>
/// Handles the player's save file
/// </summary>
public class SaveManager
{
    private string savePath = FileManager.savPath + "save.txt";
    public GAMEFILE saveFile { get; private set; }
    public bool saveFileExistsOnDisk
    {
        get
        {
            return System.IO.File.Exists(savePath);
        }
    }

    public SaveManager()
    {
        InitSaveFile();
    }

    /// <summary>
    /// Initializes the save file
    /// </summary>
    private void InitSaveFile()
    {
        saveFile = new GAMEFILE();

        List<string> lines = FileManager.ReadTextAsset(Resources.Load<TextAsset>("General/items"));
        foreach (string line in lines)
        {
            if (!string.IsNullOrEmpty(line) && !line.StartsWith('#'))
            {
                string[] split = line.Split(' ');
                saveFile.items.Add(new GAMEFILE.ITEM(split[0], split[1]));
            }
        }
    }

    /// <summary>
    /// Resets the items to their default values
    /// </summary>
    public void ResetItems()
    {
        foreach (GAMEFILE.ITEM item in saveFile.items)
        {
            item.value = item.defaultValue;
        }
    }

    /// <summary>
    /// Saves the current save file
    /// </summary>
    public void Save()
    {
        FileManager.SaveJSON(savePath, saveFile);
    }

    /// <summary>
    /// Loads the save file from disk
    /// </summary>
    /// <returns>The new save file</returns>
    public GAMEFILE Load()
    {
        if (saveFileExistsOnDisk)
        {
            GAMEFILE oldSave = saveFile;
            saveFile = FileManager.LoadJSON<GAMEFILE>(savePath);
            foreach (GAMEFILE.ITEM item in oldSave.items)
            {
                if (saveFile.items.Find(x => x.name.Equals(item.name)) == null)
                {
                    saveFile.items.Add(item);
                }
            }
        }


        return saveFile;
    }

    /// <summary>
    /// Finds the index of an item
    /// </summary>
    /// <param name="key">The item's key</param>
    /// <returns>The item's index</returns>
    private int GetIndexOfItem(string key)
    {
        for (int i = 0; i < saveFile.items.Count; i++)
        {
            if (saveFile.items[i].name == key)
            {
                return i;
            }
        }
        return 0;
    }

    /// <summary>
    /// Edits an item's value
    /// </summary>
    /// <param name="key">The item's key</param>
    /// <param name="value">The item's new value</param>
    public void EditItem(string key, string value)
    {
        int index = GetIndexOfItem(key);

        if (value.Equals("++"))
        {
            value = (int.Parse(saveFile.items[index].value) + 1).ToString();
        }
        else if (value.Equals("--"))
        {
            value = (int.Parse(saveFile.items[index].value) - 1).ToString();
        }

        saveFile.items[index].value = value;
    }

    /// <summary>
    /// Returns the value of an item
    /// </summary>
    /// <param name="key">The item's key</param>
    /// <returns>The item's value</returns>
    public string GetItem(string key)
    {
        return saveFile.items[GetIndexOfItem(key)].value;
    }
}
