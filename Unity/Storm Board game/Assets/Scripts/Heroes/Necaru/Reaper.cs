using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Reaper : Piece {
	//==STATS==//
	private static int CHARHP = 600;
	private static int CHARDAM = 60;
	private static int CHARRANGE = 1;
	private static int CHARAP = 3;
	private static int CHARAT = 0;
	private static int CHARNUM = 0;
	private static int active1Damage = 80;
	private static int active2Damage = 30;
	private static int active1Range = 1;
	private static int active2Range = 3;
	private static int active1Cooldown = 2;
	private static int active2Cooldown = 3;
	private static double attackBonus = 0.10;
	private static double abilityBonus = 0.06;


	public override void onStart () {
		maxHealth = CHARHP;
		damage = CHARDAM;
		range = CHARRANGE;
		MaxAP = CHARAP;
		attackType = CHARAT;
		charNumber = CHARNUM;
	}

	public override void attackEnemy (Piece target) {
		canAttack = false;
		int damageDealt = Random.Range((damage / 2), damage);
		damageDealt += (int) (target.getMaxHealth() * attackBonus);
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
		AP--;
	}

	public override void activeAbility1 (Piece target) {
		useAP ();
		active1OnCooldown (active1Cooldown);
		float damageDealt = Random.Range (active1Damage / 1.5f, active1Damage);
		damageDealt += (int) (target.getMaxHealth() * abilityBonus);
		Debug.Log (target.charName + target.team + " is hit with 'Shred Soul' for " + (int)damageDealt + " damage");
		target.takeDamage ((int)damageDealt, this, 2);
		base.setAttackState (0);
	}

	public override void activeAbility2 (Piece target) {
		useAP ();
		active2OnCooldown (active2Cooldown);
		int tx = target.getX () - x;
		int ty = target.getY () - y;
		if (tx < 0) {
			tx = -1;
		} else if (tx > 0)
			tx = 1;
		if (ty < 0)
			ty = -1;
		else if (ty > 0)
			ty = 1;

		if (grid.checkBoardTerrain (x + tx, y + ty) == 0 &&
			grid.pieces [x + (1 * tx), y + (1 * ty)] == null || 
			grid.pieces [x + (1 * tx), y + (1 * ty)] == target) {
			Circle dragPos = grid.boardTiles [x + tx, y + ty];
			target.moveCharacter (dragPos);
			Debug.Log (charName + team + " drags " + target.charName + target.team + " to him.");
		} else if (grid.checkBoardTerrain (x + (2 * tx), y + (2 * ty)) == 0 &&
			grid.pieces [x + (2 * tx), y + (2 * ty)] == null|| 
			grid.pieces [x + (1 * tx), y + (1 * ty)] == target) {
			Circle dragPos = grid.boardTiles [x + (2 * tx), y + (2 * ty)];
			target.moveCharacter (dragPos);
			Debug.Log (charName + team + " drags " + target.charName + target.team + " to him.");
		} else {
			Debug.Log (charName + team + " couldn't drag " + target.charName + target.team + " to him.");
		}
		float damageDealt = Random.Range (active2Damage / 1.5f, active2Damage);
		damageDealt += (int) (target.getMaxHealth() * abilityBonus);
		Debug.Log (target.charName + target.team + " is hit with 'Spectral Grasp' for " + (int)damageDealt + " damage");
		target.takeDamage ((int)damageDealt, this, 2);
		base.setAttackState (0);
	}

	public override void active1Search (int target) {
		grid.radialTargetting (target, active1Range);
	}

	public override void active2Search (int target) {
		grid.directionalTargetting (target, active2Range);
	}
}
