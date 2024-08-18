using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Handles the interactions with objects
/// </summary>
public class InteractionManager : MonoBehaviour
{
    [Header("Raycast")]
    [SerializeField] private LayerMask mask;

    public static InteractionManager instance;
    private Dictionary<string, InteractableObject> objects;
    public bool active { get; private set; }

    void Awake()
    {
        instance = this;

        objects = new Dictionary<string, InteractableObject>();
    }

    /// <summary>
    /// Registers an object
    /// </summary>
    /// <param name="id">The object's id</param>
    /// <param name="obj">The object</param>
    public void RegisterObject(string id, InteractableObject obj)
    {
        objects.Add(id, obj);
    }

    /// <summary>
    /// Unregisters an object
    /// </summary>
    /// <param name="id">The object's id</param>
    public void UnregisterObject(string id)
    {
        objects.Remove(id);
    }

    /// <summary>
    /// Changes the chapter to be loaded for an object
    /// </summary>
    /// <param name="id">The object's ID</param>
    /// <param name="chapter">The new chapter</param>
    public void ChangeObjectChapter(string id, string chapter)
    {
        if (objects.ContainsKey(id))
        {
            objects[id].ChangeChapter(chapter);
        }
    }

    /// <summary>
    /// Changes if an object is hidden or not
    /// </summary>
    /// <param name="id">The object's ID</param>
    /// <param name="value">Is the object hidden ?</param>
    public void SetObjectHidden(string id, bool value)
    {
        if (objects.ContainsKey(id))
        {
            objects[id].SetHidden(value);
        }
    }

    /// <summary>
    /// Changes if the player can interact with the objects
    /// </summary>
    /// <param name="active">Is the system active ?</param>
    public void SetActive(bool active)
    {
        this.active = active;

        VNGUI.instance.SetInteractionMode(active);

        foreach (InteractableObject obj in objects.Values)
        {
            obj.SetActive(active);
        }
    }

    /// <summary>
    /// Restore the interactables
    /// </summary>
    /// <param name="data">The save data</param>
    public void Load(List<GAMEFILE.INTERACTABLEDATA> data)
    {
        foreach (GAMEFILE.INTERACTABLEDATA entry in data)
        {
            SetObjectHidden(entry.ID, entry.hidden);
            ChangeObjectChapter(entry.ID, entry.chapter);
        }
    }

    public List<GAMEFILE.INTERACTABLEDATA> GetSaveData()
    {
        List<GAMEFILE.INTERACTABLEDATA> data = new List<GAMEFILE.INTERACTABLEDATA>();

        foreach (InteractableObject interactableObject in objects.Values)
        {
            data.Add(new GAMEFILE.INTERACTABLEDATA(
                interactableObject.GetID(),
                interactableObject.GetNextChapter(),
                interactableObject.hidden)
            );
        }

        return data;
    }

    void Update()
    {
        if (!active) return;

        RaycastHit hit;
        InteractableObject current = null;


        if (!EventSystem.current.IsPointerOverGameObject() && Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 500, mask))
        {
            current = hit.transform.GetComponent<InteractableObject>();
            Cursor.SetCursor(current.GetIcon(), Vector2.zero, CursorMode.Auto);
        }
        else
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
        }

        if (current != null && Input.GetMouseButtonDown(0))
        {
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
            current.OnInterraction();
            SetActive(false);
        }
    }
}
