using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

/// <summary>
/// Class responsible for constructing and showing text
/// </summary>
public class TextArchitect
{
	/// <summary>A dictionary keeping tabs on all architects present in a scene. Prevents multiple architects from influencing the same text object simultaneously.</summary>
	private static Dictionary<TextMeshProUGUI, TextArchitect> activeArchitects = new Dictionary<TextMeshProUGUI, TextArchitect>();

	private string preText;
	private string targetText;

	public int charactersPerFrame = 1;
	public float speed = 5f;

	public bool skip = false;

	public bool isConstructing { get { return buildProcess != null; } }
	Coroutine buildProcess = null;

	TextMeshProUGUI tmpro;

	public TextArchitect(TextMeshProUGUI tmpro, string targetText, string preText = "", int charactersPerFrame = 1, float speed = 5f)
	{
		this.tmpro = tmpro;
		this.targetText = targetText;
		this.preText = preText;
		this.charactersPerFrame = charactersPerFrame;
		this.speed = Mathf.Clamp(speed, 5f, 300f);

		Initiate();
	}

	/// <summary>
	/// Stops the construction
	/// </summary>
	public void Stop()
	{
		if (isConstructing)
		{
			DialogSystem.instance.StopCoroutine(buildProcess);
		}
		buildProcess = null;
	}

	/// <summary>
	/// Routine for constructing the text
	/// </summary>
	/// <returns>IEnumerator</returns>
	IEnumerator Construction()
	{
		int runsThisFrame = 0;

		tmpro.text = "";
		tmpro.text += preText;

		tmpro.ForceMeshUpdate(false);
		TMP_TextInfo inf = tmpro.textInfo;
		int vis = inf.characterCount;

		tmpro.text += targetText;

		tmpro.ForceMeshUpdate(false);
		inf = tmpro.textInfo;
		int max = inf.characterCount;

		tmpro.maxVisibleCharacters = vis;

		int cpf = charactersPerFrame;

		while (vis < max)
		{
			//allow skipping by increasing the characters per frame and the speed of occurance.
			if (skip)
			{
				speed = 1;
				charactersPerFrame = charactersPerFrame < 5 ? 5 : charactersPerFrame + 3;
			}

			//reveal a certain number of characters per frame.
			while (runsThisFrame < charactersPerFrame)
			{
				vis++;
				tmpro.maxVisibleCharacters = vis;
				runsThisFrame++;
			}

			//wait for the next available revelation time.
			runsThisFrame = 0;
			yield return new WaitForSeconds(0.01f * speed);
		}

		//terminate the architect and remove it from the active log of architects.
		Terminate();
	}

	/// <summary>
	/// Starts constructing the text
	/// </summary>
	void Initiate()
	{
		//check if an architect for this text object is already running. if it is, terminate it. Do not allow more than one architect to affect the same text object at once.
		TextArchitect existingArchitect;
		if (activeArchitects.TryGetValue(tmpro, out existingArchitect))
			existingArchitect.Terminate();

		buildProcess = DialogSystem.instance.StartCoroutine(Construction());
		activeArchitects.Add(tmpro, this);
	}

	/// <summary>
	/// Terminate this architect. Stops the text generation process and removes it from the cache of all active architects.
	/// </summary>
	public void Terminate()
	{
		activeArchitects.Remove(tmpro);
		if (isConstructing)
			DialogSystem.instance.StopCoroutine(buildProcess);
		buildProcess = null;
	}

	/// <summary>
	/// Forces the architect to finish constructing
	/// </summary>
	public void ForceFinish()
	{
		tmpro.maxVisibleCharacters = tmpro.text.Length;
		Terminate();
	}

	/// <summary>
	/// Continue constructing
	/// </summary>
	/// <param name="targ">The target string</param>
	/// <param name="pre">The current string</param>
	public void Renew(string targ, string pre)
	{
		targetText = targ;
		preText = pre;

		skip = false;

		if (isConstructing)
		{
			DialogSystem.instance.StopCoroutine(buildProcess);
		}
		buildProcess = DialogSystem.instance.StartCoroutine(Construction());
	}

	/// <summary>
	/// Shows the text
	/// </summary>
	/// <param name="text">The text</param>
	public void ShowText(string text)
	{
		if (isConstructing)
		{
			DialogSystem.instance.StopCoroutine(buildProcess);
		}
		targetText = text;
		tmpro.text = text;

		tmpro.maxVisibleCharacters = tmpro.text.Length;

		if (tmpro == DialogSystem.instance.speechText)
		{
			DialogSystem.instance.targetSpeech = text;
		}
	}
}
