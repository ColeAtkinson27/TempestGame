using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MasterHunter : Piece {
	//==STATS==//
	private static int CHARHP = 200;
	private static int CHARDAM = 150;
	private static int CHARRANGE = 5;
	private static int CHARAP = 2;
	private static int CHARAT = 2;
	private static int CHARNUM = 0;
	private static int active1Damage = 300;
	private static int active2Damage = 300;
	private static int active1Range = 4;
	private static int active2Range = 4;
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
