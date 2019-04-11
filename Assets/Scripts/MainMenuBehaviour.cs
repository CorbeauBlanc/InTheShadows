using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuBehaviour : MonoBehaviour
{

	public static MainMenuBehaviour instance { get; private set; }

	public GameObject panel;

	private PostProcessing camEffects;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		instance = this;
		camEffects = Camera.main.GetComponent<PostProcessing>();
	}

	public void ShowMainMenu(bool val)
	{
		camEffects.BlurEnabled(val);
		panel.SetActive(val);
	}

}
