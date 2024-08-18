using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

/// <summary>
/// Handles the Main Menu
/// </summary>
public class MainMenuManager : MonoBehaviour
{
    [Header("General")]
    [SerializeField] private string menuMusic;
    [SerializeField] private PauseMenu pauseMenu;
    [SerializeField] private Fade fade;

    [Header("Main Screen")]
    [SerializeField] private GameObject mainScreenRoot;
    [SerializeField] private Button continueButton;


    [Header("Name Input")]
    [SerializeField] private GameObject nameInputRoot;
    [SerializeField] private TMP_InputField nameInput;

    private Coroutine fading;



    void Start()
    {
        AudioManager.instance.PlaySong(menuMusic);
        continueButton.interactable = GameManager.GetSaveManager().saveFileExistsOnDisk;
        fade.ForceAlphaTo(1);
        fade.FadeTo(0);
    }

    /// <summary>
    /// Click event for starting a new game
    /// </summary>
    public void Event_NewGame()
    {
        if (fading != null) return;
        mainScreenRoot.SetActive(false);
        nameInputRoot.SetActive(true);
    }

    /// <summary>
    /// Click event for confirming the name input and starting the game 
    /// </summary>
    public void Event_ConfirmName()
    {
        if (!string.IsNullOrEmpty(nameInput.text))
        {
            GameManager.instance.SetIsLoadingSave(false);
            GameManager.instance.SetNextChapter("Intro/Intro");
            GameManager.GetSaveManager().ResetItems();
            GameManager.GetSaveManager().EditItem("playerName", nameInput.text);
            AudioManager.instance.PlaySong(null);

            StartTransitionToVN();
        }
    }

    /// <summary>
    /// Click event for loading the saved game
    /// </summary>
    public void Event_Continue()
    {
        if (fading != null) return;
        GameManager.instance.SetIsLoadingSave(true);
        GameManager.instance.SetNextChapter("");
        StartTransitionToVN();
    }

    /// <summary>
    /// Click event for opening the settings
    /// </summary>
    public void Event_OpenSettings()
    {
        if (fading != null) return;
        pauseMenu.Show();
    }

    /// <summary>
    /// Click event for quitting the game
    /// </summary>
    public void Event_Quit()
    {
        if (fading != null) return;
        Application.Quit();
    }

    /// <summary>
    /// Starts the transition to the VN Scene
    /// </summary>
    private void StartTransitionToVN()
    {
        if (fading != null) return;

        fading = StartCoroutine(Transition());
    }

    /// <summary>
    /// Routine for the transition to the VN Scene
    /// </summary>
    /// <returns>IEnumerator</returns>
    IEnumerator Transition()
    {
        fade.FadeTo(1);

        yield return new WaitForEndOfFrame();
        while (fade.fading)
        {
            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene("VN");
    }
}
