using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {

	private Transform target;
	private float lifetime;
	private float speed;
	private float growSpeed;

	public void Initialize(Transform target, float lifetime)
	{
		this.target = target;
		this.lifetime = lifetime + Constants.POOR;

		//calculate speed of note
		this.speed = (Vector3.Distance(transform.position, target.position) / lifetime);
		this.growSpeed = (10 / lifetime);
	}
		
	// Update is called once per frame
	void Update () {
		float step = speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards (transform.position, target.position, step);

		float growStep = growSpeed * Time.deltaTime;
		transform.localScale += new Vector3 (growStep, growStep, growStep);

		lifetime -= Time.deltaTime;
		if (lifetime <= Constants.POOR * -1)
			Kill ();
		
		float timing = Mathf.Abs (lifetime);
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

	public void Kill(){
		float timing = Mathf.Abs (lifetime);
		if (timing < Constants.POOR) {
			if (timing < Constants.GOOD)
				GameController.combo++;
			else
				GameController.combo = 0;
			Destroy (gameObject);
			GameController.combo = 0;
		}
	}
}
