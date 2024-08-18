using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class PPRMiniGame : MiniGame
{
    [Header("Poem Poem Rickey")]
    [SerializeField] private int target = 20;
    [SerializeField] private int maxWords = 20;

    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI currentWordText;
    [SerializeField] private Animator rickeyAnimator;
    [SerializeField] private PPRWord[] words;
    [SerializeField] private PPRButton[] buttons;
    private int score;
    private int currentWord;


    public override void StartMiniGame()
    {
        base.StartMiniGame();
        Reset();
    }

    /// <summary>
    /// Resets the minigame
    /// </summary>
    private void Reset()
    {
        RearangeWords();
        currentWord = 0;
        score = 0;
        scoreText.text = "Score : 0";
        currentWordText.text = "0/" + maxWords.ToString();
    }

    /// <summary>
    /// Adds to the player's score
    /// </summary>
    /// <param name="val">The score to add</param>
    public void AddScore(int val)
    {
        score += val;
        currentWord++;
        scoreText.text = "Score : " + score.ToString();
        currentWordText.text = currentWord.ToString() + "/" + maxWords.ToString();
        rickeyAnimator.SetTrigger(val == 0 ? "Neutral" : (val < 0 ? "Bad" : "Good"));

        if (currentWord <= maxWords)
        {
            RearangeWords();
        }
        else
        {
            if (score < target)
            {
                Reset();
            }
            else
            {
                EndMiniGame();
            }
        }
    }

    /// <summary>
    /// Rearanges the words on screen
    /// </summary>
    public void RearangeWords()
    {
        List<PPRWord> copy = new List<PPRWord>(words);

        foreach (PPRButton text in buttons)
        {
            int i = Random.Range(0, copy.Count);
            text.UpdateValues(copy[i]);
            copy.RemoveAt(i);
        }
    }
}

[System.Serializable]
public class PPRWord
{
    public string id;
    public int value;
}