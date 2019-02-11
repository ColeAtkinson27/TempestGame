using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroInfo : MonoBehaviour {
	public int heroNumber;
	public string heroName;
	public string title;
	private int level;
	public string description;
	public string race;
	public string role;
	//Stats
	public int health;
	public int attack;
	public int actionPoints;
	public string attackType;
	//Abilities
	public Sprite [] abilityIcons;
	public string [] abilityNames;
	public string [] abilityDescriptions;
	public int [] abilityCooldowns;
	public int [] abilityRanges;
	public int [] abilityDamages;
	//Skins
	public Material [] skins;
	public Sprite [] skinColors;
	//Portraits
	public Sprite [] portraits;
	//Customization
	private int currentPortrait;
	private int currentSkin;

	public HeroInfo () {
		level = SaveLoad.player.level[heroNumber];
		currentPortrait = SaveLoad.player.heroPortrait[heroNumber];
		currentSkin = SaveLoad.player.heroSkins[heroNumber];
	}

	public int getLevel () {
		return level;
	}

	public void setPortrait (int p) {
		currentPortrait = p;
	}

	public int getPortrait () {
		return currentPortrait;
	}

	public void setSkin (int s) {
		currentSkin = s;
	}

	public int getSkin () {
		return currentSkin;
	}
}
