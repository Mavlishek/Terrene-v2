using UnityEngine;
using System.Collections;
using UnityEditor;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;
using System.Collections.Generic;

public class DialogueParser : MonoBehaviour {

	struct DialogueLine {
		public string name;
		public string content;
		public int pose;
		public int mood;
		public string[] options;

		public DialogueLine(string Name, string Content, int Pose, int Mood) {
			name = Name;
			content = Content;
			pose = Pose;
			mood = Mood;
			options = new string[0];
		}
	}


	List<DialogueLine> lines;

	// Use this for initialization
	void Start () {
		string file = "Assets/Data/Dialogue";
		string sceneNum = EditorApplication.currentScene;
		sceneNum = Regex.Replace (sceneNum, "[^0-9]", "");
		file += sceneNum;
		file += ".txt";


		LoadDialogue (file);
	}

	// Update is called once per frame
	void Update () {

	}

	public void LoadDialogue(string filename) {
		lines = new List<DialogueLine>();

		string line;
		StreamReader r = new StreamReader (filename);

		using (r) {
			do {
				line = r.ReadLine();
				//Debug.Log("!!"+line);
				if (line != null) {
					string[] lineData = line.Split(';');
					if (lineData[0] == "Player") {
						DialogueLine lineEntry = new DialogueLine(lineData[0], "", 0, 0);
						lineEntry.options = new string[lineData.Length-1];
						for (int i = 1; i < lineData.Length; i++) {
							lineEntry.options[i-1] = lineData[i];
						}
						lines.Add(lineEntry);
					} else {
						DialogueLine lineEntry = new DialogueLine(lineData[0], lineData[1], int.Parse(lineData[2]), int.Parse(lineData[3]));
						lines.Add(lineEntry);
					}
				}
			}
			while (line != null);
			r.Close();
		}
	}

	public int GetMood(int lineNumber) {
		if (lineNumber < lines.Count) {
			return lines[lineNumber].mood;
		}
		return 0;
	}

	public string GetName(int lineNumber) {
		if (lineNumber < lines.Count) {
			return lines[lineNumber].name;
		}
		return "";
	}

	public string GetContent(int lineNumber) {
		if (lineNumber < lines.Count) {
			return lines[lineNumber].content;
		}
		return "";
	}

	public int GetPose(int lineNumber) {
		if (lineNumber < lines.Count) {
			return lines[lineNumber].pose;
		}
		return 0;
	}

	public string[] GetOptions(int lineNumber) {
		if (lineNumber < lines.Count) {
			return lines[lineNumber].options;
		}
		return new string[0];
	}
}