using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Audio;

public class Settings
{
    private static Settings self;
    private SETTINGSAVE save;
    private string savePath = FileManager.savPath + "general.txt";
    private AudioMixer mixer;


    public static void Init()
    {
        new Settings();
    }

    public Settings()
    {
        self = this;

        mixer = GameManager.instance.GetAudioMixer();

        if (System.IO.File.Exists(savePath))
        {
            save = FileManager.LoadJSON<SETTINGSAVE>(savePath);
            ChangeLanguage(save.language, false);
            Screen.SetResolution(save.resolutionWidth, save.resolutionHeight, save.fullscreen, save.refreshRate);
            SetVolumeBGM(save.valBGM, false);
            SetVolumeSFX(save.valSFX, false);
            SetVolumeVOICE(save.valVOICE, false);
            SetQuality(save.quality, false);
        }
        else
        {
            save = new SETTINGSAVE();
            save.refreshRate = Screen.currentResolution.refreshRate;
            save.resolutionHeight = Screen.currentResolution.height;
            save.resolutionWidth = Screen.currentResolution.width;
            SaveFile();
        }
    }

    /// <summary>
    /// Changes the current language
    /// </summary>
    /// <param name="newLocal">The new language</param>
    /// <param name="saveAfterwards">Should the settings be saved afterwards ?</param>
    public static void ChangeLanguage(string newLocal, bool saveAfterwards = true)
    {
        self.save.language = newLocal;

        Locals.ChangeLanguage(newLocal);
        LocalizedText[] texts = Object.FindObjectsOfType<LocalizedText>();
        foreach (LocalizedText text in texts)
        {
            text.ReloadText();
        }
        if (DialogSystem.instance != null) DialogSystem.instance.UpdateTextLanguage();

        if (saveAfterwards) self.SaveFile();
    }

    /// <summary>
    /// Saves the settings
    /// </summary>
    private void SaveFile()
    {
        FileManager.SaveJSON(savePath, save);
    }

    /// <summary>
    /// Changes the quality of the graphics
    /// </summary>
    /// <param name="quality">The new quality</param>
    /// <param name="saveAfterwards">Should the settings be saved afterwards ?</param>
    public static void SetQuality(int quality, bool saveAfterwards = true)
    {
        self.save.quality = quality;
        QualitySettings.SetQualityLevel(quality);

        if (saveAfterwards) self.SaveFile();
    }

    /// <summary>
    /// Changes the volume of the BGMs
    /// </summary>
    /// <param name="sliderValue">The new volume</param>
    /// <param name="saveAfterwards">Should the settings be saved afterwards ?</param>
    public static void SetVolumeBGM(float sliderValue, bool saveAfterwards = true)
    {
        self.save.valBGM = sliderValue;
        self.mixer.SetFloat("BGM", Mathf.Log10(sliderValue) * 20);

        if (saveAfterwards) self.SaveFile();
    }

    /// <summary>
    /// Changes the volume of the voices
    /// </summary>
    /// <param name="sliderValue">The new volume</param>
    /// <param name="saveAfterwards">Should the settings be saved afterwards ?</param>
    public static void SetVolumeVOICE(float sliderValue, bool saveAfterwards = true)
    {
        self.save.valVOICE = sliderValue;
        self.mixer.SetFloat("Voice", Mathf.Log10(sliderValue) * 20);

        if (saveAfterwards) self.SaveFile();
    }

    /// <summary>
    /// Changes the volume of the SFXs
    /// </summary>
    /// <param name="sliderValue">The new volume</param>
    /// <param name="saveAfterwards">Should the settings be saved afterwards ?</param>
    public static void SetVolumeSFX(float sliderValue, bool saveAfterwards = true)
    {
        self.save.valSFX = sliderValue;
        self.mixer.SetFloat("SFX", Mathf.Log10(sliderValue) * 20);

        if (saveAfterwards) self.SaveFile();
    }


    /// <summary>
    /// Changes if the game is in fullscreen or not
    /// </summary>
    /// <param name="value">Is the game in fullscreen ?</param>
    /// <param name="saveAfterwards">Should the settings be saved afterwards ?</param>
    public static void SetFullscreen(bool value, bool saveAfterwards = true)
    {
        Screen.SetResolution(self.save.resolutionWidth, self.save.resolutionHeight, value, self.save.refreshRate);
        self.save.fullscreen = value;

        if (saveAfterwards) self.SaveFile();
    }

    /// <summary>
    /// Changes the current resolution
    /// </summary>
    /// <param name="width">The new width</param>
    /// <param name="height">The new height</param>
    /// <param name="refreshRate">The new refresh rate</param>
    /// <param name="saveAfterwards">Should the settings be saved afterwards ?</param>
    public static void SetResolution(int width, int height, int refreshRate, bool saveAfterwards = true)
    {
        Screen.SetResolution(width, height, self.save.fullscreen, refreshRate);
        self.save.refreshRate = refreshRate;
        self.save.resolutionHeight = height;
        self.save.resolutionWidth = width;

        if (saveAfterwards) self.SaveFile();
    }

    /// <summary>
    /// Returns the settings's save file
    /// </summary>
    /// <returns>The settings's save file</returns>
    public static SETTINGSAVE GetSave()
    {
        return self.save;
    }
}
