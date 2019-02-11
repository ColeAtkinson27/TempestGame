using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Piece : MonoBehaviour {

	protected int x;
	protected int y;
	protected int spawnx, spawny;
	public Controller grid;

	//==STATS==//
	protected int maxHealth;
	protected int damage;
	protected int range;
	protected int MaxAP;
	protected int attackType;
	protected int charNumber;

	public string charName;
	public int team;
	public Sprite Portrait;

	//Changing variables
	public int HP;
	public int topTempHealth = 0;
	public int tempHealth = 0;
	protected int AP;
	protected int activeState = 0;
	protected int respawnTime;
	protected bool canAttack = true;
	protected bool isDead;
	protected int statusEffect = 0;

	//active 1
	public Sprite Active1;
	protected int a1Cooldown = 0;
	//active 2
	public Sprite Active2;
	protected int a2Cooldown = 0;
	//passive
	public Sprite Passive;
	public bool useablePassive;

	protected double kills;
	protected double deaths;

	//Other
	public bool canWaterWalk;

	public GameObject model;
	public Material[] skins;
	public int activeSkin;

    //Attack Types
    //0 = melee (1)
    //1 = magic (3)
    //2 = ranged (4)

	//Status Effects
	//0 = No effect
	//1 = Slowed -- AP reduced to 1
	//2 = Rooted -- Can attack, but not move
	//3 = Sleeping -- Cannot move or attack. Being attacked wakes up hero
	//4 = Stunned -- Cannot move or attack.

	// Use this for initialization
	void Start () {
		grid = GetComponentInParent <Controller> ();
		onStart ();
		HP = maxHealth;
		AP = MaxAP;
		setSkin ();
    }
	
	// Update is called once per frame
	void Update () {
		
	}

	public void setAttackState (int state) {
		//0 = Basic attack
		//1 = Active ability 1
		//2 = Active ability 2
		activeState = state;
	}

	public void setSpawn (int xSpawn, int ySpawn) {
		spawnx = xSpawn;
		spawny = ySpawn;
	}

	public void setPosition (int setX, int setY) {
		x = setX;
		y = setY;
	}

	public int getX () {
		return x;
	}

	public int getY () {
		return y;
	}

	public virtual void startTurn () {
		if (statusEffect == 0)
			AP = MaxAP;
		activeState = 0;
		canAttack = true;
		if (isDead == true) {
			respawnTime--;
			Debug.Log (charName + team + " will respawn in " + respawnTime + " turns");
			if (respawnTime == 0) {
				Respawn ();
			}
		}
		if (a1Cooldown > 0)
			a1Cooldown--;
		if (a2Cooldown > 0)
			a2Cooldown--;
	}

	public virtual void endTurn() {
		statusEffect = 0;
	}

	public virtual void moveCharacter (Circle newPos) {
		//float pace = 100.0f * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, new Vector3 (newPos.transform.position.x, 50.25f, newPos.transform.position.z), 100);
		grid.setHeroPositions (x, y, null);
		x = newPos.x;
		y = newPos.y;
		grid.setHeroPositions (x, y, this);
	}

	public void useAP () {
		AP--;
	}

	public int checkAP () {
		return AP;
	}

	public int getRange () {
		return range;
	}

	public virtual void setStatusEffect (int effect) {
		if (effect > statusEffect) {
			statusEffect = effect;
			if (statusEffect == 1)
				AP = 1;
			else if (statusEffect == 2)
				AP = MaxAP;
			else
				AP = 0;
		} else if (effect == 0 && statusEffect != 0) {
			statusEffect = effect;
			AP = MaxAP;
		}
	}

	public virtual void wakeUp () {
		if (statusEffect == 3) {
			AP = MaxAP;
			statusEffect = 0;
		}
	}

	public virtual void attackEnemy (Piece target) {
		canAttack = false;
        int damageDealt = Random.Range((damage / 2), damage);
        int defenderType = target.attackType;

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
		AP--;
    }

	public int checkState () {
		return activeState;
	}

	public virtual void activeAbility1 (Piece target) {
	}

	public virtual void activeAbility2 (Piece target) {
	}

	public virtual void activeAbility1 (Circle target) {
	}

	public virtual void activeAbility2 (Circle target) {
	}

	public virtual void passiveAbility () {
	}

	public virtual void active1Search (int target) {
		grid.checkTargets (target);
	}
	public virtual void active2Search (int target) {
		grid.checkTargets (target);
	}

	public virtual void takeDamage (int damageTaken, Piece attacker, int abilityType) {
		//Ability types
		//0 = basic attack
		//1 = spell attack
		//2 = physical attack
		//3 = special attack
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

	public virtual void healDamage (int healAmount) {
		Debug.Log (charName + team + " heals for " + healAmount + " health");
		HP += healAmount;
		if (HP > maxHealth)
			HP = maxHealth;
	}

	public virtual void retaliate (Piece attacker) {

	}

	public virtual void defend (Piece attacker) {

	}

    public virtual void onKill() {
		kills++;
    }

	public void addKill() {
		kills++;
	}

    public virtual void onDeath () {
		if (team == 1) {
			grid.redScore = grid.redScore + 1;
		} else if (team == 2) {
			grid.blueScore = grid.blueScore + 1;
		}
		grid.setHeroPositions (x, y, null);
		grid.setBoardTile (x, y, 0);
		grid.setHeroPositions (spawnx, spawny, this);
		isDead = true;
		respawnTime = 3;
		Circle newPos = grid.checkBoardTiles (spawnx, spawny);
		x = spawnx;
		y = spawny;
		transform.position = Vector3.MoveTowards(transform.position, new Vector3 (newPos.transform.position.x, 55.5f, newPos.transform.position.z), 10000);
		Renderer rend = model.GetComponent <Renderer> ();
		rend.enabled = false;
		deaths++;
	}

	public virtual void Respawn () {
		HP = maxHealth;
		isDead = false;
		grid.setBoardTile (spawnx, spawny, team);
		Renderer rend = model.GetComponent <Renderer> ();
		rend.enabled = true;
	}

	public int getRespawn () {
		return respawnTime;
	}

	public void setSkin () {
		if (model) {
			activeSkin = SaveLoad.player.heroSkins [charNumber];
			Renderer rend = model.GetComponent <Renderer> ();
			rend.material = skins [SaveLoad.player.heroSkins [charNumber]];
		}
	}

	public double showKills () {
		return kills;
	}

	public double showDeaths () {
		return deaths;
	}

	public double showKDRatio () {
		double kdRatio;
		if (deaths == 0) {
			kdRatio = kills;
		} else {
			kdRatio = (kills / deaths);
		}
		return kdRatio;
	}

	//For inherited
	public virtual void onStart () {

	}

	public void inhDeath () {
		respawnTime = 3;
		deaths++;
	}

	public void halfKill () {
		deaths += 0.5;
	}

	//==GET STATS==//
	public int getMaxHealth () {
		return maxHealth;
	}

	//==GET STATUSES==//
	public bool checkIsDead () {
		return isDead;
	}
	public bool checkCanAttack () {
		return canAttack;
	}

	public int getActive1Cooldown () {
		return a1Cooldown;
	}

	public int getActive2Cooldown () {
		return a2Cooldown;
	}

	public int getStatusEffect () {
		return statusEffect;
	}

	public int getAttackType() {
		return attackType;
	}

	//==SET STATS==//
	public void setAP () {
		AP = MaxAP;
	}

	public void setX (int xCoord) {
		x = xCoord;
	}
	public void setY (int yCoord) {
		y = yCoord;
	}

	//==SET STATUSES==//
	public void setActiveState (int newState) {
		activeState = newState;
	}

	public void respawnTimer () {
		respawnTime--;
		Debug.Log (charName + team + " will respawn in " + respawnTime + " turns");
		if (respawnTime == 0) {
			Respawn ();
		}
	}

	public void active1OnCooldown(int cd) {
		a1Cooldown = cd;
	}
	public void active2OnCooldown(int cd) {
		a2Cooldown = cd;
	}

	public int getSpawnX() {
		return spawnx;
	}
	public int getSpawnY() {
		return spawny;
	}

	/* temp Warrior
	 * hp = 600
	 * atk = 50
	 * ap = 4
	 * 
	 * temp Mage
	 * hp = 300
	 * atk = 100
	 * ap = 3
	 * 
	 * temp Ranged
	 * hp = 200
	 * atk = 150
	 * ap = 2
	 */
}