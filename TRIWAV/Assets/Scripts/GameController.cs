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
	public static Chart currentChart;
	public Text scoreText, healthText, comboText, bpmText, timingText, titleText, artistText;
	public Slider healthSlider;
	private int bpmIndex = 0;
	private int stopIndex = 0;
	float currentBeat;

	public GameObject topLeft, topRight, bottom;
	public GameObject note, swipe;

	void Start()
	{
		Screen.SetResolution (800, 1280, false);

		SongParser parsetest = new SongParser ();

		enabled = false;

		currentSong = parsetest.parse ("Assets/Simfiles/Kung Fu Beat.sm");
		currentChart = currentSong.charts [0];
		AudioSource audioSource = gameObject.GetComponent<AudioSource> ();
		AudioClip audioFile = Resources.Load ("Kung Fu Beat") as AudioClip;
		while (audioFile.loadState == AudioDataLoadState.Loading) {
			Debug.Log ("Loading");
		}
		if (audioFile.loadState == AudioDataLoadState.Loaded) {
			audioSource.clip = audioFile;
			audioSource.Play ();
		} else {
			Debug.Log ("Error Loading Song");
		}

		//Debug.Log (currentSong.offset);

		enabled = true;

		score = 0;
		health = 100;
		combo = 0;
		scrollSpeed = 1f;

		bpm = currentSong.bpms[bpmIndex].value;
		currentChart = currentSong.charts [0];
		artistText.text = currentSong.artist;
		titleText.text = currentSong.title;
		judge = "";

		for (int i = 0; i < currentSong.bpms.Count; i++) {
			//Debug.Log ($"{currentSong.bpms[i].value} at {currentSong.bpms [i].time}");
		}
			
	}

	void Update()
	{
		scoreText.text = score.ToString ();
		healthText.text = health.ToString ();
		comboText.text = combo.ToString ();
		bpmText.text = bpm.ToString ();
		healthSlider.value = health;
		//timingText.text = Time.timeSinceLevelLoad.ToString();
		currentBeat += Time.deltaTime * (bpm / 60);
		timingText.text = currentBeat.ToString ();

		/*TODO: ADD TRACKER FOR CURRENT BEAT
		 * 
		 * BEAT TIMING WILL MAKE THINGS SO MUCH EASIER. TRACK THE BEAT USING THE BPM AND
		 * THE AMOUNT OF TIME THAT HAS PASSED. MAKE SURE TO ACCOUNT FOR BPM CHANGES.
		 * 
		 * ALSO, DON'T FORGET OFFSET.*/


		//Check for BPM changes
		if (bpmIndex + 1 < currentSong.bpms.Count) {
			if (currentSong.bpms [bpmIndex+1].beat <= currentBeat) {
				bpmIndex++;
				bpm = currentSong.bpms [bpmIndex].value;
			}
		}

		//check for stops
		if (stopIndex + 1 < currentSong.stops.Count) {
			if (currentSong.stops [stopIndex+1].beat <= currentBeat) {
				//convert the stop's duration into beats and subtract from current beat
				currentBeat -= currentSong.stops [stopIndex + 1].time * (bpm / 60);
				stopIndex++;
			}
		}
		
		//check for any notes to have been triggered
		if (currentChart.topLeftNotes.Count > 0 && (currentChart.topLeftNotes [0].beat - scrollSpeed) <= currentBeat) {
			makeTopLeft ();
			//Debug.Log ("Make a note at: " + currentChart.topLeftNotes[0].time.ToString());
			currentChart.topLeftNotes.RemoveAt (0);
		}

		if (currentChart.bottomNotes.Count > 0 && (currentChart.bottomNotes [0].beat - scrollSpeed) <= currentBeat) {
			makeBottom ();
			//Debug.Log ("Make a note at: " + currentChart.topLeftNotes[0].time.ToString());
			currentChart.bottomNotes.RemoveAt (0);
		}

		if (currentChart.topRightNotes.Count > 0 && (currentChart.topRightNotes [0].beat - scrollSpeed) <= currentBeat) {
			makeTopRight ();
			//Debug.Log ("Make a note at: " + currentChart.topLeftNotes[0].time.ToString());
			currentChart.topRightNotes.RemoveAt (0);
		}


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