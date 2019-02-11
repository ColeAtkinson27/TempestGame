using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UndyingHorror : Piece {
	//==STATS==//
	private static int CHARHP = 600;
	private static int CHARDAM = 50;
	private static int CHARRANGE = 1;
	private static int CHARAP = 4;
	private static int CHARAT = 0;
	private static int CHARNUM = 0;
	private static int active1Damage = 100;
	private static int active2Damage = 100;
	private static int active1Range = 1;
	private static int active2Range = 1;
	private static int active1Cooldown = 1;
	private static int active2Cooldown = 1;

	public override void onStart () {
		maxHealth = CHARHP;
		damage = CHARDAM;
		range = CHARRANGE;
		MaxAP = CHARAP;
		attackType = CHARAT;
		charNumber = CHARNUM;
	}
}
