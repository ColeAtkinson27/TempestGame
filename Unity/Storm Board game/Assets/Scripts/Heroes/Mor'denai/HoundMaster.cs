using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoundMaster : Piece {
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

	public GameObject wolfModel;
	private bool wolfIsDead;
	private int wolfRespawnTime;

	public override void onStart () {
		maxHealth = CHARHP;
		damage = CHARDAM;
		range = CHARRANGE;
		MaxAP = CHARAP;
		attackType = CHARAT;
		charNumber = CHARNUM;
	}

	public override void endTurn() {
		setAP();
		setActiveState(0);
		canAttack = true;
		if (isDead == true) {
			respawnTimer ();
		} else if (wolfIsDead == true) {
			wolfRespawnTimer ();
		}
		if (a1Cooldown > 0)
			active1OnCooldown(a1Cooldown - 1);
		if (a2Cooldown > 0)
			active2OnCooldown(a2Cooldown - 1);
	}

	public override void onDeath () {
		if (wolfIsDead == true) {
			if (team == 1) {
				grid.redScore = grid.redScore + 0.5;
			} else if (team == 2) {
				grid.blueScore = grid.blueScore + 0.5;
			}
			wolfIsDead = true;
			wolfRespawnTime = 2;
			halfKill ();
		} else {
			if (team == 1) {
				grid.redScore = grid.redScore + 1;
			} else if (team == 2) {
				grid.blueScore = grid.blueScore + 1;
			}
			grid.setHeroPositions (getX (), getY (), null);
			grid.setBoardTile (getX (), getY (), 0);
			grid.setHeroPositions (getSpawnX (), getSpawnY (), this);
			isDead = true;
			Circle newPos = grid.checkBoardTiles (getSpawnX (), getSpawnY ());
			setX (getSpawnX ());
			setY (getSpawnX ());
			transform.position = Vector3.MoveTowards(transform.position, new Vector3 (newPos.transform.position.x, 55.5f, newPos.transform.position.z), 10000);
			Renderer rend = model.GetComponent <Renderer> ();
			rend.enabled = false;
			inhDeath ();
		}
	}

	public override void Respawn () {
		HP = maxHealth;
		isDead = false;
		wolfIsDead = false;
		Renderer rend = model.GetComponent <Renderer> ();
		Renderer rendw = wolfModel.GetComponent <Renderer> ();
		rend.enabled = true;
		rendw.enabled = true;
	}

	public void wolfRespawnTimer () {
		wolfRespawnTime--;
		Debug.Log ("Hound will respawn in " + wolfRespawnTime + " turns");
		if (wolfRespawnTime == 0) {
			wolfRespawn ();
		}
	}

	public void wolfRespawn () {
		HP = maxHealth;
		wolfIsDead = false;
		Renderer rend = wolfModel.GetComponent <Renderer> ();
		rend.enabled = true;
	}
}
