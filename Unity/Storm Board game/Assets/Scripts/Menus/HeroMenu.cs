 using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroMenu : MonoBehaviour {

	public GameObject [] selectScreens;
	public GameObject [] heroScreens;
	public GameObject[] Sidebars;
	public GameObject selectionButtons;
	private int currentScreen = 0;
	private int currentSelectScreen = 0;

	public void accessScreen (int screen){
		heroScreens [currentScreen].SetActive (false);
		selectScreens [currentSelectScreen].SetActive (false);
		selectionButtons.SetActive (false);
		heroScreens [screen].SetActive (true);
		currentScreen = screen;
	}

	public void accessSelectScreen (int screen) {
		heroScreens [currentScreen].SetActive (false);
		selectScreens [currentSelectScreen].SetActive (false);
		selectionButtons.SetActive (true);
		selectScreens [screen].SetActive (true);
		currentSelectScreen = screen;
	}

	public void accessSideBar (int bar) {
		for (int b = 0; b < 3; b++) {
			Sidebars [b].SetActive (false);
		}
		Sidebars [bar].SetActive (true);
	}

	public void returnToArmory () {
		heroScreens [currentScreen].SetActive (false);
		heroScreens [currentScreen].GetComponent <ArmoryHeroScreens>().goToDescription();
		selectionButtons.SetActive (true);
	}
}
