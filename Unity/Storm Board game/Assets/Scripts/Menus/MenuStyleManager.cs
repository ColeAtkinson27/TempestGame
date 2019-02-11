using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuStyleManager : MonoBehaviour {

	/*public Image [] HomeNavButtons;
	public Image [] BackButtons;
	public Image [] PlayMenuButtons;
	public Image HomeNamePlate;
	public Image ChangelogBackground;
	public Image ProfileNameBar;
	public Image ProfileAvatarBackground;
	public Image ProfileStyles;

	public Image [] ArmoryNameplates;
	public Image [] ArmoryButtons;
	public Image [] ArmoryPageNames;
	public Image [] ArmoryInfoBackgrounds;
	public Image [] ArmoryCharDescriptionBackgrounds;
	public Image [] ArmoryCharStatBackgrounds;
	public Image [] ArmoryAbilityBackgrounds;

	public Color32 [] Styles;

	//public Sprite [] ClothingPictures;*/

	// Use this for initialization
	void Start () {
		//SetStyle (PlayerPrefs.GetInt ("MenuStyle"));
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void StyleSelection (int style) {
		PlayerPrefs.SetInt ("MenuStyle", style);
		PlayerPrefs.Save ();
		//SetStyle (style);
	}

	/*public void SetStyle (int style) {
		for (int n = 0; n < 6; n++) {
			HomeNavButtons [n].color = Styles [style];
		}
		for (int n = 0; n < 6; n++) {
			BackButtons [n].color = Styles [style];
		}
		for (int n = 0; n < 2; n++) {
			PlayMenuButtons [n].color = Styles [style];
		}
		HomeNamePlate.color = Styles [style];
		Color CLB =  Styles [style];
		CLB.a = 20;
		ChangelogBackground.color = CLB;
		ProfileNameBar.color = Styles [style];
		ProfileAvatarBackground.color = Styles [style];
		ProfileStyles.color = Styles [style];
		for (int n = 0; n < 3; n++) {
			ArmoryNameplates [n].color = Styles [style];
		}
		for (int n = 0; n < 49; n++) {
			ArmoryButtons [n].color = Styles [style];
		}
		for (int n = 0; n < 49; n++) {
			ArmoryPageNames [n].color = Styles [style];
		}
		for (int n = 0; n < 10; n++) {
			ArmoryInfoBackgrounds [n].color = Styles [style];
		}
		for (int n = 0; n < 39; n++) {
			ArmoryCharDescriptionBackgrounds [n].color = Styles [style];
		}
		for (int n = 0; n < 39; n++) {
			ArmoryCharStatBackgrounds [n].color = Styles [style];
		}
		for (int n = 0; n < 117; n++) {
			ArmoryAbilityBackgrounds [n].color = Styles [style];
		}
	}

	/*public Sprite getClothes (int sex, int clothing, int color) {
		return ClothingPictures [(sex * 36) + (clothing * 6) + color];
	}*/
}
