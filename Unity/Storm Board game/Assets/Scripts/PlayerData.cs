using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlayerData {
	public static PlayerData current;

	private static int heroes = 36;

	//==HERO UNLOCKS==//
	public bool [] heroUnlocked = new bool[heroes];
	public bool [,] portraitsUnlocked = new bool[heroes, 5];
	public bool [,] skinsUnlocked = new bool[heroes, 5];

	//==LEVEL==//
	public int [] level = new int [heroes];
	public int [] experience = new int [heroes];
	//==CUSTOMIZATION==//
	public int [] team = new int[5];
	public int [] heroPortrait = new int[heroes];
	public int [] heroSkins = new int[heroes];

	public PlayerData () {
		for (int h = 0; h < heroes; h++) {
			this.portraitsUnlocked [h, 0] = true;
			this.skinsUnlocked [h, 0] = true;
			for (int i = 1; i < 5; i++) {
				this.portraitsUnlocked [h, i] = false;
				this.skinsUnlocked [h, i] = false;
			}
			this.level [h] = 1;
			this.experience [h] = 0;
			this.heroPortrait [h] = 0;
			this.heroSkins [h] = 0;
		}
		heroUnlocked [0] = true;
	}
}
