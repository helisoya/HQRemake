using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a Rock in the "Combat contre Mage" MiniGame
/// </summary>
public class CMRock : MonoBehaviour
{
    [Header("Rock")]
    [SerializeField] private float speed;

    void Update()
    {
        if (transform.position.y <= -10)
        {
            Destroy(gameObject);
            return;
        }

        transform.position -= Vector3.up * speed * Time.deltaTime;
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            ((CMMiniGame)MiniGame.instance).TakeDamage();
            Destroy(gameObject);
        }
    }
}
