﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}

	public void GoToSongSelect() {
		SceneManager.LoadScene ("SongSelect");
	}

	// Update is called once per frame
	void Update () {
		
	}
}
