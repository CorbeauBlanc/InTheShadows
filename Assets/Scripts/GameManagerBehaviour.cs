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

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		instance = this;
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
		if ((lastUnlockedLvl = PlayerPrefs.GetString("LastUnlockedLvl", "")) != "")
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

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		cameraAnimator = Camera.main.GetComponent<Animator>();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
		Invoke("InitiateUI", cameraAnimations.introAnimation.length);
		//PlayerPrefs.SetString("LastUnlockedLvl", "test4");
		PlayerPrefs.DeleteAll();
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if (cameraAnimator.GetCurrentAnimatorStateInfo(0).IsName("Introduction") && Input.GetKeyDown("space"))
		{
			cameraAnimator.SetTrigger("skip intro");
			InitiateUI();
		}
	}

	private void SwitchToGameMode()
	{
		HideSelectionObjects();
		TurnOnLights(false);
	}

	private void TurnOnLights(bool val)
	{
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
			listLevels[i].gameObject.SetActive(true);
		UIMode = true;
	}

	public void LevelComplete()
	{
		PlayerPrefs.SetString(currentLevelCode, "true");
		ShowSelectionObjects();
		TurnOnLights(true);
		cameraAnimator.SetTrigger("look at selection");
		StartCoroutine(ShowUnlockedLvl(UnlockNextLevel()));
		currentLevelCode = "";
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
}
