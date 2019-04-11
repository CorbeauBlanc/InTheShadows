using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IlluminatedObjectBehaviour : MonoBehaviour
{
	public DifficultyType difficultyType;
	public bool isFirstObject;
	public Quaternion winningAngles;
	public Vector3 winningPosition;
	public float rotationSpeed = 2;
	public float translationSpeed = .5F;
	public float rotationMargin = 1;
	public float translationMargin = 1;
	public GameObject nextIlluminatedPiece;
	public GameObject validatedParticles = null;
	[HideInInspector] public bool isValidated = false;

	private GameObject illuminatedObject;
	private IlluminatedObjectBehaviour nextPieceBehaviour;
	private Difficulty difficulty;
	private bool mouse1Down = false;
	private bool mouse2Down = false;
	private bool isInIdleState = false;
	private Animator animator;
	private bool forceValidation = false;
	private bool particlesInstanciated = false;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		difficulty = new Difficulty[] { Difficulty.easy, Difficulty.medium, Difficulty.hard }[(int)difficultyType];
		illuminatedObject = GetComponentInChildren<MeshRenderer>().gameObject;
		animator = GetComponent<Animator>();
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if (!isInIdleState && !isValidated)
		{
			HandleKeyboardInputs();
			HandleMouseInputs();
		} else if (isInIdleState && !isValidated)
			if ((isValidated = nextPieceBehaviour.isValidated))
				ShowValidatedAnimation(false);
	}

	/// <summary>
	/// This function is called when the MonoBehaviour will be destroyed.
	/// </summary>
	void OnDestroy()
	{
		if (particlesInstanciated)
			Destroy(validatedParticles);
	}

	private void HandleKeyboardInputs()
	{
		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			rotationSpeed /= 4;
			translationSpeed /= 4;
		}
		else if (Input.GetKeyUp(KeyCode.LeftShift))
		{
			rotationSpeed *= 4;
			translationSpeed *= 4;
		}
		else if (GameManagerBehaviour.instance.TestModeEnabled() && Input.GetKeyUp(KeyCode.Return))
			forceValidation = true;
	}

	private void HandleMouseInputs()
	{
		float hAxis = Input.GetAxis("Mouse X");
		float vAxis = difficulty.VerticalRotatationEnabled ? Input.GetAxis("Mouse Y") : 0;

		mouse1Down = (Input.GetButton("Fire1") && !mouse2Down);
		mouse2Down = (Input.GetButton("Fire2") && !mouse1Down);

		if (Input.GetButtonUp("Fire1"))
		{
			illuminatedObject.transform.SetParent(null);
			transform.rotation = Quaternion.identity;
			illuminatedObject.transform.SetParent(transform);
			CheckPosition();
		} else if (Input.GetButtonUp("Fire2"))
			CheckPosition();

		if (mouse1Down && hAxis != 0 && (!difficulty.VerticalRotatationEnabled || vAxis != 0))
		{
			if (Cursor.visible)
				ShowCursor(false);
			transform.Rotate(vAxis * rotationSpeed, 0, 0, Space.Self);
			transform.Rotate(0, -hAxis * rotationSpeed, 0, Space.World);
		}
		else if (mouse2Down && difficulty.TranslationEnabled)
		{
			if (Cursor.visible)
				ShowCursor(false);
			transform.Translate(hAxis * translationSpeed, vAxis * translationSpeed, 0);
		}
		else if (!mouse1Down && !mouse2Down)
			if (!Cursor.visible)
				ShowCursor(true);
	}

	private void ShowCursor(bool val)
	{
		Cursor.visible = val;
		Cursor.lockState = val ? CursorLockMode.None : CursorLockMode.Locked;
	}

	private void ShowValidatedAnimation(bool particlesEnabled)
	{
		animator.enabled = true;
		animator.SetTrigger("validated");
		if (particlesEnabled)
		{
			validatedParticles = Instantiate(validatedParticles, transform.position, Quaternion.identity);
			particlesInstanciated = true;
		}
	}

	private void CheckPosition()
	{
		if (forceValidation || (IsInRightAngles() && IsInRightPosition()))
		{
			if (nextIlluminatedPiece)
			{
				isInIdleState = true;
				nextIlluminatedPiece = Instantiate(nextIlluminatedPiece);
				nextPieceBehaviour = nextIlluminatedPiece.GetComponent<IlluminatedObjectBehaviour>();
				nextIlluminatedPiece.transform.SetParent(transform);
			} else
			{
				isValidated = true;
				Debug.Log("WIN");
				ShowValidatedAnimation(true);
				StartCoroutine(GameManagerBehaviour.instance.LevelComplete());
			}
		}
	}

	private bool IsInRightPosition()
	{
		return (!difficulty.TranslationEnabled || (difficulty.TranslationEnabled && isFirstObject) ||
				(transform.localPosition.x > winningPosition.x - translationMargin && transform.localPosition.x < winningPosition.x + translationMargin &&
				transform.localPosition.y > winningPosition.y - translationMargin && transform.localPosition.y < winningPosition.y + translationMargin));
	}

	private bool IsInRightAngles()
	{
		int s = (int)Mathf.Sign(illuminatedObject.transform.rotation.w);
		int s2 = (int)Mathf.Sign(winningAngles.w);
		return (s * illuminatedObject.transform.rotation.x > s2 * winningAngles.x - rotationMargin &&
				s * illuminatedObject.transform.rotation.x < s2 * winningAngles.x + rotationMargin &&
				s * illuminatedObject.transform.rotation.y > s2 * winningAngles.y - rotationMargin &&
				s * illuminatedObject.transform.rotation.y < s2 * winningAngles.y + rotationMargin &&
				s * illuminatedObject.transform.rotation.z > s2 * winningAngles.z - rotationMargin &&
				s * illuminatedObject.transform.rotation.z < s2 * winningAngles.z + rotationMargin &&
				s * illuminatedObject.transform.rotation.w > s2 * winningAngles.w - rotationMargin &&
				s * illuminatedObject.transform.rotation.w < s2 * winningAngles.w + rotationMargin);
	}

}
