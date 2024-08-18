using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using UnityEngine.UI;

/// <summary>
/// Class handling the DK MiniGame
/// </summary>
public class DKMiniGame : MiniGame
{
    [Header("Duel Katsumoto")]
    [Header("Settings")]
    [SerializeField] private float timeToSurvive = 30;
    [SerializeField][Range(1f, 5f)] private float timeBetweenShurikens = 1;
    [SerializeField][Range(1f, 2f)] private float playerMoveCooldown;
    private float startTime;
    private float lastShurikenTime;
    private float lastPlayerMove;
    private DKSide currentPlayerSide;
    private DKSide currentThrowSide;
    private bool waitToThrow;

    [Header("Components")]
    [SerializeField] private GameObject prefabShuriken;
    [SerializeField] private Animator katsumotoAnimator;
    [SerializeField] private Animator playerAnimator;
    [SerializeField] private Image timerFill;
    [SerializeField] private Animator bloodAnimator;
    [SerializeField] private Transform leftHand;
    [SerializeField] private Transform rightHand;

    public enum DKSide
    {
        LEFT,
        RIGHT,
        UP,
        DOWN,
        MIDDLE
    }


    public override void StartMiniGame()
    {
        base.StartMiniGame();
        ResetTimer();
        playerAnimator.GetComponentInChildren<Renderer>().material.SetFloat("_Alpha", 0.5f);
        lastShurikenTime = Time.time;
        currentPlayerSide = DKSide.MIDDLE;
        waitToThrow = false;
    }

    /// <summary>
    /// Resets the timer 
    /// </summary>
    public void ResetTimer()
    {
        startTime = Time.time;
        timerFill.fillAmount = 1f;
    }


    protected override void MiniGameUpdate()
    {

        float fillAmount = 1f - Mathf.Clamp((Time.time - startTime) / timeToSurvive, 0f, 1f);
        timerFill.fillAmount = fillAmount;

        if (fillAmount == 0f)
        {
            EndMiniGame();
        }
        else if (Time.time - lastShurikenTime >= (waitToThrow ? 0.5f : timeBetweenShurikens))
        {
            if (waitToThrow)
            {
                waitToThrow = false;

                Transform selectedHand = currentThrowSide == DKSide.LEFT ? leftHand : rightHand;

                Instantiate(prefabShuriken, selectedHand.position, Quaternion.identity).GetComponent<DKShuriken>().Init(currentThrowSide);
            }
            else
            {
                currentThrowSide = (DKSide)Enum.GetValues(typeof(DKSide)).GetValue(UnityEngine.Random.Range(0, 4));
                katsumotoAnimator.SetTrigger(currentThrowSide.ToString().ToLower().FirstCharacterToUpper());
                waitToThrow = true;
                lastShurikenTime = Time.time;
            }

        }


        if (Time.time - lastPlayerMove < playerMoveCooldown) return;

        if (Input.GetKey(KeyCode.DownArrow))
        {
            currentPlayerSide = DKSide.DOWN;
            playerAnimator.SetTrigger("Down");
            lastPlayerMove = Time.time;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            currentPlayerSide = DKSide.UP;
            playerAnimator.SetTrigger("Up");
            lastPlayerMove = Time.time;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            currentPlayerSide = DKSide.LEFT;
            playerAnimator.SetTrigger("Left");
            lastPlayerMove = Time.time;
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            currentPlayerSide = DKSide.RIGHT;
            playerAnimator.SetTrigger("Right");
            lastPlayerMove = Time.time;
        }
        else
        {
            currentPlayerSide = DKSide.MIDDLE;
        }
    }

    /// <summary>
    /// Checks if the player has dodged the shuriken
    /// </summary>
    /// <param name="shurikenSide">The shuriken's side</param>
    public void CheckIfTouched(DKSide shurikenSide)
    {
        bool notTouched = (currentPlayerSide == DKSide.LEFT && shurikenSide == DKSide.LEFT) ||
            (currentPlayerSide == DKSide.RIGHT && shurikenSide == DKSide.RIGHT) ||
            (currentPlayerSide == DKSide.UP && shurikenSide == DKSide.DOWN) ||
            (currentPlayerSide == DKSide.DOWN && shurikenSide == DKSide.UP);

        if (!notTouched)
        {
            bloodAnimator.SetTrigger("Blood");
            ResetTimer();
        }
    }


    void OnTriggerEnter(Collider collider)
    {
        if (exiting || !started) return;

        CheckIfTouched(collider.GetComponent<DKShuriken>().GetSide());
    }

    void OnTriggerExit(Collider collider)
    {
        collider.GetComponent<DKShuriken>().DestroyShuriken();
    }
}
