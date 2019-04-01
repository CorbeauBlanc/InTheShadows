using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionObjectBehaviour : MonoBehaviour
{

	public GameObject relatedIlluminatedObject;

	private List<Material> objectsMaterial = new List<Material>();
	private Animator objectAnimator;

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
	/// Called every frame while the mouse is over the GUIElement or Collider.
	/// </summary>
	void OnMouseEnter()
	{
		if (GameManagerBehaviour.instance.UIMode)
		{
			GameManagerBehaviour.instance.selectedObject = relatedIlluminatedObject;
			GameManagerBehaviour.instance.selectedUIAnimator = objectAnimator;
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
			GameManagerBehaviour.instance.selectedObject = null;
			GameManagerBehaviour.instance.selectedUIAnimator = null;
			foreach (Material mat in objectsMaterial)
				mat.DisableKeyword("_EMISSION");
		}
	}

}
