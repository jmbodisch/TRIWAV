using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Note : MonoBehaviour {

	private Transform target;
	private double lifetime;

	public Note(Transform target, double lifetime)
	{
		this.target = target;
		this.lifetime = lifetime;
	}

	void Start() {
		//lifetime = GameController.scrollSpeed;
	}
		
	// Update is called once per frame
	void Update () {
		lifetime -= Time.deltaTime;
		if (lifetime <= 0)
			Kill ();
	}

	public void Kill(){
		Destroy(gameObject);
	}
}
