using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a "Concerto de Flute" marker
/// </summary>
public class CFMarker : MonoBehaviour
{
    [SerializeField] private KeyCode code;

    private bool noteInside;
    private GameObject inside;

    void Update()
    {
        if (noteInside)
        {
            if (Input.GetKeyDown(code))
            {
                ((CFMiniGame)MiniGame.instance).AddScore();
                Destroy(inside);
                noteInside = false;
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        noteInside = true;
        inside = other.transform.gameObject;
    }

    void OnTriggerExit2D(Collider2D other)
    {
        noteInside = false;
    }
}
