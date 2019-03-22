using UnityEngine;

public class Difficulty
{
	public static Difficulty easy = new Difficulty(true, false, false);
	public static Difficulty medium = new Difficulty(true, true, false);
	public static Difficulty hard = new Difficulty(true, true, true);

	public bool HorizontalRotatationEnabled;
	public bool VerticalRotatationEnabled;
	public bool TranslationEnabled;

	private Difficulty(bool hr, bool vr, bool t)
	{
		HorizontalRotatationEnabled = hr;
		VerticalRotatationEnabled = vr;
		TranslationEnabled = t;
	}
}

public enum DifficultyType
{
	Easy,
	Medium,
	Hard
}
