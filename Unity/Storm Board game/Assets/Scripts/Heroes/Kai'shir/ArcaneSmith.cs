using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArcaneSmith : Piece {
	//==STATS==//
	private static int CHARHP = 300;
	private static int CHARDAM = 100;
	private static int CHARRANGE = 3;
	private static int CHARAP = 3;
	private static int CHARAT = 1;
	private static int CHARNUM = 0;
	private static int active1Range = 3;
	private static int active2Range = 1;
	private static int active1Cooldown = 2;
	private static int active2Cooldown = 2;

	public ArcaneWell well;
	public ManaMine mine;

	public override void onStart () {
		maxHealth = CHARHP;
		damage = CHARDAM;
		range = CHARRANGE;
		MaxAP = CHARAP;
		attackType = CHARAT;
		charNumber = CHARNUM;
	}

	public override void active1Search (int target) {
		for (int xRange = -active1Range; xRange <= active1Range; xRange++) {
			for (int yRange = -active1Range; yRange <= active1Range; yRange++) {
				if (Mathf.Abs(yRange) + Mathf.Abs(xRange) <= range) {
					if (x + xRange < 19 && x + xRange >= 0) {
						if (y + yRange < 9 && y + yRange >= 0) {
							int xTile = x + xRange;
							int yTile = y + yRange;
							if (grid.checkBoardTerrain (xTile, yTile) == 0) {
								if (grid.checkBoardTiles (xTile, yTile).getState () == 0) {
									grid.setBoardTile (xTile, yTile, 5);
								}
							}
						}
					}
				}
			}
		}
	}

	public override void active2Search (int target) {
		for (int xRange = -active2Range; xRange <= active2Range; xRange++) {
			for (int yRange = -active2Range; yRange <= active2Range; yRange++) {
				if (Mathf.Abs(yRange) + Mathf.Abs(xRange) <= range) {
					if (x + xRange < 19 && x + xRange >= 0) {
						if (y + yRange < 9 && y + yRange >= 0) {
							int xTile = x + xRange;
							int yTile = y + yRange;
							if (grid.checkBoardTerrain (xTile, yTile) == 0) {
								if (grid.checkBoardTiles (xTile, yTile).getState () == 0) {
									grid.setBoardTile (xTile, yTile, 5);
								}
							}
						}
					}
				}
			}
		}
	}

	public override void activeAbility1 (Circle target) {
		int xTile = target.x;
		int yTile = target.y;
		ArcaneWell AW = Instantiate (well) as ArcaneWell;
		AW.setPosition (xTile, yTile);
		//AW.transform.position.x = target.transform.position.x;
		//AW.transform.position.y = target.transform.position.y;
	}
}
