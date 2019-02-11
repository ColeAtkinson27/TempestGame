using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lich : Piece {

	public GameObject doll;
	private Animator animator;

	//==STATS==//
	private static int CHARHP = 300;
	private static int CHARDAM = 60;
	private static int CHARRANGE = 3;
	private static int CHARAP = 3;
	private static int CHARAT = 1;
	private static int CHARNUM = 16;
	private static int active1Damage = 75;
	private static int active2Damage = 150;
	private static int active1Range = 3;
	private static int active2Range = 3;
	private static int active1Cooldown = 3;
	private static int active2Cooldown = 3;

	public override void onStart () {
		Renderer dollRend = doll.GetComponent <Renderer> ();
		dollRend.enabled = false;
		animator = GetComponent <Animator> ();
		maxHealth = CHARHP;
		damage = CHARDAM;
		range = CHARRANGE;
		MaxAP = CHARAP;
		attackType = CHARAT;
		charNumber = CHARNUM;
		setSkin ();
	}

	public override void onDeath () {
		Debug.Log (charName + team + " retreats into his phylactery");
		StartCoroutine(deathAnimation ());
		if (team == 1) {
			grid.redScore = grid.redScore + 1;
		} else if (team == 2) {
			grid.blueScore = grid.blueScore + 1;
		}
		isDead = true;
		inhDeath ();
	}

	public override void Respawn () {
		HP = maxHealth;
		isDead = false;
		Renderer rend = model.GetComponent <Renderer> ();
		rend.enabled = true;
		Renderer dollRend = doll.GetComponent <Renderer> ();
		dollRend.enabled = false;
	}

	public override void activeAbility1 (Piece target) {
		Debug.Log (target.charName + target.team + " and nearby targets are hit with 'Dark Cloud'");
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
								float damageDealt = Random.Range (active1Damage / 1.5f, active1Damage);
								hit.takeDamage ((int)damageDealt, this, 1);
								Debug.Log (hit.charName + target.team + " was affected by 'Dark Cloud' for " + (int)damageDealt + " damage");
							}
						}
					}
				}
			}
		}
	    active1OnCooldown (active1Cooldown);
		setAttackState (0);
	}

	public override void activeAbility2 (Piece target) {
		useAP ();
		float damageDealt = Random.Range (active2Damage / 1.5f, active2Damage);
		target.takeDamage ((int)damageDealt, this, 1);
		active2OnCooldown (active2Cooldown);
		setAttackState (0);
		Debug.Log (target.charName + target.team + " is hit with 'Shadow Ray' for " + (int)damageDealt + " damage");
	}

	public override void active1Search (int target) {
		grid.radialTargetting (target, active1Range);
	}

	public override void active2Search (int target) {
		int range = 0;
		range = active2Range;
		//EAST
		for (int xRange = 1; xRange <= range; xRange++) {
			if ((base.getX () + xRange) < 18) {
				int xTile = getX () + xRange;
				if (grid.checkBoardTerrain (xTile, getY ()) != 2) {
					if (grid.checkBoardTiles (xTile, getY ()).getState () == target && grid.pieces [xTile, getY ()].checkIsDead () == false) {
						grid.setBoardTile (xTile, getY (), 5);
						xRange = range;
					} else {
						grid.setBoardTile (xTile, getY (), 6);
					}
				}
			}
		}
		//SOUTHEAST
		for (int yRange = -1; yRange >= -range; yRange--) {
				if ((base.getY () + yRange) >= 0) {
					if ((base.getX () - yRange) < 18) {
						int yTile = getY () + yRange;
						int xTile = getX () - yRange;
						if (grid.checkBoardTerrain (xTile, yTile) != 2) {
							if (grid.checkBoardTiles (xTile, yTile).getState () == target && grid.pieces [xTile, yTile].checkIsDead () == false) {
								grid.setBoardTile (xTile, yTile, 5);
								yRange = -range;
							} else {
								grid.setBoardTile (xTile, yTile, 6);
							}
						}
					}
				}
		}

		//SOUTH
		for (int yRange = -1; yRange >= -range; yRange--) {
			if ((base.getY () + yRange) >= 0) {
				int yTile = getY () + yRange;
				if (grid.checkBoardTerrain (getX (), yTile) != 2) {
					if (grid.checkBoardTiles (getX (), yTile).getState () == target && grid.pieces [getX (), yTile].checkIsDead () == false) {
						grid.setBoardTile (getX (), yTile, 5);
						yRange = -range;
					} else {
						grid.setBoardTile (getX (), yTile, 6);
					}
				}
			}
		}
		//SOUTHWEST
		for (int yRange = -1; yRange >= -range; yRange--) {
				if ((base.getY () + yRange) >= 0) {
					if ((base.getX () + yRange) >= 0) {
						int yTile = getY () + yRange;
						int xTile = getX () + yRange;
						if (grid.checkBoardTerrain (xTile, yTile) != 2) {
							if (grid.checkBoardTiles (xTile, yTile).getState () == target && grid.pieces [xTile, yTile].checkIsDead () == false) {
								grid.setBoardTile (xTile, yTile, 5);
								yRange = -range;
							} else {
								grid.setBoardTile (xTile, yTile, 6);
							}
						}
					}
				}
		}

		//WEST
		for (int xRange = -1; xRange >= -range; xRange--) {
			if ((base.getX () + xRange) >= 0) {
				int xTile = getX () + xRange;
				if (grid.checkBoardTerrain (xTile, getY()) != 2) {
					if (grid.checkBoardTiles (xTile, getY()).getState () == target && grid.pieces [xTile, getY()].checkIsDead () == false) {
						grid.setBoardTile (xTile, getY(), 5);
						xRange = -range;
					} else {
						grid.setBoardTile (xTile, getY(), 6);
					}
				}
			}
		}
		//NORTHWEST
		for (int yRange = 1; yRange <= range; yRange++) {
				if ((base.getY () + yRange) < 9) {
					if ((base.getX () - yRange) >= 0) {
						int yTile = getY () + yRange;
						int xTile = getX () - yRange;
						if (grid.checkBoardTerrain (xTile, yTile) != 2) {
							if (grid.checkBoardTiles (xTile, yTile).getState () == target && grid.pieces [xTile, yTile].checkIsDead () == false) {
								grid.setBoardTile (xTile, yTile, 5);
								yRange = range;
							} else {
								grid.setBoardTile (xTile, yTile, 6);
							}
						}
					}
				}
		}

		//NORTH
		for (int yRange = 1; yRange <= range; yRange++) {
			if ((getY () + yRange) < 9) {
				int yTile = getY () + yRange;
				if (grid.checkBoardTerrain (getX(), yTile) != 2) {
					if (grid.checkBoardTiles (getX(), yTile).getState () == target && grid.pieces [getX (), yTile].checkIsDead () == false) {
						grid.setBoardTile (getX(), yTile, 5);
						yRange = range;
					} else {
						grid.setBoardTile (getX(), yTile, 6);
					}
				}
			}
		}
		//NORTHEAST
		for (int yRange = 1; yRange <= range; yRange++) {
				if ((base.getY () + yRange) < 9) {
					if ((base.getX () + yRange) < 18) {
						int yTile = getY () + yRange;
						int xTile = getX () + yRange;
						if (grid.checkBoardTerrain (xTile, yTile) != 2) {
							if (grid.checkBoardTiles (xTile, yTile).getState () == target && grid.pieces [xTile, yTile].checkIsDead () == false) {
								grid.setBoardTile (xTile, yTile, 5);
								yRange = range;
							} else {
								grid.setBoardTile (xTile, yTile, 6);
							}
						}
					}
				}
			}
	}

	//==ANIMATIONS==//
	private IEnumerator deathAnimation () {
		Renderer dollRend = doll.GetComponent <Renderer> ();
		dollRend.enabled = true;
		//animator.Play ("Death");
		//yield return new WaitForSeconds (4.167f);
		Renderer modelRend = model.GetComponent <Renderer> ();
		modelRend.enabled = false;
		yield return new WaitForSeconds (4.167f);
		//animator.Play ("Idle");
	}

	private IEnumerator active1Animation () {
		animator.Play ("Ability 1");
		yield return new WaitForSeconds (4.375f);
	}

	private IEnumerator active2Animation () {
		animator.Play ("Attack");
		yield return new WaitForSeconds (3.750f);
	}
}
