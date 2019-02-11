using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Archmage : Piece {
	//==STATS==//
	private static int CHARHP = 300;
	private static int CHARDAM = 60;
	private static int CHARRANGE = 3;
	private static int CHARAP = 3;
	private static int CHARAT = 1;
	private static int CHARNUM = 0;
	private static int active1Damage = 150;
	private static int active2Damage = 150;
	private static int active1Range = 3;
	private static int active2Range = 2;
	private static int active1Cooldown = 3;
	private static int active2Cooldown = 3;

	public override void onStart () {
		maxHealth = CHARHP;
		damage = CHARDAM;
		range = CHARRANGE;
		MaxAP = CHARAP;
		attackType = CHARAT;
		charNumber = CHARNUM;
	}

	public override void activeAbility1 (Piece target) {
		useAP ();
		active1OnCooldown (active1Cooldown);
		float damageDealt = Random.Range (active1Damage / 1.5f, active1Damage);
		Debug.Log (target.charName + target.team + " is hit with 'Fire Bolt' for " + (int)damageDealt + " damage");
		target.takeDamage ((int)damageDealt, this, 1);
		base.setAttackState (0);
	}

	public override void activeAbility2 (Piece target) {
		useAP ();
		active2OnCooldown (active2Cooldown);
		int northsouth = 0;
		int eastwest = 0;
		bool firstTargetHit = false;

		if (target.getX () > getX ()) {
			eastwest = 1;
		} else if (target.getX () < getX ()) {
			eastwest = -1;
		} else if (target.getX () == getX ()) {
			eastwest = 0;
		}

		if (target.getY () > getY ()) {
			northsouth = 1;
		} else if (target.getY () < getY ()) {
			northsouth = -1;
		} else if (target.getY () == getY ()) {
			northsouth = 0;
		}

		for (int x = 1; x < active2Range + 1; x++) {
			int xTile = getX () + (x * eastwest);
			int yTile = getY () + (x * northsouth);
			if (xTile >= 0 && xTile < 18 && yTile >= 0 && yTile < 9) {
				if (grid.checkBoardTerrain (xTile, yTile) != 2) {
					if (grid.pieces [xTile, yTile] != null && grid.pieces [xTile, yTile].team == target.team && grid.pieces [xTile, yTile].checkIsDead () == false) {
						float damageDealt = Random.Range (active2Damage / 1.5f, active2Damage);
						Debug.Log (grid.pieces [xTile, yTile].charName + target.team + " is hit with 'Ice Blast' for " + (int)damageDealt + " damage");
						if (firstTargetHit == false) {
							grid.pieces [xTile, yTile].setStatusEffect (1);
							firstTargetHit = true;
						}
						grid.pieces [xTile, yTile].takeDamage ((int)damageDealt, this, 1);
					}
				}
			}
		}
		setAttackState (0);
	}

	public override void active1Search (int target) {
		grid.radialTargetting (target, active1Range);
	}

	public override void active2Search (int target) {
		grid.directionalTargetting (target, active2Range);
	}

	public override void onKill() {
		Debug.Log (charName + team + "'s mana overflows and resets cooldowns");
		addKill();
		active1OnCooldown (0);
		active2OnCooldown (0);
	}
}
