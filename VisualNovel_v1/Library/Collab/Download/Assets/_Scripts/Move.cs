using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour {

	Animation anim;

	void Start () {
		anim = gameObject.GetComponent<Animation> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonDown (0)) {
			anim.Play ();
		}
	}
}
