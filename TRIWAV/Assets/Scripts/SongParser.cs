using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using System.Globalization;

public struct noteSpawn {
	public float time;
	public string type;

	public noteSpawn(float t, string s) {
		time = t;
		type = s;
	}
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
				thisChart.topLeftNotes = new List<noteSpawn> ();
				thisChart.bottomNotes = new List<noteSpawn> ();
				thisChart.topRightNotes = new List<noteSpawn> ();
				float currentTime = result.offset; //This will be used to determine the time for each note
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
				List<string> notesInMeasure = new List<string> ();
				while (lineOfChart[0] != ';') {
					if (lineOfChart [0] == ',') {
						//End of Measure, calculate note times&types and add to chart
						for (int i = 0; i < notesInMeasure.Count; i++) {
							/*
							   Todo: the code that creates noteSpawn structs and pushes them
							   to the list of notes for the chart. Adding a note will involve
							   calculating the amount of time past the current time (using BPM
							   and amount of notes in measure). Make sure to advance currentTime
							   while adding notes.
							*/

							float step = (result.bpms [0].value / 4 * notesInMeasure.Count / 60); //seconds per note
							currentTime += step;
							if (notesInMeasure[0] [0] == '1')
								thisChart.topLeftNotes.Add (new noteSpawn (currentTime, "tap"));
							if (notesInMeasure[0] [1] == '1')
								thisChart.bottomNotes.Add (new noteSpawn (currentTime, "tap"));
							if (notesInMeasure[0] [2] == '1')
								thisChart.topRightNotes.Add (new noteSpawn (currentTime, "tap"));
						}

						//Clean up measure variables
						notesInMeasure.Clear();
						measureCounter = 0;
					} else if (lineOfChart[0] != ' '){
						//just read a note, save the note string and keep counting.
						notesInMeasure.Add(lineOfChart);
						measureCounter++;
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
