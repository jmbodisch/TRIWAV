using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResultsManager : MonoBehaviour {

	public void songSelect() {
		SceneManager.LoadScene ("SongSelect");
	}

	public void exit() {
		SceneManager.LoadScene ("Welcome");
	}
}
