using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IlluminatedObjectBehaviour : MonoBehaviour
{
	public DifficultyType difficultyType;
	public Vector3 winningAngles;
	public Vector3 winningPosition;
	public float rotationSpeed = 2;
	public float translationSpeed = .5F;
	public float rotationMargin = 1;
	public float translationMargin = 1;
	public GameObject nextIlluminatedPiece;
	public bool isValidated = false;

	private GameObject illuminatedObject;
	private IlluminatedObjectBehaviour nextPieceBehaviour;
	private Difficulty difficulty;
	private bool mouse1Down = false;
	private bool mouse2Down = false;
	private Vector3 tmpRotations;
	private bool isInIdleState = false;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		difficulty = new Difficulty[] { Difficulty.easy, Difficulty.medium, Difficulty.hard }[(int)difficultyType];
		illuminatedObject = GetComponentInChildren<MeshRenderer>().gameObject;
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
			SetTmpRotations();
			if (IsInRightPosition())
			{
				if (nextIlluminatedPiece)
				{
					isInIdleState = true;
					nextIlluminatedPiece = Instantiate(nextIlluminatedPiece);
					nextPieceBehaviour = nextIlluminatedPiece.GetComponent<IlluminatedObjectBehaviour>();
				} else
				{
					isValidated = true;
					Debug.Log("WIN");
				}
			}
		} else if (isInIdleState)
			isValidated = nextPieceBehaviour.isValidated;
	}

	private void HandleKeyboardInputs()
	{
		if (Input.GetKeyDown("left shift"))
		{
			rotationSpeed /= 4;
			translationSpeed /= 4;
		}
		else if (Input.GetKeyUp("left shift"))
		{
			rotationSpeed *= 4;
			translationSpeed *= 4;
		}
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
		}

		if (mouse1Down && hAxis != 0 && (!difficulty.VerticalRotatationEnabled || vAxis != 0))
		{
			transform.Rotate(vAxis * rotationSpeed, 0, 0, Space.Self);
			transform.Rotate(0, -hAxis * rotationSpeed, 0, Space.World);
		}
		else if (mouse2Down && difficulty.TranslationEnabled)
			transform.Translate(hAxis * translationSpeed, vAxis * translationSpeed, 0);
	}

	private void SetTmpRotations()
	{
		tmpRotations.Set(illuminatedObject.transform.eulerAngles.x > 180 ? illuminatedObject.transform.eulerAngles.x - 360 :
																			illuminatedObject.transform.eulerAngles.x,
						illuminatedObject.transform.eulerAngles.y > 180 ? illuminatedObject.transform.eulerAngles.y - 360 :
																			illuminatedObject.transform.eulerAngles.y,
						illuminatedObject.transform.eulerAngles.z > 180 ? illuminatedObject.transform.eulerAngles.z - 360 :
																			illuminatedObject.transform.eulerAngles.z);
	}

	private bool IsInWinningMargins(Vector3 data1, Vector3 winningData, float margin)
	{
		return (data1.x > winningData.x - margin && data1.x < winningData.x + margin &&
				data1.y > winningData.y - margin && data1.y < winningData.y + margin);
	}

	private bool IsInRightPosition()
	{
		return (IsInWinningMargins(tmpRotations, winningAngles, rotationMargin) &&
				(!difficulty.TranslationEnabled || IsInWinningMargins(transform.position, winningPosition, translationMargin)));
	}

}
