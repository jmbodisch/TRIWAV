using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {

	private GameObject target;
	public string type;
	private float lifetime;
	private float speed;
	private float timeToNote;
	private float growSpeed;

	public void Initialize(GameObject target, float timeToNote)
	{
		this.target = target;
		this.timeToNote = timeToNote;

		//Calculate total lifetime of note
		this.lifetime = 0;

		//calculate speed of note
		this.speed = (Vector3.Distance(transform.position, target.transform.position) / timeToNote);
		this.growSpeed = (10 / timeToNote);

		//Add the note to the target's list of notes
		target.GetComponent<Target> ().addNote (gameObject);
	}
		
	// Update is called once per frame
	void Update () {
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, target.transform.position, step);

		float growStep = growSpeed * Time.deltaTime;
		transform.localScale += new Vector3 (growStep, growStep, growStep);

		lifetime += Time.deltaTime;
		if (lifetime > timeToNote + Constants.GOOD) {
			GameController.judge = "Miss";
			GameController.updateHealth(-8);
			Kill (false);
		}
		
		//updateColor ();
	}

	public void Tap(){
		float timing = Mathf.Abs (timeToNote - lifetime);
		if (timing <= Constants.PERFECT) { //Perfect
			GameController.judge = "Perfect";
			GameController.score += 100;
			GameController.updateHealth (5);
			Kill (true);
		}
		else if (timing <= Constants.GREAT) { //Great
			GameController.judge = "Great";
			GameController.score += 70;
			GameController.updateHealth (3);
			Kill (true);
		}
		else if (timing <= Constants.GOOD) { //Good
			GameController.judge = "Good";
			GameController.score += 100;
			Kill (true);
		} 
		else if (timing <= Constants.POOR) {//Poor
			GameController.judge = "Poor";
			GameController.updateHealth(-4);
			Kill (false);
		}
	}

	private void Kill(bool combo) {
		if (combo)
			GameController.combo += 1;
		else
			GameController.combo = 0;
		
		target.GetComponent<Target> ().removeNote (gameObject);
		Destroy (gameObject);
	}

	//DEBUG Function.
	private void updateColor() {
		float timing = Mathf.Abs(timeToNote - lifetime);
		if (timing > Constants.POOR)
			gameObject.GetComponent<SpriteRenderer> ().color = Color.red;
		else if (timing > Constants.GOOD && timing < Constants.POOR)
			gameObject.GetComponent<SpriteRenderer> ().color = Color.blue;
		else if (timing > Constants.GREAT && timing < Constants.GOOD)
			gameObject.GetComponent<SpriteRenderer> ().color = Color.green;
		else if (timing > Constants.PERFECT)
			gameObject.GetComponent<SpriteRenderer> ().color = Color.yellow;
		else if (timing < Constants.PERFECT)
			gameObject.GetComponent<SpriteRenderer> ().color = Color.cyan;
	}
}
