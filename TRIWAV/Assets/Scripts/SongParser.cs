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

public struct Chart {
	public int rating;
	public string difficulty;
	public List<noteSpawn> topLeftNotes;
	public List<noteSpawn> topRightNotes;
	public List<noteSpawn> bottomNotes;
}

public class Song {
	public List<BPM> bpms = new List<BPM>();
	public string title = "";
	public string artist = "";
	public string music = "";
	public float offset = 0f;
	public List<Chart> charts = new List<Chart>();
}

public class SongParser {
	public Song parse(string link)
	{
		Song result = new Song();

		StreamReader file = new StreamReader (link);
		string line;
		while ((line = file.ReadLine ()) != null) {
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
			if (flag == "#NOTES") {
				Chart thisChart = new Chart ();
				file.ReadLine (); //dance-single
				file.ReadLine(); //chart artist
				thisChart.difficulty = file.ReadLine().TrimEnd(':'); //Hard
				thisChart.rating = int.Parse(file.ReadLine().TrimEnd(':')); //18

				//Start to parse the lines of the chart

				string lineOfChart = file.ReadLine ();
				//Get rid of any leading whitespace
				while (lineOfChart == null)
					lineOfChart = file.ReadLine ();

				int measureCounter = 0;
				while (lineOfChart[0] != ';') {
					measureCounter++;
					if (lineOfChart [0] == ',') {
						Debug.Log (measureCounter);
						measureCounter = 0;
					} 
					lineOfChart = file.ReadLine ();
				}

				Debug.Log ($"{thisChart.difficulty}, level {thisChart.rating}");
				result.charts.Add (thisChart);
			}
		}

		return result;
	}
}
