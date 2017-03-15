using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour {

	public double score;
	public int health;
	public int combo;
	public int bpm;
	public int scrollSpeed;

	void Start()
	{
		score = 0;
		health = 100;
	}
}
