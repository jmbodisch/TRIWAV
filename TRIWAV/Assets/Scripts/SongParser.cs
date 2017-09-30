using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using System.Globalization;

public struct noteSpawn {
	float time;
	string type;
}

public struct BPM {
	public float time;
	public float value;

	public BPM (float v, float t) {
		value = v;
		time = t;
	}
}

public class Song {
	public List<BPM> bpms = new List<BPM>();
	public string title;
	public string artist;
	public string music;
	public float offset;
	public List<noteSpawn> topLeftNotes = new List<noteSpawn>();
	public List<noteSpawn> topRightNotes = new List<noteSpawn>();
	public List<noteSpawn> bottomNotes = new List<noteSpawn>();
}

public class SongParser {
	public Song parse(string link)
	{
		Song result = new Song();

		StreamReader file = new StreamReader (link);
		string line;
		while ((line = file.ReadLine ()) != null && line [0] == '#') {
			string[] split = line.Split (':');
			string flag = split [0];

			if (flag == "#TITLE")
				result.title = split [1].TrimEnd (';');
			if (flag == "#ARTIST")
				result.artist = split [1].TrimEnd (';');
			if (flag == "#BPMS") {

				string bpms = split [1].TrimEnd(';');
				string[] pairs = bpms.Split (',');

				for (int i = 0; i < pairs.Length; i++) {
					string[] sides = pairs [i].Split ('=');
					//Debug.Log ($"{sides[1]} at {sides[0]}");
					float thisbpm = float.Parse(sides [1], CultureInfo.InvariantCulture.NumberFormat);
					float thistime = float.Parse(sides [0], CultureInfo.InvariantCulture.NumberFormat);
					BPM newbpm = new BPM (thisbpm, thistime);
					result.bpms.Add (newbpm);
				}
			 }
			if (flag == "#OFFSET")
				result.offset = float.Parse(split [1].TrimEnd (';'), CultureInfo.InvariantCulture.NumberFormat);
			if (flag == "#MUSIC")
				result.music = split [1].TrimEnd (';');
		}

		return result;
	}
}
