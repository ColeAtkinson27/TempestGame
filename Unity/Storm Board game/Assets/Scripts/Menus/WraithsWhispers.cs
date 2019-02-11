using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class WraithsWhispers : MonoBehaviour {

	public Text Display;
	public string [] Whispers;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void displayMessage () {
		int line = (int) Random.Range (0.0f, 9.9f);
		Display.text = Whispers [line];
	}
}
