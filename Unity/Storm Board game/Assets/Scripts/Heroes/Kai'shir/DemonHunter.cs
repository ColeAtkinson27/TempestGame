using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonHunter : Piece {
	//==STATS==//
	private static int CHARHP = 200;
	private static int CHARDAM = 150;
	private static int CHARRANGE = 4;
	private static int CHARAP = 3;
	private static int CHARAT = 2;
	private static int CHARNUM = 0;
	private static int active1Damage = 300;
	private static int active1Range = 4;
	private static int active1Cooldown = 1;
	private static int active2Cooldown = 1;

	public override void onStart () {
		maxHealth = CHARHP;
		damage = CHARDAM;
		range = CHARRANGE;
		MaxAP = CHARAP;
		attackType = CHARAT;
		charNumber = CHARNUM;
	}

	public virtual void attackEnemy (Piece target) {
		canAttack = false;
		int damageDealt = Random.Range((damage / 2), damage);
		int defenderType = target.getAttackType();

		if (attackType == 1 && defenderType == 1) {
			target.defend (this);
		} else {
			target.takeDamage (damageDealt, this, 0);
		}

		if (attackType == 0) {
			if (defenderType == 0) {
				target.retaliate (this);
			}
		} else if (attackType == 1) {
			if (defenderType == 2) {
				target.retaliate (this);
			}
		} else if (attackType == 2) {
			if (defenderType == 1) {
				target.retaliate (this);
			} else if (defenderType == 2) {
				target.retaliate (this);
			}
		}
	}

	public override void active1Search (int target) {
		grid.radialTargetting (target, active1Range);
	}

	public override void active2Search (int target) {
		for (int i = -1; i <= 1; i++) {
			if (grid.checkBoardTerrain(x + i, y) == 1) {
				if (grid.checkBoardTerrain (x + (i * 2), y) == 1) {
					grid.setBoardTile (x + (i * 2), y, 5);
				}
			}
			i++;
		}
		for (int i = -1; i <= 1; i++) {
			if (grid.checkBoardTerrain(x, y + i) == 1) {
				if (grid.checkBoardTerrain (x, y + (i * 2)) == 1) {
					grid.setBoardTile (x, y + (i * 2), 5);
				}
			}
			i++;
		}
	}
}
