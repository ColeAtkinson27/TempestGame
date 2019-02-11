using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Style : MonoBehaviour {
	private int currentStyle;
	private Image display;

	private Color32 [] styles = new Color32 [7];

	// Use this for initialization
	void Start () {
		display = this.GetComponent <Image> ();

		styles [0] = new Color32 (255, 212, 125, 255);
		styles [1] = new Color32 (0, 125, 255, 255);
		styles [2] = new Color32 (212, 0, 125, 255);
		styles [3] = new Color32 (255, 212, 0, 255);
		styles [4] = new Color32 (212, 0, 0, 255);
		styles [5] = new Color32 (0, 212, 0, 255);
		styles [6] = new Color32 (255, 125, 0, 255);
	}
	
	// Update is called once per frame
	void Update () {
		int newStyle = PlayerPrefs.GetInt ("MenuStyle");
		if (display)
			display.color = styles [newStyle];
	}
}
