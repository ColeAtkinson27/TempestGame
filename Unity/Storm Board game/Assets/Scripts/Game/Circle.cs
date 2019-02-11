using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Circle : MonoBehaviour {

	public Material green, target, noTarget, inactive, Player1Hero, Player2Hero, selected;
	private int state;
	public int x, y;

	// Use this for initialization
	void Start () {
	}

	public int getState () {
		return state;
	}

	public void setState (int newState) {
		state = newState;
		//0 = Inactive
		//1 = Player 1
		//2 = Player 2
		//3 = Selected
		//4 = Green
		//5 = Target
		//6 = No Target
	}

	// Update is called once per frame
	void Update () {
		Renderer rend = GetComponent <Renderer> ();

		if (state == 0) {
			rend.material = inactive;
		} else if (state == 1) {
			rend.material = Player1Hero;
		} else if (state == 2) {
			rend.material = Player2Hero;
		} else if (state == 3) {
			rend.material = selected;
		} else if (state == 4) {
			rend.material = green;
		} else if (state == 5) {
			rend.material = target;
		} else if (state == 6) {
			rend.material = noTarget;
		} else {
			rend.material = inactive;
		}
	}
}
