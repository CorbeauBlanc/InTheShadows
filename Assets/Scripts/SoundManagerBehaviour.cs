using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManagerBehaviour : MonoBehaviour
{

	public static SoundManagerBehaviour instance { get; private set; }

	public AudioSource natureSounds;
	public AudioSource mainMusic;
	public AudioSource clickSound;
	public AudioSource validatedSound;
	public AudioSource popSound;
	public AudioSource woodenClickSound;
	public AudioSource paperSound;
	public AudioSource lampSound;




	private bool musicEnabled = true;
	private bool soundEnabled = true;
	private bool isGameStarted = false;

	/// <summary>
	/// Awake is called when the script instance is being loaded.
	/// </summary>
	void Awake()
	{
		instance = this;
	}

	public void StartMusicGame()
	{
		isGameStarted = true;
		if (!musicEnabled)
			return;
		natureSounds.loop = false;
		mainMusic.Play();
	}

	public void PlayClickSound()
	{
		if (soundEnabled)
			clickSound.Play();
	}

	public void PlayValidatedSound()
	{
		if (soundEnabled)
			validatedSound.Play();
	}

	public void PlayPopSound()
	{
		if (soundEnabled)
			popSound.Play();
	}

	public void PlayWoodenClickSound()
	{
		if (soundEnabled)
			woodenClickSound.Play();
	}

	public void PlayPaperSound()
	{
		if (soundEnabled)
			paperSound.Play();
	}

	public void PlayLampSound()
	{
		if (soundEnabled)
			lampSound.Play();
	}

	public void SwitchSoundEnabled()
	{
		soundEnabled = !soundEnabled;
	}

	public void SwitchMusicEnabled()
	{
		if (!(musicEnabled = !musicEnabled))
		{
			if (natureSounds.isPlaying)
				natureSounds.Pause();
			if (mainMusic.isPlaying)
				mainMusic.Pause();
		}
		else
		{
			if (isGameStarted)
				mainMusic.Play();
			else
				natureSounds.Play();
		}
	}

}
