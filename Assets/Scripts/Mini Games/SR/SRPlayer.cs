using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Represents the player in the Swamp Run Minigame
/// </summary>
public class SRPlayer : MonoBehaviour
{
    [Header("Infos")]
    [SerializeField] private float playerSpeed = 5;
    [SerializeField] private float stunTime = 1f;

    [Header("Components")]
    [SerializeField] private Transform player;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Animator bloodAnimator;

    private float stunStart = 0f;
    private bool waitForStun = false;

    // Update is called once per frame
    void Update()
    {
        if (!MiniGame.instance.started || MiniGame.instance.exiting) return;

        if (!waitForStun)
        {
            player.position = new Vector3(
                Mathf.Clamp(player.position.x + Input.GetAxis("Horizontal") * playerSpeed * Time.deltaTime, -6f, 6f),
                player.position.y,
                player.position.z
            );
        }
        else if (Time.time - stunStart >= stunTime)
        {
            waitForStun = false;
        }
    }


    /// <summary>
    /// Takes damage
    /// </summary>
    public void TakeDamage()
    {
        playerAnimator.SetTrigger("Damage");
        bloodAnimator.SetTrigger("Blood");
        stunStart = Time.time;
        waitForStun = true;
        ((SRMiniGame)MiniGame.instance).TakeDamage();
    }


    public void OnTriggerEnter(Collider collider)
    {
        if (!waitForStun)
        {
            TakeDamage();
        }
    }
}
