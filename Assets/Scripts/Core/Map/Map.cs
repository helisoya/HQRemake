using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System;
using System.Numerics;
using System.Linq;

/// <summary>
/// Handles the game's maps
/// </summary>
public class Map : MonoBehaviour
{
    public static Map instance;

    [Header("Map")]
    [SerializeField] private GameObject root;
    [SerializeField] private MapData[] maps;
    [SerializeField] private MapInfo cursor;
    private MapData currentMap;

    [Header("Quests")]
    [SerializeField] private GameObject questRoot;
    [SerializeField] private Transform questsParent;
    [SerializeField] private GameObject questPrefab;
    private List<string> questsId;

    void Awake()
    {
        instance = this;
        InitQuestIds();
    }

    void Start()
    {
        InitMaps();
    }

    /// <summary>
    /// Toggle the quests tab
    /// </summary>
    public void ClickQuestButton()
    {
        if (questRoot.activeInHierarchy) CloseQuests();
        else OpenQuests();
    }

    /// <summary>
    /// Closes the quests tab
    /// </summary>
    void CloseQuests()
    {
        questRoot.SetActive(false);
    }

    /// <summary>
    /// Opens the quests tab
    /// </summary>
    void OpenQuests()
    {
        questRoot.SetActive(true);
        foreach (Transform child in questsParent)
        {
            Destroy(child.gameObject);
        }

        int nbQuestShown = 0;
        foreach (string questId in questsId)
        {
            string value = GameManager.GetSaveManager().GetItem(questId);
            if (value != "-1" && value != "100")
            {
                Instantiate(questPrefab, questsParent).GetComponent<QuestInfo>().Init(questId, value);
                nbQuestShown++;
            }
        }

        questsParent.GetComponent<RectTransform>().sizeDelta = new UnityEngine.Vector2(
            questsParent.GetComponent<RectTransform>().sizeDelta.x,
            (questPrefab.GetComponent<RectTransform>().sizeDelta.y + 5) * nbQuestShown
        );
    }

    /// <summary>
    /// Shows informations on the tooltip
    /// </summary>
    /// <param name="show">Is the tooltip visible ?</param>
    /// <param name="key">The key to the tooltip's text</param>
    public void ShowInfo(bool show, string key)
    {
        cursor.UpdateInfo(show, key);
    }

    /// <summary>
    /// Opens the map
    /// </summary>
    /// <param name="map">The map to open</param>
    /// <param name="currentPoint">The player's current position</param>
    public void OpenMap(string map, string currentPoint)
    {

        root.SetActive(true);
        CloseQuests();

        if (currentMap != null) currentMap.Hide();

        foreach (MapData mapData in maps)
        {
            if (mapData.GetID().Equals(map))
            {
                currentMap = mapData;
                currentMap.Show(currentPoint);
            }
        }
    }

    /// <summary>
    /// Closes the map
    /// </summary>
    public void CloseMap()
    {
        root.SetActive(false);
        if (currentMap != null)
        {
            currentMap.Hide();
        }
    }

    /// <summary>
    /// Launches the selected chapter
    /// </summary>
    /// <param name="filename">The chapter's name</param>
    public void GoToChapter(string filename)
    {
        cursor.UpdateInfo(false, "");
        InteractionManager.instance.SetActive(false);
        CloseMap();
        NovelController.instance.LoadChapterFile(filename);
    }

    /// <summary>
    /// Reads which variables are quests
    /// </summary>
    private void InitQuestIds()
    {
        questsId = new List<string>();

        List<string> lines = FileManager.ReadTextAsset(Resources.Load<TextAsset>("General/quests"));
        foreach (string line in lines)
        {
            if (!string.IsNullOrEmpty(line) && !line.StartsWith('#'))
            {
                questsId.Add(line);
            }
        }
    }

    /// <summary>
    /// Initialize the maps
    /// </summary>
    private void InitMaps()
    {
        MapData currentMap = null;
        List<string> lines = FileManager.ReadTextAsset(Resources.Load<TextAsset>("General/map"));
        foreach (string line in lines)
        {
            if (!string.IsNullOrEmpty(line) && !line.StartsWith('#'))
            {
                if (line.StartsWith('['))
                {
                    string id = line.Split(new char[] { '[', ']' }, StringSplitOptions.RemoveEmptyEntries)[0];
                    currentMap = maps.First(x => x.GetID().Equals(id));
                    if (currentMap != null)
                    {
                        currentMap.ResetAll();
                    }
                }
                else if (currentMap != null)
                {
                    string[] split = line.Split(' ');
                    if (split[1].Equals("HIDDEN")) currentMap.SetData(split[0], "", "HIDDEN", 0);
                    else if (split[2].Equals("ALWAYS")) currentMap.SetData(split[0], split[1], split[2], 0);
                    else currentMap.SetData(split[0], split[1], split[2], int.Parse(split[3]));
                }
            }
        }
    }
}
