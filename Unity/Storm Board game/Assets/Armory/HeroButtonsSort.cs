using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroButtonsSort : MonoBehaviour {

	private static int columns = 7;
	private bool resize = false;

	public HeroButton [] heroes;
	public HeroButtonAbility [] abilities;
	public Text heroName;

	// Use this for initialization
	void Start () {
		resetSize ();
		HeroButton [] sort = new HeroButton [heroes.Length];
		HeroButton storage;
		for (int h = 0; h < heroes.Length; h++) {
			sort [h] = heroes [h];
		}
		//Sorting
		for (int m = 0; m < heroes.Length; m++) {
			for (int h = 0; h < heroes.Length; h++) {
				if (h + 1 < heroes.Length) {
					if (string.Compare (heroes [h].heroName, heroes [h + 1].heroName) > 0) {
						storage = sort [h];
						sort [h] = sort [h + 1];
						sort [h + 1] = storage;
					}
				}
			}
		}
		//Making buttons
		for (int h = 0; h < heroes.Length; h++) {
			createButton (sort[h].hero, h);
		}
		setSize ();
	}

	// Update is called once per frame
	public void setSize () {
		//base width = 1185
		//base height = 852
		RectTransform size = this.GetComponent <RectTransform> ();
		float x = 0.55f;
		float y = 0.55f;
		size.transform.localScale = new Vector3 (x, y, 1);
	}

	private void resetSize () {
		resize = false;
		RectTransform size = this.GetComponent <RectTransform> ();
		float x = 1;
		float y = 1;
		size.transform.localScale = new Vector3 (x, y, 1);
	}

	private void createButton (int button, int location) {
		HeroButton hero = Instantiate (heroes [button]) as HeroButton;
		hero.transform.SetParent (transform);
		hero.transform.position = new Vector3 (location % columns * 100 - 80, location / columns * - 100 + 375, 0);
	}

	private HeroButton [] normalize () {
		resetSize ();
		HeroButton [] sort = new HeroButton [heroes.Length];
		HeroButton storage;
		for (int h = 0; h < heroes.Length; h++) {
			sort [h] = heroes [h];
		}
		//Sorting
		for (int m = 0; m < heroes.Length; m++) {
			for (int h = 0; h < heroes.Length; h++) {
				if (h + 1 < heroes.Length) {
					if (string.Compare (sort [h].heroName, sort [h + 1].heroName) > 0) {
						storage = sort [h];
						sort [h] = sort [h + 1];
						sort [h + 1] = storage;
					}
				}
			}
		}
		return (sort);
	}

	//==SORTING BUTTONS==//
	//--BY NAME--//
	public void sortByName () {
		normalize ();
		HeroButton [] sort = new HeroButton [heroes.Length];
		HeroButton storage;
		for (int h = 0; h < heroes.Length; h++) {
			sort [h] = heroes [h];
		}
		//Sorting
		for (int m = 0; m < heroes.Length; m++) {
			for (int h = 0; h < heroes.Length; h++) {
				if (h + 1 < heroes.Length) {
					if (string.Compare (sort [h].heroName, sort [h + 1].heroName) > 0) {
						storage = sort [h];
						sort [h] = sort [h + 1];
						sort [h + 1] = storage;
					}
				}
			}
		}
		removeOldButtons ();
		//Making buttons
		for (int h = 0; h < heroes.Length; h++) {
			createButton (sort[h].hero, h);
		}
		setSize ();
	}

	//--BY RACE--//
	public void sortByRace () {
		HeroButton [] sort = normalize ();
		HeroButton storage;
		//Sorting
		for (int m = 0; m < heroes.Length; m++) {
			for (int h = 0; h < heroes.Length; h++) {
				if (h + 1 < heroes.Length) {
					if (string.Compare (sort [h].race, sort [h + 1].race) > 0) {
						storage = sort [h];
						sort [h] = sort [h + 1];
						sort [h + 1] = storage;
					}
				}
			}
		}
		removeOldButtons ();
		//Making buttons
		for (int h = 0; h < heroes.Length; h++) {
			createButton (sort[h].hero, h);
		}
		setSize ();
	}

	//  DWARF = 0  //
	//  EARTHBORN = 1  //
	//  ELF = 2 //
	//  GOBLIN = 3  //
	//  JORDESH = 4  //
	//  KAI'SHIR = 5  //
	//  MOR'DENAI = 6  //
	//  NECARU = 7  //
	//  ZHAKAJII = 8  //
	public void sortSpecificRace (string race) {
		HeroButton [] sort = normalize ();
		HeroButton [] racial = new HeroButton [heroes.Length];
		int storage = 0;

		for (int h = 0; h < heroes.Length; h++) {
			if (sort [h].race == race) {
				racial [storage] = sort [h];
				storage++;
			}
		}

		removeOldButtons ();
		//Making buttons
		for (int h = 0; h < racial.Length; h++) {
			if (racial [h])
				createButton (racial[h].hero, h);
		}
		setSize ();
	}
	//Future races:
	//	Dathragol
	//	Elementals
	//	Gnomes
	//	Jinyi
	//	Naga
	//	Ogres
	//	Orcs

	//--BY CLASS--//
	public void sortByClass () {
		HeroButton [] sort = normalize ();
		HeroButton storage;
		//Sorting
		for (int m = 0; m < heroes.Length; m++) {
			for (int h = 0; h < heroes.Length; h++) {
				if (h + 1 < heroes.Length) {
					if (sort [h].role > sort [h + 1].role) {
						storage = sort [h];
						sort [h] = sort [h + 1];
						sort [h + 1] = storage;
					}
				}
			}
		}
		removeOldButtons ();
		//Making buttons
		for (int h = 0; h < heroes.Length; h++) {
			createButton (sort[h].hero, h);
		}
		setSize ();
	}

	//  TANK = 0  //
	//  BRAWLER = 1  //
	//  FIGHTER = 2  //
	//  MAGE = 3  //
	//  SUMMONER= 4  //
	//  SUPPORT = 5  //
	//  HEALER = 6  //
	public void sortSpecificClass (int role) {
		HeroButton [] sort = normalize ();
		HeroButton [] classes = new HeroButton [heroes.Length];
		int storage = 0;

		for (int h = 0; h < heroes.Length; h++) {
			if (sort [h].role == role) {
				classes [storage] = sort [h];
				storage++;
			}
		}

		removeOldButtons ();
		//Making buttons
		for (int h = 0; h < classes.Length; h++) {
			if (classes [h])
				createButton (classes [h].hero, h);
			else
				h = classes.Length;
		}
		setSize ();
	}

	//--BY ATTACK TYPE--//
	public void sortByAttack () {
		HeroButton [] sort = normalize ();
		HeroButton storage;
		for (int h = 0; h < heroes.Length; h++) {
			sort [h] = heroes [h];
		}
		//Sorting
		for (int m = 0; m < heroes.Length; m++) {
			for (int h = 0; h < heroes.Length; h++) {
				if (h + 1 < heroes.Length) {
					if (sort [h].attack > sort [h + 1].attack) {
						storage = sort [h];
						sort [h] = sort [h + 1];
						sort [h + 1] = storage;
					}
				}
			}
		}
		removeOldButtons ();
		//Making buttons
		for (int h = 0; h < heroes.Length; h++) {
			createButton (sort[h].hero, h);
		}
		setSize ();
	}

	//  MELEE = 0  //
	//  MAGIC = 1  //
	//  RANGED = 2  //
	public void sortSpecificAttack (int attack) {
		HeroButton [] sort = normalize ();
		HeroButton [] atk = new HeroButton [heroes.Length];
		int storage = 0;

		for (int h = 0; h < heroes.Length; h++) {
			if (sort [h].attack == attack) {
				atk [storage] = sort [h];
				storage++;
			}
		}

		removeOldButtons ();
		//Making buttons
		for (int h = 0; h < atk.Length; h++) {
			if (atk [h])
				createButton (atk [h].hero, h);
			else
				h = atk.Length;
		}
		setSize ();
	}

	//--BY LEVEL--//
	public void sortByLevel () {
		HeroButton [] sort = normalize ();
		HeroButton storage;
		for (int h = 0; h < heroes.Length; h++) {
			sort [h] = heroes [h];
		}
		//Sorting
		for (int m = 0; m < heroes.Length; m++) {
			for (int h = 0; h < heroes.Length; h++) {
				if (h + 1 < heroes.Length) {
					if (sort [h].level < sort [h + 1].level) {
						storage = sort [h];
						sort [h] = sort [h + 1];
						sort [h + 1] = storage;
					}
				}
			}
		}
		removeOldButtons ();
		//Making buttons
		for (int h = 0; h < heroes.Length; h++) {
			createButton (sort[h].hero, h);
		}
		setSize ();
	}

	private void removeOldButtons () {
		HeroButton [] oldButtons = GetComponentsInChildren <HeroButton> ();
		for (int h = 0; h < oldButtons.Length; h++) {
			oldButtons [h].removeButton ();
		}
	}


	public int state;
	public HeroButtonModels models;
	public void pickHero (int hero) {
		models.getHero (hero);
		heroName.text = heroes [hero - 1].characterName;
		for (int i = 0; i < 3; i++) {
			abilities [i].pickHero (hero);
		}
	}
}