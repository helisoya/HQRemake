using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class ChoiceScreen : MonoBehaviour
{
    public static ChoiceScreen instance;

    [Header("Choice Screen")]
    [SerializeField] private GameObject root;
    [SerializeField] private TtileHeader header;
    [SerializeField] private GameObject awnserPrefab;
    [SerializeField] private Transform answersRoot;
    [SerializeField] private VerticalLayoutGroup layoutGroup;

    public bool isWaitingForChoiceToBeMade { get { return isShowingChoices && chosenIndex == -1; } }
    public bool isShowingChoices { get { return showChoices != null; } }
    public int chosenIndex { get; private set; }

    private Coroutine showChoices = null;


    void Awake()
    {
        instance = this;
        Hide();
    }

    public void Show(Choice choice)
    {
        instance.root.SetActive(true);
        if (!string.IsNullOrEmpty(choice.title)) header.Show(choice.title);
        else header.Hide();

        if (isShowingChoices)
        {
            instance.StopCoroutine(showChoices);
        }
        ClearAllCurrentChoices();
        showChoices = instance.StartCoroutine(ShowingChoices(choice.answers));
    }

    public void Hide()
    {
        if (isShowingChoices)
        {
            instance.StopCoroutine(showChoices);
        }
        showChoices = null;
        header.Hide();
        ClearAllCurrentChoices();
        instance.root.SetActive(false);
    }

    void ClearAllCurrentChoices()
    {
        foreach (Transform child in answersRoot)
        {
            Destroy(child.gameObject);
        }
    }


    public IEnumerator ShowingChoices(List<Choice.ChoiceAnswer> answers)
    {
        yield return new WaitForEndOfFrame();
        chosenIndex = -1;

        while (header.isRevealing)
        {
            yield return new WaitForEndOfFrame();
        }

        for (int i = 0; i < answers.Count; i++)
        {
            CreateChoice(answers[i].choice, i);
        }

        SetLayoutSpacing();

        while (isWaitingForChoiceToBeMade)
        {
            yield return new WaitForEndOfFrame();
        }
        Hide();
    }

    void SetLayoutSpacing()
    {
        int i = answersRoot.childCount;
        if (i <= 3)
        {
            instance.layoutGroup.spacing = 20;
        }
        else if (i >= 7)
        {
            instance.layoutGroup.spacing = 1;
        }
        else
        {
            switch (i)
            {
                case 4:
                    instance.layoutGroup.spacing = 15;
                    break;
                case 5:
                    instance.layoutGroup.spacing = 10;
                    break;
                case 6:
                    instance.layoutGroup.spacing = 5;
                    break;
            }
        }
    }

    void CreateChoice(string choice, int index)
    {
        ChoiceButton button = Instantiate(awnserPrefab, answersRoot).GetComponent<ChoiceButton>();
        button.Init(choice, index);
    }



    public void MakeChoice(int index)
    {
        chosenIndex = index;
    }
}
