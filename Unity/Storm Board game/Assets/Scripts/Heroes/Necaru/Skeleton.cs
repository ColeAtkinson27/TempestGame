using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Skeleton : MonoBehaviour {

	private int x;
	private int y;
	private int team;
	private Controller grid;
	private Necromancer master;
	private static int damage = 32;
	private int timer = 3;

	public void setup (Controller g, int xPos, int yPos, int t, Necromancer m) {
		grid = g;
		x = xPos;
		y = yPos;
		team = t;
		master = m;
	}

	// Update is called once per frame
	void Update () {
		for (int checkX = -1; checkX <= 1; checkX++) {
			for (int checkY = -1; checkY <= 1; checkY++) {
				if (x + checkX >= 0 && x + checkX < 18) {
					if (y + checkY >= 0 && y + checkY < 9) {
						if (grid.pieces [x + checkX, y + checkY] != null && grid.pieces [x + checkX, y + checkY].team != team) {
							Piece walker = grid.pieces [x + checkX, y + checkY];
							walker.takeDamage (damage, master, 3);
							Debug.Log (walker.charName + " walked too close to a skeleton");
							wearOut ();
						}
					}
				}
			}
		}
	}

	public void deathTimer () {
		timer--;
		if (timer == 0)
			wearOut ();
	}

	public void wearOut() {
		Destroy (gameObject);
	}
}
