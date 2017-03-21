using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public static double score;
	public static int health;
	public static int combo;
	public static int bpm;
	public static float scrollSpeed;
	public GUIText scoreText, healthText, comboText, bpmText;

	public GameObject topLeft, topRight, bottom;
	public GameObject note;

	public List<GameObject> topLeftNotes;
	private List<GameObject> topRightNotes;
	private List<GameObject> bottomNotes;

	void Start()
	{
		score = 0;
		health = 100;
		combo = 0;
		scrollSpeed = .25f;
		bpm = 90;

		topLeftNotes = new List<GameObject> ();
		topRightNotes = new List<GameObject> ();
		bottomNotes = new List<GameObject> ();
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
			topLeftPress ();
		if (Input.GetKeyDown (KeyCode.H))
			topRightPress ();
		if (Input.GetKeyDown (KeyCode.B))
			bottomPress ();
	}

	private void makeTopLeft()
	{
		var newNote = Instantiate (note);
		newNote.GetComponent<Note>().Initialize(topLeft.transform, scrollSpeed);
		topLeftNotes.Add (newNote);
	}

	private void makeTopRight()
	{
		var newNote = Instantiate (note);
		newNote.GetComponent<Note>().Initialize(topRight.transform, scrollSpeed);
		topRightNotes.Add (newNote);
	}

	private void makeBottom()
	{
		var newNote = Instantiate (note);
		newNote.GetComponent<Note>().Initialize(bottom.transform, scrollSpeed);
		bottomNotes.Add (newNote);
	}

	private void topLeftPress()
	{
		if (topLeftNotes.Count > 0) {
			GameObject note = topLeftNotes [0];
			topLeftNotes.Remove (note);
			note.GetComponent<Note> ().Kill ();
		}
	}

	private void bottomPress()
	{
		if (bottomNotes.Count > 0) {
			GameObject note = bottomNotes [0];
			bottomNotes.Remove (note);
			note.GetComponent<Note> ().Kill ();
		}
	}

	private void topRightPress()
	{
		if (topRightNotes.Count > 0) {
			GameObject note = topRightNotes [0];
			topRightNotes.Remove (note);
			note.GetComponent<Note> ().Kill ();
		}
	}
}