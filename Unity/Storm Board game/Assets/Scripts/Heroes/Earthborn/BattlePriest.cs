using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePriest : Piece {
	//==STATS==//
	private static int CHARHP = 300;
	private static int CHARDAM = 30;
	private static int CHARRANGE = 1;
	private static int CHARAP = 3;
	private static int CHARAT = 0;
	private static int CHARNUM = 0;
	private static int active1Healing = 100;
	private static int active2Damage = 65;
	private static int active1Range = 2;
	private static int active2Range = 1;
	private static int active1Cooldown = 3;
	private static int active2Cooldown = 3;
	private static int passiveHealing = 75;
	private static int passiveRange = 2;

	public override void onStart () {
		maxHealth = CHARHP;
		damage = CHARDAM;
		range = CHARRANGE;
		MaxAP = CHARAP;
		attackType = CHARAT;
		charNumber = CHARNUM;
	}

	public override void startTurn () {
		salvation ();
		if (statusEffect == 0)
			AP = MaxAP;
		activeState = 0;
		canAttack = true;
		if (isDead == true) {
			respawnTime--;
			Debug.Log (charName + " will respawn in " + respawnTime + " turns");
			if (respawnTime == 0) {
				Respawn ();
			}
		}
		if (a1Cooldown > 0)
			a1Cooldown--;
		if (a2Cooldown > 0)
			a2Cooldown--;
	}

	public override void activeAbility1 (Piece target) {
		useAP ();
		active1OnCooldown (active1Cooldown);
		float damageHealed = Random.Range (active1Healing / 1.25f, active1Healing);
		target.healDamage ((int)damageHealed);
		base.setAttackState (0);
	}

	public override void activeAbility2 (Piece target) {
		useAP ();
		active2OnCooldown (active2Cooldown);
		float damageDealt = Random.Range (active2Damage / 1.5f, active2Damage);
		Debug.Log (target.charName + target.team + " is hit with 'Blessed Mace' for " + (int)damageDealt + " damage");
		target.takeDamage ((int)damageDealt, this, 2);
		base.setAttackState (0);
	}

	public override void active1Search (int target) {
		grid.radialTargetting (team, active1Range);
		grid.setBoardTile (x, y, 5);
	}

	public override void active2Search (int target) {
		grid.radialTargetting (target, active2Range);
	}

	private void salvation () {
		float distance = 999999;
		Piece saved = this;
		for (int xRange = 0; xRange <= passiveRange; xRange++) {
			for (int yRange = 0; yRange <= passiveRange; yRange++) {
				if (yRange + xRange <= passiveRange) {
					if (x + xRange < 19) {
						if (y + yRange < 9) {
							int xTile = x + xRange;
							int yTile = y + yRange;
							if (grid.pieces[xTile, yTile] && grid.pieces [xTile, yTile].team == team && grid.pieces [xTile, yTile].checkIsDead () == false) {
								Piece target = grid.pieces [xTile, yTile];
								if (target.charName != "Alnia") {
									float d = Mathf.Sqrt (xRange ^ 2 + yRange ^ 2);
									if (d < distance) {
										distance = d;
										saved = target;
									}
								}
							}
						}
					}
				}
			}
		}
		//Quadrant 2
		for (int xRange = 1; xRange < passiveRange; xRange++) {
			for (int yRange = 0; yRange < passiveRange; yRange++) {
				if (yRange + xRange <= passiveRange) {
					if (x - xRange > 0) {
						if (y + yRange < 9) {
							int xTile = x - xRange;
							int yTile = y + yRange;
							if (grid.pieces[xTile, yTile] && grid.pieces [xTile, yTile].team == team && grid.pieces [xTile, yTile].checkIsDead () == false) {
								Piece target = grid.pieces [xTile, yTile];
								float d = Mathf.Sqrt (xRange ^ 2 + yRange ^ 2);
								if (d < distance) {
									distance = d;
									saved = target;
								}
							}
						}
					}
				}
			}
		}
		//Quadrant 3
		for (int xRange = 1; xRange <= passiveRange; xRange++) {
			for (int yRange = 1; yRange <= passiveRange; yRange++) {
				if (yRange + xRange <= passiveRange) {
					if (x - xRange >= 0) {
						if (y - yRange >= 0) {
							int xTile = x - xRange;
							int yTile = y - yRange;
							if (grid.pieces[xTile, yTile] && grid.pieces [xTile, yTile].team == team && grid.pieces [xTile, yTile].checkIsDead () == false) {
								Piece target = grid.pieces [xTile, yTile];
								float d = Mathf.Sqrt (xRange ^ 2 + yRange ^ 2);
								if (d < distance) {
									distance = d;
									saved = target;
								}
							}
						}
					}
				}
			}
		}
		//Quadrant 4
		for (int xRange = 0; xRange < passiveRange; xRange++) {
			for (int yRange = 1; yRange < passiveRange; yRange++) {
				if (yRange + xRange <= passiveRange) {
					if (x + xRange < 19) {
						if (y - yRange >= 0) {
							int xTile = x + xRange;
							int yTile = y - yRange;
							if (grid.pieces[xTile, yTile] && grid.pieces [xTile, yTile].team == team && grid.pieces [xTile, yTile].checkIsDead () == false) {
								Piece target = grid.pieces [xTile, yTile];
								float d = Mathf.Sqrt (xRange ^ 2 + yRange ^ 2);
								if (d < distance) {
									distance = d;
									saved = target;
								}
							}
						}
					}
				}
			}
		}
		float damageHealed = Random.Range (passiveHealing / 1.25f, passiveHealing);
		saved.healDamage ((int) damageHealed);
		Debug.Log (saved.charName + saved.team + " was healed by salvation");
	}
}
