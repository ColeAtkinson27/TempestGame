using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AncientOne : Piece {
	//==STATS==//
	private static int CHARBASEHP = 600;
	private static int CHARMINIMUMHP = 250;
	private static int HPGAIN = 75;
	private static int HPLOSS = 100;
	private static int CHARDAM = 40;
	private static int CHARRANGE = 1;
	private static int CHARAP = 4;
	private static int CHARAT = 0;
	private static int CHARNUM = 0;
	private static float active1EnemyDamage = 60;
	private static float active1AllyDamage = 30;
	private static float active1Heal = 50;
	private static int active2Damage = 100;
	private static int active1Range = 1;
	private static int active2Range = 2;
	private static int active1Cooldown = 3;
	private static int active2Cooldown = 3;

	private double score = 0;

	public override void onStart () {
		maxHealth = CHARBASEHP;
		damage = CHARDAM;
		range = CHARRANGE;
		MaxAP = CHARAP;
		attackType = CHARAT;
		charNumber = CHARNUM;
	}

	void Update () {
		if (team == 1) {
			if (grid.blueScore != score) {
				Debug.Log (charName + team + "'s health increased");
				maxHealth += HPGAIN;
				HP += HPGAIN;
				score = grid.blueScore;
			}
		} else if (team == 2) {
			if (grid.redScore != score) {
				Debug.Log (charName + team + "'s health increased");
				maxHealth += HPGAIN;
				HP += HPGAIN;
				score = grid.redScore;
			}
		}
	}

	public override void onKill() {
		kills++;
	}

	public override void onDeath () {
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
		maxHealth -= HPLOSS;
		Debug.Log (charName + team + "'s health decreased");
		if (maxHealth < CHARMINIMUMHP)
			maxHealth = CHARMINIMUMHP;
	}

	public override void activeAbility1 (Piece target) {
		AP--;
		active1OnCooldown (active1Cooldown);
		for (int xAxis = -1; xAxis < 2; xAxis++) {
			for (int yAxis = -1; yAxis < 2; yAxis++) {
				int xTile = x + xAxis;
				int yTile = y + yAxis;
				if (xTile >= 0 && xTile < 18) {
					if (yTile >= 0 && yTile < 9) {
						if (grid.pieces[xTile, yTile] && !grid.pieces[xTile, yTile].checkIsDead()) {
							Piece hit = grid.pieces [xTile, yTile];
							if (hit != this) {
								if (hit.team == target.team) {
									int damageTaken = (int)(Random.Range ((active1EnemyDamage / 1.5f), active1EnemyDamage));
									Debug.Log (hit.charName + hit.team + " was hit by 'Devouring Spin' for " + damageTaken + " damage.");
									hit.takeDamage (damageTaken, this, 2);
									healDamage ((int)(Random.Range ((active1Heal / 1.25f), active1Heal)));
								} else {
									int damageTaken = (int)(Random.Range ((active1AllyDamage / 1.5f), active1AllyDamage));
									Debug.Log (hit.charName + hit.team + " was hit by 'Devouring Spin' for " + damageTaken + " damage.");
									hit.takeDamage (damageTaken, this, 2);
									healDamage ((int)(Random.Range ((active1Heal / 1.25f), active1Heal)));
								}
							}
						}
					}
				}
			}
		}
		base.setAttackState (0);
	}

	public override void activeAbility2 (Piece target) {
		useAP ();
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
			Debug.Log (charName + team + " hypnotizes " + target.charName + target.team + " to him.");
			float damageDealt = Random.Range (active2Damage / 1.5f, active2Damage);
			Debug.Log (target.charName + target.team + " is raked with claws for " + (int)damageDealt + " damage");
			target.takeDamage ((int)damageDealt, this, 2);
			active2OnCooldown (active2Cooldown);
		} else {
			Debug.Log (charName + team + " couldn't bring " + target.charName + target.team + " any closer.");
			active2OnCooldown (1);
		}
		base.setAttackState (0);
	}

	public override void active1Search (int target) {
		grid.directionalTargetting (target, active1Range);
	}

	public override void active2Search (int target) {
		grid.directionalTargetting (target, active2Range);
	}
}
