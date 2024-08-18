using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChoiceButton : MonoBehaviour
{
    [SerializeField] private LocalizedText localText;
    private int choiceIndex = -1;

    public void Init(string textId, int index)
    {
        localText.SetNewKey(textId);
        choiceIndex = index;
    }

    public int GetChoiceIndex()
    {
        return choiceIndex;
    }

    public void SetChoiceIndex(int choiceIndex)
    {
        this.choiceIndex = choiceIndex;
    }

    public LocalizedText GetLocalizedText()
    {
        return localText;
    }

    public void Event_OnClick()
    {
        ChoiceScreen.instance.MakeChoice(choiceIndex);
    }
}
