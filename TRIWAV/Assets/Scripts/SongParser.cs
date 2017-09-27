using System.Collections.Generic;
using System;
using System.IO;

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

}
