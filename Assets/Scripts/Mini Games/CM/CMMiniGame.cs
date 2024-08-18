using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Reprensents the "Combat contre Mage" MiniGame 
/// </summary>
public class CMMiniGame : MiniGame
{
    [Header("Combat contre Mage")]
    [SerializeField] private float timeToSurvive = 60;
    [SerializeField] private float rockSpawnCooldown;
    [SerializeField] private float playerSpeed;
    [SerializeField] private Rigidbody player;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private GameObject prefabRock;
    [SerializeField] private Image timerFill;
    [SerializeField] private Animator damageAnimator;
    [SerializeField] private Vector2 arenaSize;
    private float lastRockSpawn;
    private float startTime;

    public override void StartMiniGame()
    {
        base.StartMiniGame();
        lastRockSpawn = Time.time;
        startTime = Time.time;
    }

    public void TakeDamage()
    {
        startTime = Time.time;
        damageAnimator.SetTrigger("Blood");
    }

    protected override void MiniGameUpdate()
    {
        float fillAmount = 1f - Mathf.Clamp((Time.time - startTime) / timeToSurvive, 0f, 1f);
        timerFill.fillAmount = fillAmount;

        if (fillAmount == 0f)
        {
            EndMiniGame();
            return;
        }


        if (Time.time - lastRockSpawn >= rockSpawnCooldown)
        {
            lastRockSpawn = Time.time;

            for (int i = 0; i < 3; i++)
            {
                Instantiate(prefabRock, new Vector3(Random.Range(-arenaSize.x, arenaSize.x), 50, Random.Range(-arenaSize.y, arenaSize.y)), Quaternion.identity, transform);
            }
        }


        Vector3 moveVector = new Vector3(
            Input.GetAxis("Horizontal"),
            0,
            Input.GetAxis("Vertical")
        );

        player.velocity = moveVector * playerSpeed;

        if (moveVector != Vector3.zero)
        {
            player.transform.forward = moveVector;
        }

        playerAnimator.SetFloat("Speed", moveVector.normalized.magnitude);

    }
}
