using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SongSelectManager : MonoBehaviour {

	public void goToSong() {
		SceneManager.LoadScene ("GamePlay");
	}

	public void back() {
		SceneManager.LoadScene ("Welcome");
	}
}
