using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireLord : Piece {
	//==STATS==//
	private static int CHARHP = 1000;
	private static int CHARDAM = 40;
	private static int CHARRANGE = 1;
	private static int CHARAP = 4;
	private static int CHARAT = 0;
	private static int CHARNUM = 0;
	private static int active1Damage = 60;
	private static int active2Damage = 45;
	private static int active1Range = 1;
	private static int active2Range = 2;
	private static int active1Cooldown = 3;
	private static int active2Cooldown = 2;
	private static int active2HealAmount = 50;
	public FireStep fire;
	private FireStep[] trail = new FireStep[8];

	public override void onStart () {
		maxHealth = CHARHP;
		damage = CHARDAM;
		range = CHARRANGE;
		MaxAP = CHARAP;
		attackType = CHARAT;
		charNumber = CHARNUM;
	}

	public override void startTurn () {
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
		for (int i = 0; i < trail.Length; i++) {
			if (trail [i])
				trail [i].fizzleOut ();
		}
	}

	public override void moveCharacter (Circle newPos) {
		int xDirection = newPos.x - x;
		int yDirection = newPos.y - y;
		float rotation = 0f;
		if (xDirection == 1)
			rotation = -90;
		else if (xDirection == -1)
			rotation = 90;
		else if (yDirection == 1)
			rotation = 180;

		FireStep fStep = Instantiate (fire) as FireStep;
		fStep.transform.position = new Vector3(this.transform.position.x, 34.0f, this.transform.position.z);
		fStep.transform.Rotate (new Vector3(0f, rotation, 0f));
		fStep.setup(grid, x, y, team, this);
		trail [AP] = fStep;

		transform.position = Vector3.MoveTowards(transform.position, new Vector3 (newPos.transform.position.x, 50.25f, newPos.transform.position.z), 100);
		grid.setHeroPositions (x, y, null);
		x = newPos.x;
		y = newPos.y;
		grid.setHeroPositions (x, y, this);
	}

	public override void activeAbility1 (Piece target) {
		useAP ();
		active1OnCooldown (active1Cooldown);
		float damageDealt = Random.Range (active1Damage / 1.5f, active1Damage);
		Debug.Log (target.charName + target.team + " is hit with 'Infernal Smite' for " + (int)damageDealt + " damage");
		target.takeDamage ((int)damageDealt, this, 2);
		base.setAttackState (0);
	}

	public override void activeAbility2 (Piece target) {
		useAP ();
		active2OnCooldown (active2Cooldown);
		int northsouth = 0;
		int eastwest = 0;

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
						Debug.Log (grid.pieces [xTile, yTile].charName + target.team + " is hit with 'Incendiary Blast' for " + (int)damageDealt + " damage");
						grid.pieces [xTile, yTile].takeDamage ((int)damageDealt, this, 2);
						healDamage (active2HealAmount);
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
}
