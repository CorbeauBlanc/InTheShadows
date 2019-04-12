using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerBehaviour : MonoBehaviour
{

	public static GameManagerBehaviour instance { get; private set; }

	[System.Serializable] public class CameraAnimations
	{
		public AnimationClip introAnimation;
		public AnimationClip UIInAnimation;
		public AnimationClip UIOutAnimation;
	}
	[System.Serializable] public class UIObjectAnimations
	{
		public AnimationClip appearAnimation;
		public AnimationClip disappearAnimation;
	}

	public AnimationClip validatedObjAnimation;

	[HideInInspector] public CameraAnimations cameraAnimations = new CameraAnimations();
	public UIObjectAnimations uIObjectAnimations = new UIObjectAnimations();
	[HideInInspector] public bool UIMode = false;
	[HideInInspector] public Animator selectedUIAnimator = null;
	[HideInInspector] public GameObject selectedObject = null;
	[HideInInspector] public string currentLevelCode;


	public GameObject pointLight;
	public GameObject spotLight;
	public SelectionObjectBehaviour[] listLevels;

	private Animator cameraAnimator;
	private GameObject currentLevelObject;
	private bool uIInitiated = false;

	private bool testMode = false;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		instance = this;
		cameraAnimator = Camera.main.GetComponent<Animator>();
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if (cameraAnimator.enabled && cameraAnimator.GetCurrentAnimatorStateInfo(0).IsName("Introduction") && Input.GetKeyDown(KeyCode.Space))
		{
			cameraAnimator.SetTrigger("skip intro");
			InitiateUI();
		}
	}

	private bool EnableLevel(int lvlNb, string lvlName)
	{
		if (lvlNb == listLevels.Length)
			return false;
		listLevels[lvlNb].gameObject.SetActive((listLevels[lvlNb].levelCode == lvlName) || EnableLevel(lvlNb + 1, lvlName));
		return listLevels[lvlNb].gameObject.activeSelf;
	}

	private void ShowSelectionObjects()
	{
		string lastUnlockedLvl;

		listLevels[0].gameObject.SetActive(true);
		if ((lastUnlockedLvl = testMode ? listLevels[listLevels.Length - 1].levelCode : PlayerPrefs.GetString("LastUnlockedLvl", "")) != "")
			EnableLevel(1, lastUnlockedLvl);
	}

	private void HideSelectionObjects()
	{
		foreach (SelectionObjectBehaviour lvl in listLevels)
			lvl.gameObject.SetActive(false);
	}

	private void InitiateUI()
	{
		if (!uIInitiated)
		{
			uIInitiated = true;
			ShowSelectionObjects();
			UIMode = true;
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}
	}

	private void SwitchToGameMode()
	{
		HideSelectionObjects();
		TurnOnLights(false);
	}

	private void TurnOnLights(bool val)
	{
		SoundManagerBehaviour.instance.PlayLampSound();
		pointLight.SetActive(val);
		spotLight.SetActive(!val);
	}

	private int UnlockNextLevel()
	{
		for (int i = 0; i < listLevels.Length; i++)
			if ((listLevels[i].levelCode == currentLevelCode) && (i + 1 != listLevels.Length) && (!listLevels[i + 1].gameObject.activeSelf))
			{
				PlayerPrefs.SetString("LastUnlockedLvl", listLevels[i + 1].levelCode);
				return i + 1;
			}
		return -1;
	}

	private IEnumerator ShowUnlockedLvl(int i)
	{
		yield return new WaitForSeconds(cameraAnimations.UIInAnimation.length + 1);
		if (i > -1)
		{
			listLevels[i].gameObject.SetActive(true);
			SoundManagerBehaviour.instance.PlayPopSound();
		}
		UIMode = true;
	}

	public bool TestModeEnabled()
	{
		return testMode;
	}

	public void DeletePlayerData()
	{
		PlayerPrefs.DeleteAll();
	}

	public void StartGame(bool testModeEnabled)
	{
		SoundManagerBehaviour.instance.StartMusicGame();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		testMode = testModeEnabled;
		Invoke("InitiateUI", cameraAnimations.introAnimation.length);
		Camera.main.GetComponent<PostProcessing>().BlurEnabled(false);
		cameraAnimator.enabled = true;
	}

	public void GoToSelection()
	{
		MainMenuBehaviour.instance.ShowMainMenu(false);
		Destroy(currentLevelObject);
		TurnOnLights(true);
		ShowSelectionObjects();
		cameraAnimator.SetTrigger("look at selection");
		StartCoroutine(ShowUnlockedLvl(UnlockNextLevel()));
		currentLevelCode = "";
	}

	public IEnumerator LevelComplete()
	{
		PlayerPrefs.SetString(currentLevelCode, "true");
		SoundManagerBehaviour.instance.PlayValidatedSound();
		yield return new WaitForSeconds(1 * validatedObjAnimation.length);
		MainMenuBehaviour.instance.ShowMainMenu(true);
	}

	public void LaunchSelectedLvl()
	{
		UIMode = false;
		currentLevelObject = Instantiate(selectedObject);
		selectedUIAnimator.SetTrigger("disappear");
		cameraAnimator.SetTrigger("look at fabric");
		selectedObject = null;
		Invoke("SwitchToGameMode", cameraAnimations.UIOutAnimation.length);
	}

	public void QuitGame()
	{
		Debug.Log("QUIT");
		Application.Quit();
	}
}
