using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;

public struct noteSpawn {
	float time;
	string type;
}

public class Song {
	public float bpm;
	public string title;
	public string artist;
	public string music;
	public float offset;
	public List<noteSpawn> topLeftNotes;
	public List<noteSpawn> topRightNotes;
	public List<noteSpawn> bottomNotes;
}

public class SongParser {
	public Song parse(string link)
	{
		Song result = new Song();

		StreamReader file = new StreamReader (link);
		string line;
		while ((line = file.ReadLine ()) != null && line [0] == '#') {
			string flag = line.Substring (0, line.IndexOf (':'));
			Debug.Log (flag);
		}

		return result;
	}
}
