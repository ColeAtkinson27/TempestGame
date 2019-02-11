using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keeper : Piece {
	//==STATS==//
	private static int CHARHP = 250;
	private static int CHARDAM = 40;
	private static int CHARRANGE = 3;
	private static int CHARAP = 2;
	private static int CHARAT = 1;
	private static int CHARNUM = 0;
	private static int active1Healing = 125;
	private static int active2Damage = 40;
	private static int active1Range = 3;
	private static int active2Range = 3;
	private static int active1Cooldown = 3;
	private static int active2Cooldown = 2;
	private static int passiveHealing = 30;
	private static int passiveRange = 3;

	public override void onStart () {
		maxHealth = CHARHP;
		damage = CHARDAM;
		range = CHARRANGE;
		MaxAP = CHARAP;
		attackType = CHARAT;
		charNumber = CHARNUM;
	}

	public override void endTurn () {
		statusEffect = 0;
		rejuvenate ();
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
		active1OnCooldown (active2Cooldown);
		float damageDealt = Random.Range (active2Damage / 1.5f, active2Damage);
		Debug.Log (target.charName + target.team + " is hit with 'Strangling Algae' for " + (int)damageDealt + " damage");
		target.setStatusEffect (2);
		target.takeDamage ((int)damageDealt, this, 2);
		setAttackState (0);
	}

	public override void active1Search (int target) {
		grid.radialTargetting (team, active1Range);
		grid.setBoardTile (x, y, 5);
	}

	public override void active2Search (int target) {
		grid.radialTargetting (target, active2Range);
	}

	private void rejuvenate () {
		Debug.Log (charName + team + " rejuvenates nearby allies");
		for (int xRange = 0; xRange <= passiveRange; xRange++) {
			for (int yRange = 0; yRange <= passiveRange; yRange++) {
				if (yRange + xRange <= passiveRange) {
					if (x + xRange < 19) {
						if (y + yRange < 9) {
							int xTile = x + xRange;
							int yTile = y + yRange;
							if (grid.pieces[xTile, yTile] && grid.pieces [xTile, yTile].team == team && grid.pieces [xTile, yTile].checkIsDead () == false) {
								float damageHealed = Random.Range (passiveHealing / 1.25f, passiveHealing);
								grid.pieces [xTile, yTile].healDamage ((int) damageHealed);
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
								float damageHealed = Random.Range (passiveHealing / 1.25f, passiveHealing);
								grid.pieces [xTile, yTile].healDamage ((int) damageHealed);
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
								float damageHealed = Random.Range (passiveHealing / 1.25f, passiveHealing);
								grid.pieces [xTile, yTile].healDamage ((int) damageHealed);
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
								float damageHealed = Random.Range (passiveHealing / 1.25f, passiveHealing);
								grid.pieces [xTile, yTile].healDamage ((int) damageHealed);
							}
						}
					}
				}
			}
		}
	}
}
