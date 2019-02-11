using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManaMine : MonoBehaviour {

	private int x;
	private int y;
	private int team;
	private Controller grid;
	private ArcaneSmith master;
	private static int damage = 32;

	public void setup (Controller g, int xPos, int yPos, int t, ArcaneSmith m) {
		grid = g;
		x = xPos;
		y = yPos;
		team = t;
		master = m;
	}

	// Update is called once per frame
	void Update () {
		if (grid.pieces[x, y] != null && grid.pieces[x, y].team != team) {
			Piece walker = grid.pieces [x, y];
			walker.takeDamage (damage, master, 3);
			Debug.Log (walker.charName + " stepped on a Mana Mine");
			fallApart();
		}
	}

	public void fallApart() {
		Destroy (gameObject);
	}
}
