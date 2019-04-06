using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionObjectBehaviour : MonoBehaviour
{

	public string levelName;
	public GameObject relatedIlluminatedObject;

	private List<Material> objectsMaterial = new List<Material>();
	private Animator objectAnimator;
	private bool isSelected = false;

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		Renderer[] listRend = GetComponentsInChildren<Renderer>();
		foreach (Renderer rnd in listRend)
			objectsMaterial.Add(rnd.material);
		objectAnimator = GetComponent<Animator>();
	}

	/// <summary>
	/// Update is called every frame, if the MonoBehaviour is enabled.
	/// </summary>
	void Update()
	{
		if (!GameManagerBehaviour.instance.UIMode)
			return;

		if (Input.GetButtonUp("Fire1") && isSelected)
		{
			isSelected = false;
			foreach (Material mat in objectsMaterial)
				mat.DisableKeyword("_EMISSION");
			GameManagerBehaviour.instance.LaunchSelectedLvl();
		}
	}

	/// <summary>
	/// Called every frame while the mouse is over the GUIElement or Collider.
	/// </summary>
	void OnMouseEnter()
	{
		if (GameManagerBehaviour.instance.UIMode)
		{
			isSelected = true;
			GameManagerBehaviour.instance.selectedObject = relatedIlluminatedObject;
			GameManagerBehaviour.instance.selectedUIAnimator = objectAnimator;
			GameManagerBehaviour.instance.currentLevelName = levelName;
			foreach (Material mat in objectsMaterial)
				mat.EnableKeyword("_EMISSION");
		}
	}

	/// <summary>
	/// Called when the mouse is not any longer over the GUIElement or Collider.
	/// </summary>
	void OnMouseExit()
	{
		if (GameManagerBehaviour.instance.UIMode)
		{
			isSelected = false;
			GameManagerBehaviour.instance.selectedObject = null;
			GameManagerBehaviour.instance.selectedUIAnimator = null;
			GameManagerBehaviour.instance.currentLevelName = "";
			foreach (Material mat in objectsMaterial)
				mat.DisableKeyword("_EMISSION");
		}
	}

}
