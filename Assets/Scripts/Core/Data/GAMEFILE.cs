using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[System.Serializable]
public class GAMEFILE
{
    public string chapterName;
    public int chapterProgress = 0;

    public string currentTextSystemDisplayText = "";
    public string currentTextSystemSpeakerDisplayText = "";

    public List<CHARACTERDATA> characterInScene = null;

    public string background;

    public string music;

    public Choice currentChoice;

    public List<ITEM> items;

    public float fadeBg;
    public float fadeFg;
    public bool interactionMode;
    public List<INTERACTABLEDATA> interactables;

    public Vector3 cameraPosition;
    public Vector3 cameraRotation;

    public GAMEFILE()
    {
        this.chapterName = "test2";
        this.chapterProgress = 0;
        this.characterInScene = new List<CHARACTERDATA>();
        this.background = null;
        this.music = null;
        this.currentChoice = null;
        this.items = new List<ITEM>();
        this.fadeBg = 0;
        this.fadeFg = 0;
        this.interactionMode = false;
        this.interactables = new List<INTERACTABLEDATA>();
        this.cameraPosition = new Vector3(0, 0, -10);
        this.cameraRotation = Vector3.zero;
    }

    [System.Serializable]
    public class CHARACTERDATA
    {
        public string characterName;
        public string bodyAnimationSet = "";
        public string eyeAnimationSet = "";
        public string mouthAnimationSet = "";
        public Vector3 position;
        public float rotation;
        public float alpha;

        public CHARACTERDATA(Character character)
        {
            characterName = character.characterName;
            position = character.characterPosition;
            rotation = character.characterRotation;
            alpha = character.characterAlpha;
            bodyAnimationSet = character.bodyAnimationSet;
            eyeAnimationSet = character.eyeAnimationSet;
            mouthAnimationSet = character.mouthAnimationSet;
        }
    }

    [System.Serializable]
    public class INTERACTABLEDATA
    {
        public string ID;
        public string chapter;
        public bool hidden;

        public INTERACTABLEDATA(string ID, string chapter, bool hidden)
        {
            this.ID = ID;
            this.chapter = chapter;
            this.hidden = hidden;
        }
    }

    [System.Serializable]
    public class ITEM
    {
        public string name;
        public string value;
        public string defaultValue;

        public ITEM(string key, string defaultValue)
        {
            this.name = key;
            this.value = defaultValue;
            this.defaultValue = defaultValue;
        }
    }
}
