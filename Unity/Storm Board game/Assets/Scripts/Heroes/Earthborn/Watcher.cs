using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Watcher : Piece {
	//==STATS==//
	private static int CHARHP = 300;
	private static int CHARDAM = 100;
	private static int CHARRANGE = 3;
	private static int CHARAP = 3;
	private static int CHARAT = 1;
	private static int CHARNUM = 0;
	private static int active1Damage = 200;
	private static int active2Damage = 200;
	private static int active1Range = 3;
	private static int active2Range = 3;
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
