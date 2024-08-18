using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SRMiniGame : MiniGame
{
    [Header("Swamp Run")]
    [Header("Settings")]
    [SerializeField] private float tileSize = 89.94f;
    [SerializeField] private float scrollingSpeed = 4;
    [SerializeField] private int maxTiles = 5;
    [SerializeField] private float barFillSpeed;
    private float barFillAmount = 0;

    [Header("Components")]
    [SerializeField] private Transform tilesRoot;
    [SerializeField] private RectTransform barPlayer;
    [SerializeField] private Image barFill;
    private GameObject[] availableTiles;

    public override void StartMiniGame()
    {
        base.StartMiniGame();
        availableTiles = Resources.LoadAll<GameObject>("MiniGames/SR/Tiles");
        SetBarFill(0);
        FillTiles();
    }

    /// <summary>
    /// Fills the initial map
    /// </summary>
    private void FillTiles()
    {
        Instantiate(availableTiles[0], tilesRoot).transform.position = Vector3.zero;

        for (int i = 1; i < maxTiles; i++)
        {
            Instantiate(availableTiles[Random.Range(0, availableTiles.Length)], tilesRoot).transform.position =
                new Vector3(0, 0, tileSize * i);
        }
    }

    /// <summary>
    /// Takes damage
    /// </summary>
    public void TakeDamage()
    {
        SetBarFill(Mathf.Clamp(barFillAmount - 0.1f, 0f, 1f));
    }

    /// <summary>
    /// Sets the progress bar fill amount
    /// </summary>
    /// <param name="fillAmount">The new fill amount</param>
    private void SetBarFill(float fillAmount)
    {
        barFillAmount = fillAmount;
        barFill.fillAmount = barFillAmount;
        barPlayer.anchoredPosition = new Vector3(barFillAmount * 600f, 0);
    }

    protected override void MiniGameUpdate()
    {
        base.MiniGameUpdate();

        // Tiles movements

        Vector3 vector = new Vector3(0, 0, -1) * scrollingSpeed * Time.deltaTime;

        foreach (Transform child in tilesRoot)
        {
            child.position += vector;
        }

        // Deletes a tile if off screen


        if (tilesRoot.GetChild(0).position.z <= -tileSize)
        {

            Instantiate(availableTiles[Random.Range(0, availableTiles.Length)], tilesRoot).transform.position =
                new Vector3(0, 0, tilesRoot.GetChild(maxTiles - 1).position.z + tileSize);

            Destroy(tilesRoot.GetChild(0).gameObject);
        }

        if (barFillAmount == 1f)
        {
            EndMiniGame();
        }
        else
        {
            SetBarFill(Mathf.Clamp(barFillAmount + barFillSpeed * Time.deltaTime, 0f, 1f));
        }

    }


}
