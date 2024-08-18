using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

/// <summary>
/// Represents the "Concerto de Flute" MiniGame
/// </summary>
public class CFMiniGame : MiniGame
{
    [Header("Concerto de Flute")]
    [SerializeField] private GameObject prefabNote;
    [SerializeField] private Transform spawnsRoot;
    [SerializeField] private Image barFill;
    [SerializeField] private float amountPerNote;
    [SerializeField] private float malusPerNote;
    [SerializeField] private float cooldownBetweenNotes;
    [SerializeField] private AudioClip goodClip;
    [SerializeField] private AudioClip badClip;
    [SerializeField] private AudioMixerGroup group;
    private float currentScore;
    private float lastNoteSpawn;

    public override void StartMiniGame()
    {
        base.StartMiniGame();
        currentScore = 0;
        lastNoteSpawn = Time.time;
        barFill.fillAmount = 0;
    }

    /// <summary>
    /// Adds score to the player
    /// </summary>
    public void AddScore()
    {
        currentScore = Mathf.Clamp(currentScore + amountPerNote, 0f, 1f);
        barFill.fillAmount = currentScore;
        PlaySFX(goodClip);

        if (currentScore == 1f)
        {
            EndMiniGame();
        }
    }

    /// <summary>
    /// Plays a SFX
    /// </summary>
    /// <param name="clip">The SFX's clip</param>
    private void PlaySFX(AudioClip clip)
    {
        AudioSource newSource = new GameObject("sound").AddComponent<AudioSource>();
        newSource.transform.SetParent(instance.transform);
        newSource.clip = clip;
        newSource.volume = 1f;
        newSource.outputAudioMixerGroup = group;
        newSource.pitch = Random.Range(0.5f, 1f);
        newSource.Play();

        Destroy(newSource.gameObject, clip.length);
    }

    /// <summary>
    /// Takes damage
    /// </summary>
    public void TakeDamage()
    {
        currentScore = Mathf.Clamp(currentScore - malusPerNote, 0f, 1f);
        barFill.fillAmount = currentScore;
        PlaySFX(badClip);
    }

    protected override void MiniGameUpdate()
    {
        if (Time.time - lastNoteSpawn >= cooldownBetweenNotes)
        {
            lastNoteSpawn = Time.time;
            Instantiate(prefabNote, spawnsRoot.GetChild(Random.Range(0, spawnsRoot.childCount)).position, Quaternion.identity, transform);
        }
    }
}
