using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

	public static double score;
	public static int health;
	public static int combo;
	public static float bpm;
	public static float scrollSpeed;
	public static string judge;
	public static Song currentSong;
	public Text scoreText, healthText, comboText, bpmText, timingText, titleText, artistText;
	public Slider healthSlider;

	public GameObject topLeft, topRight, bottom;
	public GameObject note, swipe;

	void Start()
	{
		Screen.SetResolution (800, 1280, false);

		SongParser parsetest = new SongParser ();
		currentSong = parsetest.parse ("Assets/Simfiles/50 Ways To Say Goodbye.sm");

		score = 0;
		health = 100;
		combo = 0;
		scrollSpeed = 1f;
		bpm = currentSong.bpm;
		bpmText.text = bpm.ToString ();
		artistText.text = currentSong.artist;
		titleText.text = currentSong.title;
		judge = "";
	}

	void Update()
	{
		scoreText.text = score.ToString ();
		healthText.text = health.ToString ();
		comboText.text = combo.ToString ();
		healthSlider.value = health;
		timingText.text = judge;

		//Keyboard handlers
		if(Input.GetKeyDown (KeyCode.A))
			makeTopLeft ();
		if (Input.GetKeyDown (KeyCode.S))
			makeTopRight ();
		if (Input.GetKeyDown (KeyCode.Z))
			makeBottom ();

		if(Input.GetKeyDown (KeyCode.D))
			makeTopLeftSwipe ();
		if (Input.GetKeyDown (KeyCode.F))
			makeTopRightSwipe ();
		if (Input.GetKeyDown (KeyCode.C))
			makeBottomSwipe ();

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

	private void makeTopLeftSwipe()
	{
		var newNote = Instantiate (swipe);
		newNote.GetComponent<Note>().Initialize(topLeft, scrollSpeed);
	}

	private void makeTopRightSwipe()
	{
		var newNote = Instantiate (swipe);
		newNote.GetComponent<Note>().Initialize(topRight, scrollSpeed);
	}

	private void makeBottomSwipe()
	{
		var newNote = Instantiate (swipe);
		newNote.GetComponent<Note>().Initialize(bottom, scrollSpeed);
	}

	static void fail()
	{
		SceneManager.LoadScene ("Results");
	}

	public static void updateHealth(int delta)
	{
		health += delta;
		if (health > 100)
			health = 100;
		if (health <= 0) {
			health = 0;
			fail ();
		}
	}
}