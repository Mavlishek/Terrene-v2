using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Character : MonoBehaviour {

	public Sprite[] characterPoses = null;

	// Use this for initialization
	void Start () {

		GetComponent<Image> ().enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
