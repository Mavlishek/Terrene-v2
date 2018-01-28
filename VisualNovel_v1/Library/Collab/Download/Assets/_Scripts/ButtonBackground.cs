using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonBackground : MonoBehaviour {

	// Use this for initialization
	void Start () {
		GetComponent<Image> ().enabled = false;	
		GameObject ni = GameObject.Find ("NameBorder");
		Image currname = ni.GetComponent<Image> ();
		currname.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	}
}