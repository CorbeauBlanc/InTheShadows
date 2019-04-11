using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PostProcessing : MonoBehaviour
{

	[HideInInspector] public Material mat;

	private bool blur = true;

	void OnRenderImage(RenderTexture src, RenderTexture dst)
	{
		if (blur)
		{
			RenderTexture tmp = RenderTexture.GetTemporary(src.width, src.height);
			Graphics.Blit(src, tmp, mat, 0);
			Graphics.Blit(tmp, dst, mat, 1);
		}
		else
			Graphics.Blit(src, dst);
	}

	public void BlurEnabled(bool val)
	{
		blur = val;
	}

}
