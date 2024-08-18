using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Background : MonoBehaviour
{
    [Header("Infos")]
    [SerializeField] public string backgroundName;
    [SerializeField] private Transform markersRoot;
    [SerializeField] private Material skybox;
    private Dictionary<string, Transform> markers;

    void Awake()
    {
        RegisterMarkers();
        RenderSettings.skybox = skybox;
    }

    /// <summary>
    /// Returns the background's name
    /// </summary>
    /// <returns>The background's name</returns>
    public string GetBackgroundName()
    {
        return backgroundName;
    }

    /// <summary>
    /// Registers the background's makers
    /// </summary>
    private void RegisterMarkers()
    {
        markers = new Dictionary<string, Transform>();

        foreach (Transform child in markersRoot)
        {
            markers[child.name] = child;
        }
    }

    /// <summary>
    /// Finds a marker's position
    /// </summary>
    /// <param name="marker">The marker's name</param>
    /// <returns>The marker's position</returns>
    public Vector3 GetMarkerPosition(string marker)
    {
        if (markers.ContainsKey(marker))
        {
            return markers[marker].position;
        }
        return Vector3.zero;
    }

    /// <summary>
    /// Finds a marker's rotation
    /// </summary>
    /// <param name="marker">The marker's name</param>
    /// <returns>The marker's rotation</returns>
    public float GetMarkerRotation(string marker)
    {
        if (markers.ContainsKey(marker))
        {
            return markers[marker].eulerAngles.y;
        }
        return 0;
    }

    /// <summary>
    /// Unregisters the background's interactables
    /// </summary>
    public void UnregisterInteractables()
    {
        InteractableObject[] interactables = GetComponentsInChildren<InteractableObject>();
        foreach (InteractableObject interactable in interactables)
        {
            interactable.Unregister();
        }
    }
}
