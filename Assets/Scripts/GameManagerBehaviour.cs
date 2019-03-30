using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManagerBehaviour : MonoBehaviour
{

	public GameObject PointLight;
	public GameObject SpotLight;

	private Animator cameraAnimator;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		cameraAnimator = Camera.main.GetComponent<Animator>();
	}

}
