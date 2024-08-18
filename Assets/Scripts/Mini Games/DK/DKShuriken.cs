using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents a Shuriken in the Katsumoto minigame
/// </summary>
public class DKShuriken : MonoBehaviour
{
    [SerializeField] private float speed;
    private DKMiniGame.DKSide side;

    /// <summary>
    /// Initialize the shuriken
    /// </summary>
    /// <param name="side">The shuriken's side</param>
    public void Init(DKMiniGame.DKSide side)
    {
        this.side = side;
        transform.eulerAngles = new Vector3(0, 0, (side == DKMiniGame.DKSide.LEFT || side == DKMiniGame.DKSide.RIGHT) ? 90 : 0);
    }

    void Update()
    {
        transform.position -= Vector3.forward * speed * Time.deltaTime;
    }

    /// <summary>
    /// Destroys the shuriken
    /// </summary>
    public void DestroyShuriken()
    {
        Destroy(gameObject);
    }

    /// <summary>
    /// Gets the shuriken's side
    /// </summary>
    /// <returns>The shuriken's side</returns>
    public DKMiniGame.DKSide GetSide()
    {
        return side;
    }
}
