using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimelessOne : Piece {
	//==STATS==//
	private static int CHARHP = 280;
	private static int CHARDAM = 50;
	private static int CHARRANGE = 3;
	private static int CHARAP = 3;
	private static int CHARAT = 1;
	private static int CHARNUM = 0;
	private static int active1Damage = 60;
	private static int active1Heal = 60;
	private static int active2Damage = 30;
	private static int active1Range = 3;
	private static int active2Range = 3;
	private static int active1Cooldown = 2;
	private static int active2Cooldown = 3;

	private int active1State = 0;
	public Sprite fastForward;
	public Sprite rewind;

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
		if (checkIsDead() == true) {
			base.respawnTimer ();
		}
		if (base.getActive1Cooldown() > 0)
			base.active1OnCooldown(base.getActive1Cooldown() - 1);
		if (base.getActive2Cooldown() > 0)
			base.active2OnCooldown(base.getActive2Cooldown() - 1);
		grid.getPlayer (team - 1).increaseAP (5);
		Debug.Log (charName + team + " adds 5 to their team's total AP");
	}

	public override void activeAbility1 (Piece target) {
		useAP ();
		active1OnCooldown (active1Cooldown);
		if (active1State == 0) {
			float damageDealt = Random.Range (active1Damage / 1.5f, active1Damage);
			Debug.Log (target.charName + target.team + " is hit with 'Fast Forward' for " + (int)damageDealt + " damage");
			target.takeDamage ((int)damageDealt, this, 1);
			Active1 = fastForward;
			active1State = 1;
		} else if (active1State == 1) {
			float damageHealed = Random.Range (active1Heal / 1.5f, active1Heal);
			target.healDamage ((int)(damageHealed));
			Active1 = rewind;
			active1State = 0;
		}
		base.setAttackState (0);
	}

	public override void activeAbility2 (Piece target) {
		useAP ();
		active2OnCooldown (active2Cooldown);
		float damageDealt = Random.Range (active2Damage / 1.5f, active2Damage);
		Debug.Log (target.charName + target.team + " is hit with 'Temporal Shift' for " + (int)damageDealt + " damage");
		target.setStatusEffect (4);
		target.takeDamage ((int)damageDealt, this, 1);
		base.setAttackState (0);
	}

	public override void active1Search (int target) {
		if (active1State == 0)
		    grid.radialTargetting (target, active1Range);
		else if (active1State == 1)
			grid.radialTargetting (team, active1Range);
	}

	public override void active2Search (int target) {
		grid.radialTargetting (target, active2Range);
	}
}
