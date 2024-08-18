using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class Choice
{
    public string title;
    public List<ChoiceAnswer> answers;

    public Choice(string title)
    {
        this.title = title;
        this.answers = new List<ChoiceAnswer>();
    }

    [Serializable]
    public class ChoiceAnswer
    {
        public string choice;
        public string action;

        public ChoiceAnswer(string choice, string action)
        {
            this.choice = choice;
            this.action = action;
        }
    }
}
