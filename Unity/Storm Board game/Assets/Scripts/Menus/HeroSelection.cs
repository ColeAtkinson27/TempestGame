using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroSelection : MonoBehaviour {
	public Menu menu;

	private int currentHero;
	public int [] heroes;
	public Image [] heroPortraits;
	public Sprite [] portraits;
	private int playerUser;

	private int currentScreen;
	private int currentSearch;
	public GameObject [] Screens;
	public GameObject [] Races;
	public GameObject [] Classes;
	public GameObject [] ATKType;

	public void enterMenu (int player, int hero1, int hero2, int hero3, int hero4, int hero5) {
		playerUser = player;
		heroes [0] = hero1;
		heroPortraits [0].sprite = portraits [hero1];
		heroes [1] = hero2;
		heroPortraits [1].sprite = portraits [hero2];
		heroes [2] = hero3;
		heroPortraits [2].sprite = portraits [hero3];
		heroes [3] = hero4;
		heroPortraits [3].sprite = portraits [hero4];
		heroes [4] = hero5;
		heroPortraits [4].sprite = portraits [hero5];
	}

	public void selectHero (int hero) {
		int notUsed = 0;
		int heroNumberStorage = 0;
		int storage = heroes [currentHero];
		if (hero != 0 && storage != 0) {
			for (int n = 0; n < 5; n++) {
				if (hero == heroes [n]) {
					notUsed = 1;
					heroNumberStorage = n;
					n = 5;
				}
			}
		}
		heroes [currentHero] = hero;
		heroPortraits [currentHero].sprite = portraits [hero];
		if (notUsed == 1) {
			heroes [heroNumberStorage] = storage;
			heroPortraits [heroNumberStorage].sprite = portraits [storage];
		}
	}

	public void chooseHero (int hero) {
		currentHero = hero;
	}

	public void switchSearch (int type) {
		narrowSearch (0);
		Screens [currentScreen].SetActive (false);
		Screens [type].SetActive (true);
		currentScreen = type;
	}

	public void narrowSearch (int type) {
		if (currentScreen == 1) {
			Races [currentSearch].SetActive (false);
			Races [type].SetActive (true);
			currentSearch = type;
		} else if (currentScreen == 2) {
			Classes [currentSearch].SetActive (false);
			Classes [type].SetActive (true);
			currentSearch = type;
		} else if (currentScreen == 3) {
			ATKType [currentSearch].SetActive (false);
			ATKType [type].SetActive (true);
			currentSearch = type;
		}
	}
	public void exitMenu (bool accept) {
		menu.setTeam (playerUser, accept, heroes [0], heroes [1], heroes [2], heroes [3], heroes [4]);
	}
}
