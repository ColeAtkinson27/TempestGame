using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostKnight : Piece {
	//==STATS==//
	private static int CHARHP = 800;
	private static int CHARDAM = 60;
	private static int CHARRANGE = 1;
	private static int CHARAP = 4;
	private static int CHARAT = 0;
	private static int CHARNUM = 0;
	private static int active1Damage = 75;
	private static int active2Damage = 80;
	private static int active1Range = 1;
	private static int active2Range = 1;
	private static int active1Cooldown = 3;
	private static int active2Cooldown = 3;
	private static int active1HealAmount = 60;

	public override void onStart () {
		maxHealth = CHARHP;
		damage = CHARDAM;
		range = CHARRANGE;
		MaxAP = CHARAP;
		attackType = CHARAT;
		charNumber = CHARNUM;
	}

	public override void activeAbility1 (Piece target) {
		useAP ();
		active1OnCooldown (active1Cooldown);
		float damageDealt = Random.Range (active1Damage / 1.5f, active1Damage);
		Debug.Log (target.charName + target.team + " is hit with 'Sapping Slice' for " + (int)damageDealt + " damage");
		target.takeDamage ((int)damageDealt, this, 2);
		healDamage (active1HealAmount);
		base.setAttackState (0);
	}

	public override void activeAbility2 (Piece target) {
		useAP ();
		active2OnCooldown (active2Cooldown);
		float damageDealt = Random.Range (active2Damage / 1.5f, active2Damage);
		Debug.Log (target.charName + target.team + " is hit with 'Freezing Strike' for " + (int)damageDealt + " damage");
		target.setStatusEffect (1);
		target.takeDamage ((int)damageDealt, this, 2);
		base.setAttackState (0);
	}

	public override void active1Search (int target) {
		grid.radialTargetting (target, active1Range);
	}

	public override void active2Search (int target) {
		grid.radialTargetting (target, active2Range);
	}
}
