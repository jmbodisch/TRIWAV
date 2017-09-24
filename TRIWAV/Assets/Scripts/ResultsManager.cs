using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResultsManager : MonoBehaviour {
	
	public Text resultText;

	void Start() {
		resultText.text = $"Perfects: {PlayerStats.perfects}\nGreats: {PlayerStats.greats}\nGoods: {PlayerStats.goods}\nPoors: {PlayerStats.poors}\n\nScore: {PlayerStats.score}";
	}

	public void songSelect() {
		SceneManager.LoadScene ("SongSelect");
	}

	public void exit() {
		SceneManager.LoadScene ("Welcome");
	}
}
