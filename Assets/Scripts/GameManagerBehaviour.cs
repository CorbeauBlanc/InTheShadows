using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerBehaviour : MonoBehaviour
{

	public static GameManagerBehaviour instance { get; private set; }

	[HideInInspector] public bool UIMode = true;
	[HideInInspector] public Animator selectedUIAnimator = null;
	[HideInInspector] public GameObject selectedObject = null;
	public GameObject pointLight;
	public GameObject spotLight;

	private Animator cameraAnimator;

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
		cameraAnimator = Camera.main.GetComponent<Animator>();
		Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if (cameraAnimator.GetCurrentAnimatorStateInfo(0).IsName("Introduction"))
			return;
		else if (!Cursor.visible)
		{
			Cursor.visible = true;
			Cursor.lockState = CursorLockMode.None;
		}

		if (UIMode)
		{
			if (Input.GetButtonDown("Fire1") && selectedObject)
			{
				Instantiate(selectedObject);
				cameraAnimator.SetTrigger("look at fabric");
				selectedUIAnimator.SetBool("idle", false);
				selectedUIAnimator.SetTrigger("disappear");
			}
		}


	}

}
