using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine;
using System.Globalization;

public struct noteSpawn {
	public float beat;
	public string type;

	public noteSpawn(float b, string s) {
		beat = b;
		type = s;
	}
}

public class BPM {
	public float beat;
	public float value;

	public BPM (float v, float b) {
		value = v;
		beat = b;
	}
}

public class Stop {
	public float beat;
	public float time;

	public Stop (float b, float t) {
		beat = b;
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
	public List<Stop> stops = new List<Stop>();
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
					float thisbeat = float.Parse(sides [0], CultureInfo.InvariantCulture.NumberFormat);
					BPM newbpm = new BPM (thisbpm, thisbeat);
					result.bpms.Add (newbpm);
				}
			 }
			if (flag == "#OFFSET")
				result.offset = float.Parse(split [1].TrimEnd (';'), CultureInfo.InvariantCulture.NumberFormat);
			if (flag == "#MUSIC")
				result.music = split [1].TrimEnd (';');
			if (flag == "#STOPS") {
				Debug.Log ("Got to stops");
				List<string> stopStrings = new List<string> ();

				stopStrings.Add (split [1]);

				string nextLn = file.ReadLine ();
				while (nextLn[0] != ';') {
					stopStrings.Add (nextLn);
					Debug.Log (nextLn);
				}

				for (int i = 0; i < stopStrings.Count; i++) {
					float beat, time;
					string[] splitStop = stopStrings [i].Split ('=');
					beat = float.Parse (splitStop [0]);
					time = float.Parse (splitStop [1]);
					result.stops.Add (new Stop (beat, time));
					Debug.Log ($"Added {time} stop at {beat}");
				}
			}
			if (flag == "#NOTES") {
				Chart thisChart = new Chart ();
				thisChart.topLeftNotes = new List<noteSpawn> ();
				thisChart.bottomNotes = new List<noteSpawn> ();
				thisChart.topRightNotes = new List<noteSpawn> ();
				int currentBpm = 0; //This will be the index of the BPM currently being used to determine note times.

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
				float beatCounter = 0;
				List<string> notesInMeasure = new List<string> ();
				while (lineOfChart[0] != ';') {
					if (lineOfChart [0] == ',') {
						//End of Measure, calculate note times&types and add to chart
						//Debug.Log ($"Measure has {notesInMeasure.Count} subdivisions.");
						//Debug.Log (beatCounter);
						//Before determining any note times, check if BPM has changed.
						if (currentBpm + 1 < result.bpms.Count) {
							if (beatCounter >= result.bpms [currentBpm + 1].beat) {
								currentBpm++;
							}
						}
							
						//Debug.Log ($"Step is {step}.");
						for (int i = 0; i < notesInMeasure.Count; i++) {
							/*
							   Todo: the code that creates noteSpawn structs and pushes them
							   to the list of notes for the chart. Adding a note will involve
							   calculating the amount of time past the current time (using BPM
							   and amount of notes in measure). Make sure to advance currentTime
							   while adding notes.
							*/
							//advance beat
							beatCounter += (float)(4/(float)notesInMeasure.Count);
							//Debug.Log ($"Current time is {currentTime}");

							string lineNote = "";
							if (notesInMeasure [i] [0] == '1') {
								//add a top left note at the current time
								lineNote += "topleft ";
								thisChart.topLeftNotes.Add (new noteSpawn (beatCounter, "tap"));
							}
							if (notesInMeasure [i] [1] == '1') {
								//add a bottom note at the current time
								lineNote += "bottom ";
								thisChart.bottomNotes.Add (new noteSpawn (beatCounter, "tap"));
							}
							if (notesInMeasure [i] [2] == '1') {
								//add a top right note at the current time
								lineNote += "topright";
								thisChart.topRightNotes.Add (new noteSpawn (beatCounter, "tap"));
							}

							//Debug.Log ("this note is : " + lineNote);
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

				//Debug.Log ($"{thisChart.difficulty}, level {thisChart.rating}");
				result.charts.Add (thisChart);
			}
		}

		return result;
	}
}
