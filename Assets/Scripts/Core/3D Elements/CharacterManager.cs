using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Handles the characters present in the scene
/// </summary>
public class CharacterManager : MonoBehaviour
{
    private string pathToCharacters = "Characters/";
    private Dictionary<string, Character> characters;
    public static CharacterManager instance;

    void Awake()
    {
        instance = this;
        characters = new Dictionary<string, Character>();
    }

    /// <summary>
    /// Adds a new character
    /// </summary>
    /// <param name="characterName">The new character</param>
    /// <param name="startsHidden">Should the character starts hidden ?</param>
    public void AddCharacter(string characterName, bool startsHidden = true)
    {
        print("Adding Character : " + characterName);
        if (!characters.ContainsKey(characterName))
        {
            GameObject obj = Resources.Load<GameObject>(pathToCharacters + characterName);
            if (obj)
            {
                Character character = Instantiate(obj, transform).GetComponent<Character>();
                character.SetAlpha(startsHidden ? 0 : 1, true);
                characters.Add(characterName, character);
            }
        }
    }

    /// <summary>
    /// Removes a character
    /// </summary>
    /// <param name="characterName">The character's name</param>
    public void RemoveCharacter(string characterName)
    {
        if (characters.ContainsKey(characterName))
        {
            Character character = characters[characterName];
            character.UnregisterInteraction();
            characters.Remove(characterName);
            Destroy(character.gameObject);
        }
    }

    /// <summary>
    /// Removes all existing characters
    /// </summary>
    public void RemoveAllCharacters()
    {
        List<string> list = new List<string>(characters.Keys);

        foreach (string character in list)
        {
            RemoveCharacter(character);
        }
    }

    /// <summary>
    /// Returns if the character is fading or not
    /// </summary>
    /// <param name="characterName">The character</param>
    /// <returns>Is the character fading ?</returns>
    public bool IsCharacterFading(string characterName)
    {
        if (characters.ContainsKey(characterName))
        {
            return characters[characterName].fading;
        }
        return false;
    }

    /// <summary>
    /// Starts the transition of a character's alpha
    /// </summary>
    /// <param name="characterName">The character's name</param>
    /// <param name="target">The target alpha</param>
    public void TransitionCharacterAlpha(string characterName, float target)
    {
        if (characters.ContainsKey(characterName))
        {
            characters[characterName].FadeTo(target);
        }
    }

    /// <summary>
    /// Changes a character's alpha
    /// </summary>
    /// <param name="characterName">The character's name</param>
    /// <param name="target">The target alpha</param>
    public void SetCharacterAlpha(string characterName, float target)
    {
        if (characters.ContainsKey(characterName))
        {
            characters[characterName].SetAlpha(target);
        }
    }

    /// <summary>
    /// Sets if a character is talking
    /// </summary>
    /// <param name="character">The character's name</param>
    /// <param name="isTalking">Is the character talking ?</param>
    public void SetCharacterTalking(string character, bool isTalking)
    {
        if (characters.ContainsKey(character))
        {
            characters[character].SetTalking(isTalking);
        }
    }

    /// <summary>
    /// Sets a character's rotation
    /// </summary>
    /// <param name="character">The character's name</param>
    /// <param name="angle">The new rotation</param>
    public void SetCharacterRotation(string character, float angle)
    {
        if (characters.ContainsKey(character))
        {
            characters[character].SetRotation(angle);
        }
    }

    /// <summary>
    /// Sets a character's position
    /// </summary>
    /// <param name="character">The character's name</param>
    /// <param name="position">The new position</param>
    public void SetCharacterPosition(string character, Vector3 position)
    {

        if (characters.ContainsKey(character))
        {
            characters[character].SetPosition(position);
        }
    }

    /// <summary>
    /// Changes a character's mouth animations
    /// </summary>
    /// <param name="character">The character</param>
    /// <param name="animation">The new mouth animations</param>
    public void SetCharacterMouthAnimation(string character, string animation)
    {
        if (characters.ContainsKey(character))
        {
            characters[character].ChangeMouthAnimation(animation);
        }
    }

    /// <summary>
    /// Changes a character's eye animations
    /// </summary>
    /// <param name="character">The character</param>
    /// <param name="animation">The new eye animations</param>
    public void SetCharacterEyeAnimation(string character, string animation)
    {
        if (characters.ContainsKey(character))
        {
            characters[character].ChangeEyeAnimation(animation);
        }
    }

    /// <summary>
    /// Changes a character's body animations
    /// </summary>
    /// <param name="character">The character</param>
    /// <param name="animation">The new body animations</param>
    /// <param name="immediate">Should the change be immediate ?</param>
    public void SetCharacterBodyAnimation(string character, string animation, bool immediate = false)
    {
        if (characters.ContainsKey(character))
        {
            characters[character].ChangeBodyAnimation(animation, immediate);
        }
    }

    /// <summary>
    /// Adds a character from a save file
    /// </summary>
    /// <param name="characterData">The character's data</param>
    public void AddCharacterFromData(GAMEFILE.CHARACTERDATA characterData)
    {
        AddCharacter(characterData.characterName, false);
        Character character = characters[characterData.characterName];
        character.SetPosition(characterData.position);
        character.SetRotation(characterData.rotation);
        character.SetAlpha(characterData.alpha, true);
        character.ChangeBodyAnimation(characterData.bodyAnimationSet, true);
        character.ChangeEyeAnimation(characterData.eyeAnimationSet);
        character.ChangeMouthAnimation(characterData.mouthAnimationSet);
    }


    /// <summary>
    /// Copy the character's data to a list
    /// </summary>
    /// <returns>The list containing the characters data</returns>
    public List<GAMEFILE.CHARACTERDATA> SaveCharacters()
    {
        List<GAMEFILE.CHARACTERDATA> list = new List<GAMEFILE.CHARACTERDATA>();

        foreach (Character character in characters.Values)
        {
            list.Add(new GAMEFILE.CHARACTERDATA(character));
        }

        return list;
    }
}
