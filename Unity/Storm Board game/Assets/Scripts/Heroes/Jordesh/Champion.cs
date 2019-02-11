using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Champion : Piece {
	//==STATS==//
	private static int CHARHP = 600;
	private static int CHARDAM = 50;
	private static int CHARRANGE = 1;
	private static int CHARAP = 4;
	private static int CHARAT = 0;
	private static int CHARNUM = 0;
	private static int active1Damage = 100;
	private static int active2Damage = 100;
	private static int active1Range = 1;
	private static int active2Range = 1;
	private static int active1Cooldown = 1;
	private static int active2Cooldown = 1;
	private static int airBonus = 1;
	private static int fireBonus = 25;
	private static int earthBonus = 15;

	private int elementalBonus;

	public override void onStart () {
		maxHealth = CHARHP;
		damage = CHARDAM;
		range = CHARRANGE;
		MaxAP = CHARAP;
		attackType = CHARAT;
		charNumber = CHARNUM;
	}

	public virtual void startTurn () {
		if (statusEffect == 0)
			AP = MaxAP;
		activeState = 0;
		canAttack = true;
		if (isDead == true) {
			respawnTime--;
			Debug.Log (charName + team + " will respawn in " + respawnTime + " turns");
			if (respawnTime == 0) {
				Respawn ();
			}
		}
		if (a1Cooldown > 0)
			a1Cooldown--;
		if (a2Cooldown > 0)
			a2Cooldown--;
		elementalBonus = (int)(Random.Range (0.0f, 4.0f));
	}
}
