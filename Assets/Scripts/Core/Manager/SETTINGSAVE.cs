using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[System.Serializable]
public class SETTINGSAVE
{
    public string language;
    public float valBGM = 1;
    public float valSFX = 1;
    public float valVOICE = 1;
    public int resolutionWidth;
    public int resolutionHeight;
    public int refreshRate;
    public bool fullscreen;
    public int quality;

    public SETTINGSAVE()
    {
        quality = 2;
        valBGM = 0.2f;
        valSFX = 0.2f;
        valVOICE = 0.3f;
        fullscreen = true;
        language = "eng";
    }
}
