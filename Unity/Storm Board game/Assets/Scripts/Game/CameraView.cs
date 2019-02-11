using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraView : MonoBehaviour {
	
	public GameObject angled, overhead;
	public Camera Current;
	private int currentcamera;

	// Use this for initialization
	void Start () {
		Current = angled.GetComponent <Camera> ();
		angled.SetActive (true);
		overhead.SetActive (false);
		currentcamera = 0;
	}
	
	// Update is called once per frame
	public void SwitchCamera () {
		//angled.enabled = !angled.enabled;
		//overhead.enabled = !overhead.enabled;
		if (currentcamera == 0) {
			angled.SetActive (false);
			overhead.SetActive (true);
			currentcamera = 1;
			Current = overhead.GetComponent <Camera> ();
		} else {
			overhead.SetActive (false);
			angled.SetActive (true);
			currentcamera = 0;
			Current = angled.GetComponent <Camera> ();
		}
	}

	public Camera currentCamera () {
		return Current;
	}
}
