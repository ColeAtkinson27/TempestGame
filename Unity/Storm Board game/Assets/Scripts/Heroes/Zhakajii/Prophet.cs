using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Prophet : Piece {
	//==STATS==//
	private static int CHARBASEHP = 500;
	private static int CHARBASEDAM = 50;
	private static int BONUSHP = 50;
	private static int BONUSDAM = 10;
	private static int CHARRANGE = 1;
	private static int CHARAP = 4;
	private static int CHARAT = 0;
	private static int CHARNUM = 0;
	private static int active1Damage = 80;
	private static int active2Damage = 60;
	private static int a2BonusDamage = 20;
	private static int active1Range = 2;
	private static int active2Range = 1;
	private static int active1Cooldown = 3;
	private static int active2Cooldown = 3;

	public GameObject acolyte1;
	public GameObject acolyte2;
	public GameObject acolyte3;
	public GameObject acolyte4;
	private int acolytes;
	public Sprite [] passiveCount;

	public override void onStart () {
		maxHealth = CHARBASEHP;
		damage = CHARBASEDAM;
		range = CHARRANGE;
		MaxAP = CHARAP;
		attackType = CHARAT;
		charNumber = CHARNUM;
		acolytes = 0;
		active2OnCooldown (1);
	}

	public override void startTurn() {
		setAP();
		setActiveState(0);
		canAttack = true;
		if (checkIsDead() == true) {
			respawnTimer ();
		}
		if (getActive1Cooldown() > 0)
			active1OnCooldown(getActive1Cooldown() - 1);
		if (getActive2Cooldown() > 0)
			active2OnCooldown(getActive2Cooldown() - 1);
		acolytes++;
		countAcolytes ();
		if (acolytes < 5)
			active2OnCooldown (1);
	}

	public override void takeDamage (int damageTaken, Piece attacker, int abilityType) {
		HP = HP - damageTaken;
		if (HP <= 0) {
			attacker.onKill ();
			onDeath ();
		}
		int checkHealth = CHARBASEHP + ((acolytes - 1) * BONUSHP);
		if (HP < checkHealth) {
			acolytes--;
			countAcolytes ();
		}
	}

	private void countAcolytes() {
		if (acolytes > 20)
			acolytes = 20;
		acolyte1.SetActive (false);
		acolyte2.SetActive (false);
		acolyte3.SetActive (false);
		acolyte4.SetActive (false);
		if (acolytes >= 1) {
			acolyte1.SetActive (true);
			if (acolytes >= 3) {
				acolyte2.SetActive (true);
				if (acolytes >= 6) {
					acolyte3.SetActive (true);
					if (acolytes >= 10) {
						acolyte4.SetActive (true);
					}
				}
			}
		}
		maxHealth = CHARBASEHP + (acolytes * BONUSHP);
		damage = CHARBASEDAM + (acolytes * BONUSDAM);
		HP += BONUSHP;
		Passive = passiveCount [acolytes];
		Debug.Log (charName + team + "'s current acolytes: " + acolytes);
	}

	public override void activeAbility1 (Piece target) {
		useAP ();
		active1OnCooldown (active1Cooldown);
		float damageDealt = Random.Range (active1Damage / 1.5f, active1Damage);
		Debug.Log (target.charName + target.team + " is hit with 'Shifting Sands' for " + (int)damageDealt + " damage");
		target.setStatusEffect (2);
		target.takeDamage ((int)damageDealt, this, 2);
		setAttackState (0);
	}

	public override void activeAbility2 (Piece target) {
		int enemy = target.team;
		float dam = active2Damage + ((acolytes - 5) * a2BonusDamage);
		for (int x = -active2Range; x < active2Range; x++) {
			for (int y = -active2Range; y < active2Range; y++) {
				int xTile = getX () + x;
				int yTile = getY () + y;
				if (grid.pieces [xTile, yTile] != null && grid.pieces [xTile, yTile].team == enemy) {
					Piece hit = grid.pieces [xTile, yTile];
					float damageDealt = Random.Range (dam / 1.5f, dam);
					Debug.Log (hit.charName + target.team + " is hit with 'Submission' for " + (int)damageDealt + " damage");
					hit.takeDamage ((int)damageDealt, this, 2);
				}
			}
		}
		useAP ();
		active2OnCooldown (active2Cooldown);
		setAttackState (0);
	}

	public override void active1Search (int target) {
		grid.radialTargetting (target, active1Range);
	}

	public override void active2Search (int target) {
		grid.directionalTargetting (target, active2Range);
	}
}
