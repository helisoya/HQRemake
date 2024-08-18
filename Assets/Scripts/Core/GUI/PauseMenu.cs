using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Represents the pause menu
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [Header("Pause Menu")]
    [SerializeField] private GameObject root;
    [SerializeField] private Slider sliderBGM;
    [SerializeField] private Slider sliderVOICE;
    [SerializeField] private Slider sliderSFX;
    [SerializeField] private Resolution[] resolutions;
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    [SerializeField] private Toggle fullscreenToggle;
    [SerializeField] private TMP_Dropdown qualityDropdown;
    public bool open { get { return root.activeInHierarchy; } }

    /// <summary>
    /// Resets the options values
    /// </summary>
    void ResetValues()
    {
        SETTINGSAVE save = Settings.GetSave();
        sliderBGM.SetValueWithoutNotify(save.valBGM);
        sliderVOICE.SetValueWithoutNotify(save.valVOICE);
        sliderSFX.SetValueWithoutNotify(save.valSFX);
        fullscreenToggle.SetIsOnWithoutNotify(save.fullscreen);


        resolutions = Screen.resolutions;

        List<string> list = new List<string>();

        int currentRes = 0;

        Resolution res;
        Resolution reference = Screen.currentResolution;
        for (int i = 0; i < resolutions.Length; i++)
        {
            res = resolutions[i];
            list.Add(res.width + "x" + res.height);
            if (currentRes == 0 && res.width == reference.width && res.height == reference.height && res.refreshRate == reference.refreshRate)
            {
                currentRes = i;
            }
        }
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(list);
        resolutionDropdown.SetValueWithoutNotify(currentRes);

        qualityDropdown.SetValueWithoutNotify(QualitySettings.GetQualityLevel());
    }

    /// <summary>
    /// Shows the pause menu
    /// </summary>
    public void Show()
    {
        Time.timeScale = 0;
        ResetValues();
        root.SetActive(true);
    }

    /// <summary>
    /// Changes the BGM volume
    /// </summary>
    public void SetBGM()
    {
        Settings.SetVolumeBGM(sliderBGM.value);
    }

    /// <summary>
    /// Changes the voice volume
    /// </summary>
    public void SetVoice()
    {
        Settings.SetVolumeVOICE(sliderVOICE.value);
    }

    /// <summary>
    /// Changes the SFX volume
    /// </summary>
    public void SetSFX()
    {
        Settings.SetVolumeSFX(sliderSFX.value);
    }

    /// <summary>
    /// Changes the game's resolution
    /// </summary>
    /// <param name="value">The resolution's index</param>
    public void ChangeResolution(int value)
    {
        Resolution newRes = resolutions[value];
        Settings.SetResolution(newRes.width, newRes.height, newRes.refreshRate);
    }

    /// <summary>
    /// Changes the game's quality
    /// </summary>
    /// <param name="value">The quality's index</param>
    public void ChangeQuality(int value)
    {
        Settings.SetQuality(value);
    }

    /// <summary>
    /// Changes if the game is in fullscreen or not
    /// </summary>
    /// <param name="value">Is the game in fullscreen ?</param>
    public void ChangeFullScreen(bool value)
    {
        Settings.SetFullscreen(value);
    }

    /// <summary>
    /// Closes the pause menu
    /// </summary>
    public void Close()
    {
        Time.timeScale = 1;
        root.SetActive(false);
    }

    /// <summary>
    /// Changes the game's language
    /// </summary>
    /// <param name="newVal">The new language's code</param>
    public void ChangeLanguage(string newVal)
    {
        Settings.ChangeLanguage(newVal);
    }

}
