using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

	public List<GameObject> notes;

	public void addNote(GameObject note) {
		notes.Add (note);
	}

	public void removeNote(GameObject note) {
		notes.Remove (note);
	}

	public void press() {
		if (notes.Count > 0) {
			GameObject note = notes [0];
			note.GetComponent<Note> ().Tap ();
		}
	}

	void Start () {
		notes = new List<GameObject> ();
	}

	// Update is called once per frame
	void Update () {
		
	}
}
