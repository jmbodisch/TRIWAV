using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static double score;
	public static int health;
	public static int combo;
	public static int bpm;
	public static float scrollSpeed;
	public GUIText scoreText, healthText, comboText, bpmText, timingText;

	public GameObject topLeft, topRight, bottom;
	public GameObject note;

	void Start()
	{
		Screen.SetResolution (800, 1280, false);
		score = 0;
		health = 100;
		combo = 0;
		scrollSpeed = 1f;
		bpm = 90;
	}

	void Update()
	{
		scoreText.text = score.ToString ();
		healthText.text = health.ToString ();
		comboText.text = combo.ToString ();
		bpmText.text = bpm.ToString ();

		//Keyboard handlers
		if(Input.GetKeyDown (KeyCode.A))
			makeTopLeft ();
		if (Input.GetKeyDown (KeyCode.S))
			makeTopRight ();
		if (Input.GetKeyDown (KeyCode.Z))
			makeBottom ();

		if (Input.GetKeyDown (KeyCode.G))
			topLeft.GetComponent<Target> ().press ();
		if (Input.GetKeyDown (KeyCode.H))
			topRight.GetComponent<Target> ().press ();
		if (Input.GetKeyDown (KeyCode.B))
			bottom.GetComponent<Target> ().press ();
	}

	private void makeTopLeft()
	{
		var newNote = Instantiate (note);
		newNote.GetComponent<Note>().Initialize(topLeft, scrollSpeed);
	}

	private void makeTopRight()
	{
		var newNote = Instantiate (note);
		newNote.GetComponent<Note>().Initialize(topRight, scrollSpeed);
	}

	private void makeBottom()
	{
		var newNote = Instantiate (note);
		newNote.GetComponent<Note>().Initialize(bottom, scrollSpeed);
	}

	public void updateTimingWindow(string judge)
	{
		timingText.text = judge;
	}
}