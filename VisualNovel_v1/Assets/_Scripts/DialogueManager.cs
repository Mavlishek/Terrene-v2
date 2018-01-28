using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour {

	DialogueParser parser;

	public string dialogue, characterName;
	public int lineNum;
	int pose;
	int mood;
	string[] options;
	public bool playerTalking;
	List<Button> buttons = new List<Button> ();

	int currentMood1;
	int currentMood2;

	public Text dialogueBox;
	public Text nameBox;
	public GameObject choiceBox;

	// Use this for initialization
	void Start () {
		dialogue = "";
		characterName = "";
		pose = 0;
		mood = 0;
		playerTalking = false;
		parser = GameObject.Find("Dialogue Parser").GetComponent<DialogueParser>();
		lineNum = 0;
		currentMood1 = 0;
		currentMood2 = 0;

		hideDeath ();
		hideSuccess ();

	}

	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (0) && playerTalking == false) {
	
			if (parser.GetName (lineNum) == "SceneChange") {
				//Debug.Log (parser.GetContent (lineNum));
				parser.LoadDialogue ("Assets/Data/" + parser.GetContent (lineNum));
				lineNum = 0;
				ShowDialogue ();
				lineNum++;
				sceneImage ();
			} else if (parser.GetName (lineNum) == "Decision") {

				string[] content = parser.GetContent (lineNum).Split ('-');

				string variable = content [0];
				string comparison = content [1];
				int valueThatWeAreComparingAgainst = int.Parse(content [2]);
				string sceneIfTrue = content [3];
				string sceneIfFalse = content [4];

				int comparisonValue;
				bool comparisonTrue;

				if (variable == "currentMood1") {
					comparisonValue = currentMood1;
				} else {
					comparisonValue = currentMood2;
				}
				if (comparison == "lessThan") {
					if (valueThatWeAreComparingAgainst < comparisonValue) {
						comparisonTrue = true;
					} else {
						comparisonTrue = false;
					}
				} else {
					if (valueThatWeAreComparingAgainst > comparisonValue) {
						comparisonTrue = true;
					} else {
						comparisonTrue = false;
					}
				}
				if (comparisonTrue) {
					parser.LoadDialogue ("Assets/Data/" + sceneIfTrue);
				} else {
					parser.LoadDialogue ("Assets/Data/" + sceneIfFalse);
				}
				lineNum = 0;
				ShowDialogue ();
				lineNum++;
				sceneImage ();
			}
			else {
				ShowDialogue ();
				lineNum++;
			}
		}
		UpdateUI ();
		showDeath ();
		showSuccess ();
		if (parser.GetContent (lineNum) == "Wall.txt") {
			sceneImage2 ();
		}
	}

	public void ShowDialogue() {
		ResetImages ();
		ParseLine ();
	}

	void UpdateUI() {
		if (!playerTalking) {
			ClearButtons();
			ButtBackFalse ();
		}
		dialogueBox.text = dialogue;
		nameBox.text = characterName;
	}

	void ClearButtons() {
		for (int i = 0; i < buttons.Count; i++) {
			print ("Clearing buttons");
			Button b = buttons[i];
			buttons.Remove(b);
			Destroy(b.gameObject);
		}
	}

	void ParseLine() {
		if (parser.GetName (lineNum) != "Player") {
			playerTalking = false;
			characterName = parser.GetName (lineNum);
			dialogue = parser.GetContent (lineNum);
			pose = parser.GetPose (lineNum);
			mood = parser.GetMood (lineNum);
			DisplayImages ();
			SetMood ();
			showArrow ();
		}

		 else {
			playerTalking = true;
			characterName = "";
			dialogue = "";
			pose = 0;
			mood = 0;
			options = parser.GetOptions(lineNum);
			CreateButtons();
			ButtBackTrue ();
			hideArrow ();
		}
	}

	void CreateButtons() {
		for (int i = 0; i < options.Length; i++) {
			GameObject button = (GameObject)Instantiate (choiceBox);
			Button b = button.GetComponent<Button> ();
			ChoiceButton cb = button.GetComponent<ChoiceButton> ();
			cb.SetText (options [i].Split (':') [0]);
			cb.option = options [i].Split (':') [1];
			cb.box = this;
			b.transform.SetParent (this.transform);
			b.transform.localPosition = new Vector3 (0, -25 + (i * 110));
			b.transform.localScale = new Vector3 (2, 2, 2);
			buttons.Add (b);
		}
	}

	void ResetImages() {
		if (characterName != "") {
			GameObject character = GameObject.Find (characterName);
			Image currSprite = character.GetComponent<Image>();
			currSprite.enabled = false;

			NameItemsFalse ();
		}
	}

	void DisplayImages() {
		if (characterName != "") {
			GameObject character = GameObject.Find(characterName);

			//SetSpritePositions(character);
			//SetSpriteSize (character);

			Image currSprite = character.GetComponent<Image>();
			currSprite.enabled = true;
			currSprite.sprite = character.GetComponent<Character>().characterPoses[pose];

			NameItemsTrue ();
		}
	}

	void SetMood() {
		if (mood == 0) {
			//neutral
			currentMood1 += 0;
		} else if (mood == 1) {
			//happy
			currentMood1 += 2;
		} else if (mood == 2) {
			//sad
			currentMood1 -= 2;
		} else if (mood == 3) {
			//angry
			currentMood2 += 4;
		} else if (mood == 4) {
			//fearful
			currentMood2 -= 4;
		}
		print("mood 1 =" + currentMood1);
		print ("mood 2 =" + currentMood2);
	}

	void SetSpriteSize(GameObject spriteObj) {
		//GetComponent<RectTransform> ().sizeDelta = new Vector2 (height, width);
		//spriteObj.transform.localScale = new Vector3 (100,100);
		//spriteObj.transform.localScale = new Vector3 (spriteObj.transform.localScale.x, spriteObj.transform.localScale.y, 0);
	}
		
	void ButtBackTrue() {
		GameObject bb = GameObject.Find ("ButtonPanel");
		Image currbutton = bb.GetComponent<Image> ();
		Text textbutton = bb.GetComponentInChildren<Text> ();
		currbutton.enabled = true;
		textbutton.enabled = true;
	}

	void ButtBackFalse() {
		GameObject bb = GameObject.Find ("ButtonPanel");
		Image currbutton = bb.GetComponent<Image> ();
		Text textbutton = bb.GetComponentInChildren<Text> ();
		currbutton.enabled = false;
		textbutton.enabled = false;
	}

	void NameItemsTrue() {
		GameObject ni = GameObject.Find ("NameItems");
		Image currname = ni.GetComponentInChildren<Image> ();
		Text currtext = ni.GetComponentInChildren<Text> ();
		currname.enabled = true;
		currtext.enabled = true;
		GameObject nb = GameObject.Find ("NameBorder");
		Image namebord = nb.GetComponent<Image> ();
		namebord.enabled = true;
	}

	void NameItemsFalse() {
		GameObject ni = GameObject.Find ("NameItems");
		Image currname = ni.GetComponentInChildren<Image> ();
		currname.enabled = false;
		GameObject nb = GameObject.Find ("NameBorder");
		Image namebord = nb.GetComponent<Image> ();
		namebord.enabled = false;
	}

	void showArrow() {
		GameObject arr = GameObject.Find ("Arrow");
		Image arrow = arr.GetComponent<Image> ();
		arrow.enabled = true;
	}

	void hideArrow() {
		GameObject arr = GameObject.Find ("Arrow");
		Image arrow = arr.GetComponent<Image> ();
		arrow.enabled = false;
	}

	void showDeath() {
		if (characterName == "Death") {
			GameObject d = GameObject.Find ("Death");
			Text currd = d.GetComponentInChildren<Text> ();
			currd.enabled = true;

			GameObject b = GameObject.Find ("QuitButton");
			Text currb = b.GetComponentInChildren<Text> ();
			currb.enabled = true;
			Image currt = b.GetComponent<Image> ();
			currt.enabled = true;
			GameObject f = GameObject.Find ("BackButton");
			Text currf = f.GetComponentInChildren<Text> ();
			currf.enabled = true;
			Image curry = f.GetComponent<Image> ();
			curry.enabled = true;
		}
	}

	void hideDeath() {
			GameObject d = GameObject.Find ("Death");
			Text currd = d.GetComponentInChildren<Text> ();
			currd.enabled = false;
		GameObject b = GameObject.Find ("QuitButton");
		Text currb = b.GetComponentInChildren<Text> ();
		currb.enabled = false;
		Image currt = b.GetComponent<Image> ();
		currt.enabled = false;
		GameObject f = GameObject.Find ("BackButton");
		Text currf = f.GetComponentInChildren<Text> ();
		currf.enabled = false;
		Image curry = f.GetComponent<Image> ();
		curry.enabled = false;
	}

	void showSuccess() {
		if (characterName == "Success") {
			GameObject d = GameObject.Find ("Success");
			Text currd = d.GetComponentInChildren<Text> ();
			currd.enabled = true;

			GameObject b = GameObject.Find ("QuitButton1");
			Text currb = b.GetComponentInChildren<Text> ();
			currb.enabled = true;
			Image currt = b.GetComponent<Image> ();
			currt.enabled = true;
			GameObject f = GameObject.Find ("BackButton1");
			Text currf = f.GetComponentInChildren<Text> ();
			currf.enabled = true;
			Image curry = f.GetComponent<Image> ();
			curry.enabled = true;
		}
	}

	void hideSuccess() {
		GameObject d = GameObject.Find ("Success");
		Text currd = d.GetComponentInChildren<Text> ();
		currd.enabled = false;
		GameObject b = GameObject.Find ("QuitButton1");
		Text currb = b.GetComponentInChildren<Text> ();
		currb.enabled = false;
		Image currt = b.GetComponent<Image> ();
		currt.enabled = false;
		GameObject f = GameObject.Find ("BackButton1");
		Text currf = f.GetComponentInChildren<Text> ();
		currf.enabled = false;
		Image curry = f.GetComponent<Image> ();
		curry.enabled = false;
	}

	void sceneImage() {
		GameObject im = GameObject.Find ("Image");
		Image currim = im.GetComponent<Image> ();
		currim.enabled = false;
	}

	void sceneImage2() {
		GameObject im = GameObject.Find ("StreetImage1");
		Image currim = im.GetComponent<Image> ();
		currim.enabled = false;
	}
}