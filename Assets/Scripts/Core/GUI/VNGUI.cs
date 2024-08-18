using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Reprensents the VN's GUI
/// </summary>
public class VNGUI : MonoBehaviour
{
    public static VNGUI instance;

    [Header("Fades")]
    [SerializeField] private Fade fadeBg;
    [SerializeField] private Fade fadeFg;

    [Header("Buttons")]
    [SerializeField] private Button[] buttonsRequiringSaveReady;
    [SerializeField] private Button loadButton;

    [Header("Pause")]
    [SerializeField] private PauseMenu pauseMenu;

    [Header("Interaction Mode")]
    [SerializeField] private GameObject interactionModeRoot;

    private Coroutine routineExit;

    public bool fadingBg { get { return fadeBg.fading; } }
    public bool fadingFg { get { return fadeFg.fading; } }
    public float fadeBgAlpha { get { return fadeBg.currentAlpha; } }
    public float fadeFgAlpha { get { return fadeFg.currentAlpha; } }

    void Awake()
    {
        instance = this;

        fadeFg.ForceAlphaTo(1);
        fadeFg.FadeTo(0);
        fadeBg.ForceAlphaTo(1);
        fadeBg.FadeTo(0);
    }

    /// <summary>
    /// Fades the Background
    /// </summary>
    /// <param name="alpha">Alpha target</param>
    public void FadeBgTo(float alpha)
    {
        fadeBg.FadeTo(alpha);
    }

    /// <summary>
    /// Fades the Foreground
    /// </summary>
    /// <param name="alpha">Alpha target</param>
    public void FadeFgTo(float alpha)
    {
        fadeFg.FadeTo(alpha);
    }

    /// <summary>
    /// Force the Background to a set alpha
    /// </summary>
    /// <param name="alpha">The new alpha</param>
    public void ForceBgTo(float alpha)
    {
        fadeBg.ForceAlphaTo(alpha);
    }

    /// <summary>
    /// Force the Foreground to a set alpha
    /// </summary>
    /// <param name="alpha">The new alpha</param>
    public void ForceFgTo(float alpha)
    {
        fadeFg.ForceAlphaTo(alpha);
    }


    /// <summary>
    /// Click event for saving
    /// </summary>
    public void Save()
    {
        if (!NovelController.instance.isReadyForSaving) return;

        NovelController.instance.SaveGameFile();
    }

    /// <summary>
    /// Click event for loading
    /// </summary>
    public void Load()
    {
        if (GameManager.GetSaveManager().saveFileExistsOnDisk)
        {
            ResetCursor();
            NovelController.instance.LoadGameFile();
        }


    }

    /// <summary>
    /// Resets the cursor
    /// </summary>
    private void ResetCursor()
    {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    /// <summary>
    /// Click event for loading to the main menu
    /// </summary>
    public void QuitToMainMenu()
    {
        if (routineExit != null) return;
        ResetCursor();

        routineExit = StartCoroutine(Routine_Exit());
    }

    IEnumerator Routine_Exit()
    {
        InteractionManager.instance.SetActive(false);
        AudioManager.instance.PlaySong(null);
        fadeFg.FadeTo(1);

        yield return new WaitForEndOfFrame();
        while (fadeFg.fading)
        {
            yield return new WaitForEndOfFrame();
        }

        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Event for opening the settings
    /// </summary>
    public void OpenSettings()
    {
        ResetCursor();
        if (!pauseMenu.open) pauseMenu.Show();
    }

    /// <summary>
    /// Event for skipping the dialog
    /// </summary>
    public void SkipDialog()
    {
        NovelController.instance.Next();
    }

    /// <summary>
    /// Changes if the interaction mode is active or not
    /// </summary>
    /// <param name="active">Is the interaction mode active ?</param>
    public void SetInteractionMode(bool active)
    {
        interactionModeRoot.SetActive(active);
        if (active)
        {
            DialogSystem.instance.Close();
        }
    }

    void Update()
    {
        foreach (Button button in buttonsRequiringSaveReady)
        {
            button.interactable = NovelController.instance.isReadyForSaving;
        }
        loadButton.interactable = loadButton.interactable && GameManager.GetSaveManager().saveFileExistsOnDisk;

        if (NovelController.instance.isReadyForSaving && Input.GetKeyDown(KeyCode.Escape))
        {
            if (pauseMenu.open) pauseMenu.Close();
            else pauseMenu.Show();
        }

    }
}
