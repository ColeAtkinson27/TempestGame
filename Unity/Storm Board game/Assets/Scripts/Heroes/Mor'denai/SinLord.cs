using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SinLord : Piece {
	//==STATS==//
	private static int CHARHP = 300;
	private static int CHARDAM = 60;
	private static int CHARRANGE = 3;
	private static int CHARAP = 3;
	private static int CHARAT = 1;
	private static int CHARNUM = 0;
	private static int active1Damage = 100;
	private static int active2Damage = 100;
	private static int active1Range = 3;
	private static int active2Range = 3;
	private static int active1Cooldown = 2;
	private static int active2Cooldown = 2;

	private int currentSin = 0;
	//0 IRA
	//1 AVARITIA
	//2 LUXURIA
	//3 INVIDIA
	//4 GULA
	//5 ACEDIA
	//6 SUPERBIA
	private static float IRABONUS = 1.30f;
	private static float AVARITIABONUS = 0.50f;
	private static float LUXURIABONUS = 0.75f;
	private static float INVIDIABONUS = 0.30f;
	private static float GULABONUS = 0.30f;
	private static int ACEDIABONUS = 50;
	//private static float SUPERBIABONUS = 0.70f;
	public Sprite [] sinPassives;

	public override void onStart () {
		maxHealth = CHARHP;
		damage = CHARDAM;
		range = CHARRANGE;
		MaxAP = CHARAP;
		attackType = CHARAT;
		charNumber = CHARNUM;
	}

	public override void startTurn() {
		tempHealth = 0;
		if (statusEffect == 0)
			AP = MaxAP;
		activeState = 0;
		canAttack = true;
		if (isDead == true) {
			respawnTimer ();
		}
		if (a1Cooldown > 0)
			active1OnCooldown(a1Cooldown - 1);
		if (a2Cooldown > 0)
			active2OnCooldown(a2Cooldown - 1);
		currentSin = (int) (Random.Range (0, 7));
		switch (currentSin) {
			case 0:
				Debug.Log (charName + team + "'s current sin: Ira");
				break;
			case 1:
				Debug.Log (charName + team + "'s current sin: Avaritia");
				break;
			case 2:
				Debug.Log (charName + team + "'s current sin: Luxuria");
				break;
			case 3:
				Debug.Log (charName + team + "'s current sin: Invidia");
				break;
			case 4:
				Debug.Log (charName + team + "'s current sin: Gula");
				break;
		case 5:
			AP = 1;
			topTempHealth = ACEDIABONUS;
			tempHealth = ACEDIABONUS;
			Debug.Log (charName + team + "'s current sin: Acedia");
				break;
			case 6:
				Debug.Log (charName + team + "'s current sin: Superbia");
				topTempHealth = ACEDIABONUS;
				tempHealth = ACEDIABONUS;
				break;
		}
		Passive = sinPassives[currentSin];
	}

	public override void attackEnemy (Piece target) {
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
		if (currentSin == 4 || currentSin == 6)
			healDamage ((int) (damageDealt * GULABONUS));
		if (currentSin != 1 && currentSin != 6)
			AP--;
	}

	public override void takeDamage (int damageTaken, Piece attacker, int abilityType) {
		//Ability types
		//0 = basic attack
		//1 = spell attack
		//2 = physical attack
		//3 = special attack
		if (currentSin == 2 || currentSin == 6) {
			damageTaken = (int) (damageTaken * LUXURIABONUS);
		}
		if (currentSin == 3 || currentSin == 6) {
			attacker.takeDamage ((int) (damageTaken * INVIDIABONUS), this, 3);
			damageTaken -= (int)(damageTaken * INVIDIABONUS);
		}
		if (tempHealth == 0) {
			HP = HP - damageTaken;
			wakeUp ();
			if (HP <= 0) {
				attacker.onKill ();
				onDeath ();
			}
		} else {
			tempHealth -= damageTaken;
			if (tempHealth >= 0) {
				tempHealth = 0;
				topTempHealth = 0;
			}
		}
	}

	public override void activeAbility1 (Piece target) {
		if (currentSin != 1 && currentSin != 6)
			useAP ();
		active1OnCooldown (active1Cooldown);
		float damageDealt = Random.Range (active1Damage / 1.5f, active1Damage);
		if (currentSin == 0 || currentSin == 6)
			damageDealt = damageDealt * IRABONUS;
		else if (currentSin == 1)
			damageDealt = damageDealt * AVARITIABONUS;
		Debug.Log (target.charName + target.team + " is hit with 'Hellish Shot' for " + (int)damageDealt + " damage");
		target.takeDamage ((int)damageDealt, this, 1);
		if (currentSin == 4 || currentSin == 6)
			healDamage ((int) (damageDealt * GULABONUS));
		setAttackState (0);
	}

	public override void activeAbility2 (Piece target) {
		if (currentSin != 1 && currentSin != 6)
			useAP ();
		active2OnCooldown (active2Cooldown);
		float damageDealt = Random.Range (active2Damage / 1.5f, active2Damage);
		if (currentSin == 0 || currentSin == 6)
			damageDealt = damageDealt * IRABONUS;
		else if (currentSin == 1)
			damageDealt = damageDealt * AVARITIABONUS;
		Debug.Log (target.charName + target.team + " is hit with 'Dark Lightning' for " + (int)damageDealt + " damage");
		target.takeDamage ((int)damageDealt, this, 1);
		if (currentSin == 4 || currentSin == 6)
			healDamage ((int) (damageDealt * GULABONUS));
		setAttackState (0);
	}

	public override void active1Search (int target) {
		grid.radialTargetting (target, active1Range);
	}

	public override void active2Search (int target) {
		grid.radialTargetting (target, active2Range);
	}
}
