using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Necromancer : Piece {
	//==STATS==//
	private static int CHARHP = 300;
	private static int CHARDAM = 60;
	private static int CHARRANGE = 3;
	private static int CHARAP = 3;
	private static int CHARAT = 1;
	private static int CHARNUM = 0;
	private static int active1Damage = 60;
	private static int active2Damage = 80;
	private static int active1Range = 3;
	private static int active2Range = 3;
	private static int active1Cooldown = 3;
	private static int active2Cooldown = 2;

	private int targetXPos;
	private int targetYPos;
	public Skeleton bones;
	private Skeleton[] undead = new Skeleton[10];

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
		for (int i = 0; i < 10; i++) {
			if (undead [i])
				undead [i].deathTimer ();
		}
	}

	public override void attackEnemy (Piece target) {
		canAttack = false;
		int damageDealt = Random.Range((damage / 2), damage);
		int defenderType = target.getAttackType();
		targetXPos = target.getX ();
		targetYPos = target.getY ();

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

	public override void onKill() {
		kills++;
		raiseUndead (0);
	}

	public override void onDeath () {
		raiseUndead (1);
		if (team == 1) {
			grid.redScore = grid.redScore + 1;
		} else if (team == 2) {
			grid.blueScore = grid.blueScore + 1;
		}
		grid.setHeroPositions (x, y, null);
		grid.setBoardTile (x, y, 0);
		grid.setHeroPositions (spawnx, spawny, this);
		isDead = true;
		respawnTime = 3;
		Circle newPos = grid.checkBoardTiles (spawnx, spawny);
		x = spawnx;
		y = spawny;
		transform.position = Vector3.MoveTowards(transform.position, new Vector3 (newPos.transform.position.x, 55.5f, newPos.transform.position.z), 10000);
		Renderer rend = model.GetComponent <Renderer> ();
		rend.enabled = false;
		deaths++;
	}

	public override void activeAbility1 (Piece target) {
		useAP ();
		active1OnCooldown (active1Cooldown);
		float damageDealt = Random.Range (active1Damage / 1.5f, active1Damage);
		Debug.Log (target.charName + target.team + " is hit with 'Rising Tomb' for " + (int)damageDealt + " damage");
		target.setStatusEffect (2);
		targetXPos = target.getX ();
		targetYPos = target.getY ();
		target.takeDamage ((int)damageDealt, this, 2);
		setAttackState (0);
	}

	public override void activeAbility2 (Piece target) {
		Debug.Log (target.charName + team + " and nearby targets are hit with 'Skull Bomb'");
		useAP ();
		int targetX = target.getX ();
		int targetY = target.getY ();
		Piece hit;
		for (int x = -1; x < 2; x++) {
			for (int y = -1; y < 2; y++) {
				if (targetX + x <= 17 && targetX + x >= 1) {
					if (targetY + y <= 8 && targetY + y >= 0) {
						if (grid.checkHeroPositions (targetX + x, targetY + y)) {
							Piece t = grid.checkHeroPositions (targetX + x, targetY + y);
							if (t.team == target.team && t.checkIsDead () == false) {
								hit = grid.checkHeroPositions (targetX + x, targetY + y);
								float damageDealt = Random.Range (active2Damage / 1.5f, active2Damage);
								targetXPos = hit.getX ();
								targetYPos = hit.getY ();
								hit.takeDamage ((int)damageDealt, this, 1);
								Debug.Log (hit.charName + target.team + " was hit by 'Skull Bomb' for " + (int)damageDealt + " damage");
							}
						}
					}
				}
			}
		}
		active2OnCooldown (active2Cooldown);
		setAttackState (0);
	}

	public override void active1Search (int target) {
		grid.radialTargetting (target, active1Range);
	}

	public override void active2Search (int target) {
		grid.radialTargetting (target, active2Range);
	}

	private void raiseUndead(int dead) {
		if (dead == 0) {
			Skeleton s = Instantiate (bones) as Skeleton;
			Circle pos = grid.checkBoardTiles (targetXPos, targetYPos);
			s.transform.position = new Vector3(pos.transform.position.x, 40.0f, pos.transform.position.z);
			s.setup(grid, targetXPos, targetYPos, team, this);
			for (int i = 0; i < 10; i++) {
				if (undead[i] == null)
					undead [i] = s;
			}
		} else {
			Skeleton s = Instantiate (bones) as Skeleton;
			s.transform.position = new Vector3(this.transform.position.x, 40.0f, this.transform.position.z);
			s.setup(grid, targetXPos, targetXPos, team, this);
			for (int i = 0; i < 10; i++) {
				if (undead[i] == null)
					undead [i] = s;
			}
		}
	}
}
