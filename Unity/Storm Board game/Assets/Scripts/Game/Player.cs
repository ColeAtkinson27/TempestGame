using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour {

	public Controller gameController;
	public string username;
	public Sprite portrait;
	private int [] heroes = new int [5];
	private int team;
	private int AP;
	public bool controllable;


	private GameObject [] heroObjects = new GameObject [5];

	// Use this for initialization
	public virtual void Start () {
		DontDestroyOnLoad (gameObject);
		AP = 12;
		controllable = true;
	}

	void Update () {
		if (!gameController)
			linkToBoard ();
	}
	
	// Update is called once per frame
	public void changeHero (int hero, int newHero) {
		heroes [hero] = newHero;
	}

	//Next two set hero objects
	public int giveHeroNumber (int hero) {
		return heroes [hero];
	}

	public void setHero (int hero, GameObject heroObject) {
		heroObjects [hero] = heroObject;
	}

	public GameObject getHero (int hero) {
		return heroObjects [hero];
	}

	public void setTeam (int side) {
		team = side;
	}

	public int checkTeam () {
		return team;
	}

	public void startTurn (int maxAP) {
		AP = maxAP;
	}

	public void increaseAP (int increase) {
		AP = AP + increase;
	}

	public void takeAction () {
		AP--;
	}

	public int checkAP () {
		return AP;
	}

	public void setRandomHeroes () {
		for (int i = 0; i < 5; i++) {
			if (heroes [i] == 0) {
				while (heroes [i] == 0) {
					bool used = false;
					int random = (int) (Random.Range (1.0f, 36.0f));
					if (SaveLoad.player.heroUnlocked [random]) {
						for (int h = 0; h < 5; h++) {
							if (random == heroes [h]) {
								used = true;
							}
						}
						if (used == false) {
							heroes [i] = random;
						}
					}
				}
			}
		}
	}

	public void linkToBoard () {
		gameController = GameObject.FindObjectOfType<Controller> ();
		if (gameController)
			gameController.setPlayer (this, team - 1);
	}

	public void endGame () {
		DestroyObject (gameObject);
	}
}
