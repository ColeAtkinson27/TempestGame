using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroButtonScreen : MonoBehaviour {
	public HeroInfo [] heroInfo;
	public HeroButtonModels models;
	private int currentHero;

	public Image portrait;
	public Text heroName;
	public Text title;
	public Text level;
	public Text raceAndRole;
	public Text description;
	public Text hp;
	public Text damage;
	public Text ap;
	public Text attackType;

	public Text [] abilityNames;
	public Text [] abilityCooldowns;
	public Text [] abilityDamage;
	public Text [] abilityRange;
	public Text [] abilityDescriptions;
	public Image [] abilityIcons;

	public Image [] portraitButtons;
	public Image [] skinButtons;

	public GameObject currentSkinLocked;
	public GameObject currentPortraitLocked;
	public GameObject heroSkinLocked;
	public GameObject heroPortraitLocked;
	public GameObject[] lockedSkins;
	public GameObject[] lockedPortraits;

	public Slider experienceBar;

	// Use this for initialization
	void Start () {
		
	}
	
	void Update () {
		
	}

	public void selectHero (int h) {
		currentHero = h;
		HeroInfo hero = heroInfo [h];

		heroName.text = hero.heroName;
		title.text = hero.title;
		level.text = "Lv. " + SaveLoad.player.level [h];
		raceAndRole.text = hero.race + " " + hero.role;
		description.text = hero.description;
		hp.text = "HP: " + hero.health;
		damage.text = "Damage: " + hero.attack;
		ap.text = "AP: " + hero.actionPoints;
		attackType.text = "Attack Type: " + hero.attackType;

		models.getHero (h);
		for (int i = 0; i < 5; i++) {
			portraitButtons [i].sprite = hero.portraits [i];
			skinButtons [i].sprite = hero.skinColors [i];
		}
		for (int i = 0; i < 3; i++) {
			abilityNames [i].text = heroInfo [currentHero].abilityNames [i];
			abilityIcons [i].sprite = heroInfo [currentHero].abilityIcons [i];
			abilityDescriptions [i].text = heroInfo [currentHero].abilityDescriptions [i];
			if (i < 2) {
				abilityCooldowns [i].text = "Cooldown: " + heroInfo [currentHero].abilityCooldowns [i] + " turns";
				abilityRange [i].text = "Range: " + heroInfo [currentHero].abilityRanges [i] + " tiles";
				abilityDamage [i].text = "Damage: " + heroInfo [currentHero].abilityDamages [i];
			}
		}

		models.setSkin (h, heroInfo [h].skins [SaveLoad.player.heroSkins[h]]);
		portrait.sprite =  heroInfo [h].portraits [SaveLoad.player.heroPortrait[h]];

		if (SaveLoad.player.heroUnlocked [h] == true) {
			currentPortraitLocked.SetActive (false);
			currentSkinLocked.SetActive (false);
			heroPortraitLocked.SetActive (false);
			heroSkinLocked.SetActive (false);
		} else {
			currentPortraitLocked.SetActive (true);
			currentSkinLocked.SetActive (true);
			heroPortraitLocked.SetActive (true);
			heroSkinLocked.SetActive (true);
		}
		for (int i = 0; i < 4; i++) {
			if (SaveLoad.player.skinsUnlocked [h, i + 1] == false) {
				lockedSkins [i].SetActive (true);
			} else {
				lockedSkins [i].SetActive (false);
			}
			if (SaveLoad.player.portraitsUnlocked [h, i + 1] == false) {
				lockedPortraits [i].SetActive (true);
			} else {
				lockedPortraits [i].SetActive (false);
			}
		}
		experienceBar.maxValue = SaveLoad.expCaps[SaveLoad.player.level[h]];
		experienceBar.value = SaveLoad.player.experience [h];
	}

	public int getCurrentHero () {
		return currentHero;
	}

	public void setHeroSkin (int skinNumber) {
		Material skin = heroInfo [currentHero].skins [skinNumber];
		models.setSkin (currentHero, skin);
		if (SaveLoad.player.skinsUnlocked [currentHero, skinNumber] == true) {
			currentSkinLocked.SetActive(false);
			SaveLoad.player.heroSkins [currentHero] = skinNumber;
			SaveLoad.Save ();
		} else {
			currentSkinLocked.SetActive(true);
		}
	}

	public void setHeroPortrait (int portraitNumber) {
		portrait.sprite = heroInfo [currentHero].portraits [portraitNumber];
		if (SaveLoad.player.portraitsUnlocked [currentHero, portraitNumber] == true) {
			currentPortraitLocked.SetActive(false);
			SaveLoad.player.heroPortrait [currentHero] = portraitNumber;
			SaveLoad.Save ();
		} else {
			currentPortraitLocked.SetActive(true);
		}
	}

	public void cycleHero (int direction) {
		if (currentHero + direction >= heroInfo.Length) {
			selectHero (1);
		} else if (currentHero + direction == 0) {
			selectHero (heroInfo.Length - 1);
		} else {
			selectHero (currentHero + direction);
		}
	}
}
