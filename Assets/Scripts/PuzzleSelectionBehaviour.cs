using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PuzzleSelectionBehaviour : MonoBehaviour
{

	public static PuzzleSelectionBehaviour instance { get; private set; }

	public GameObject puzzleInfos;
	public Text puzzleTitle;
	public Text puzzleDifficulty;
	public string solvedText;
	public Color solvedColor;

	private Color defaultDifficultyColor;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		instance = this;
	}

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		defaultDifficultyColor = puzzleDifficulty.color;
	}

	public void ShowPuzzleInfos(string title, string difficulty, bool hasBeenSolved)
	{
		puzzleTitle.text = title;
		puzzleDifficulty.text = hasBeenSolved ? solvedText : difficulty;
		puzzleDifficulty.color = hasBeenSolved ? solvedColor : defaultDifficultyColor;
		puzzleInfos.SetActive(true);
	}

	public void HidePuzzleInfos()
	{
		puzzleInfos.SetActive(false);
	}

}
