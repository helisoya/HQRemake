using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a "Concerto de Flute" Note
/// </summary>
public class CFNote : MonoBehaviour
{
    [SerializeField] private float speed;

    void Start()
    {
        GetComponent<SpriteRenderer>().color = new Color(Random.Range(0.6f, 1), Random.Range(0.6f, 1), Random.Range(0.6f, 1), 1);
    }

    void Update()
    {
        transform.position -= Vector3.up * speed * Time.deltaTime;

        if (transform.position.y <= -1.5f)
        {
            ((CFMiniGame)MiniGame.instance).TakeDamage();
            Destroy(gameObject);
        }
    }
}
