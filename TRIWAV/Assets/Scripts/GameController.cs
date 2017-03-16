using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static double score;
	public static int health;
	public static int combo;
	public static int bpm;
	public static double scrollSpeed;
	public GUIText scoreText, healthText, comboText, bpmText;

	void Start()
	{
		score = 0;
		health = 100;
		combo = 0;
		scrollSpeed = 10;
		bpm = 90;

		Instantiate (new Note (GameObject.Find("TopLeft").transform, scrollSpeed));
	}

	void Update()
	{
		scoreText.text = score.ToString ();
		healthText.text = health.ToString ();
		comboText.text = combo.ToString ();
		bpmText.text = bpm.ToString ();
	}
}
