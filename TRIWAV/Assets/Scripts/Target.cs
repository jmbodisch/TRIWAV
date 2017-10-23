using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Target : MonoBehaviour {

	public List<GameObject> notes;
	private Vector3 touchPosition;
	private float swipeResistance = 50.0f;
	private bool beingTouched;

	public void addNote(GameObject note) {
		notes.Add (note);
	}

	public void removeNote(GameObject note) {
		notes.Remove (note);
	}

	public void press() {
		if (notes.Count > 0) {
			GameObject note = notes [0];
			if (note.GetComponent<Note> ().type == "tap") {
				note.GetComponent<Note> ().Tap ();
			}
		}
	}

	public void swipe() {
		if (notes.Count > 0) {
			GameObject note = notes [0];
			if (note.GetComponent<Note> ().type == "swipe") {
				note.GetComponent<Note> ().Tap ();
			}
		}
	}

	void OnMouseDown()
	{
		press();
		touchPosition = Input.mousePosition;
		gameObject.GetComponent<SpriteRenderer> ().color = Color.blue;
		beingTouched = true;
	}

	void Start () {
		notes = new List<GameObject> ();
	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (0)) {
			gameObject.GetComponent<SpriteRenderer> ().color = Color.white;
			Vector2 deltaSwipe = touchPosition - Input.mousePosition;
			if ((Mathf.Abs(deltaSwipe.magnitude) > swipeResistance) && beingTouched) {
				swipe ();
			}
			beingTouched = false;
		}
	}
}
