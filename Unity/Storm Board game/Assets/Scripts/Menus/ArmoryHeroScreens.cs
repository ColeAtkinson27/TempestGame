using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ArmoryHeroScreens : MonoBehaviour {

	public GameObject description;
	public GameObject abilities;
	public GameObject customize;
	public GameObject charImage;
	public GameObject [] forms;

	public Image talentIcon;
	public Text talentDescription;
	public Sprite [] tIcons;
	public string [] tDesc;
	public GameObject CharModel;
	public Text skinNameDisplay;
	public Material [] skins;
	public string [] skinNames;

	private Material[] materials;

	public void goToDescription () {
		charImage.SetActive (true);
		description.SetActive (true);
		abilities.SetActive (false);
		customize.SetActive (false);
		if (CharModel != null)
			CharModel.SetActive (false);
	}

	public void goToAbilities () {
		charImage.SetActive (true);
		description.SetActive (false);
		abilities.SetActive (true);
		customize.SetActive (false);
		if (CharModel != null)
			CharModel.SetActive (false);
	}

	public void goToCustomize () {
		charImage.SetActive (false);
		description.SetActive (false);
		abilities.SetActive (false);
		customize.SetActive (true);
		selectTalent (0);
		if (CharModel != null)
			CharModel.SetActive (true);
	}

	public void selectTalent (int talent) {
		talentIcon.sprite = tIcons [talent];
		talentDescription.text = tDesc [talent];
	}

	public void setSkin (int skin) {
		Renderer rend = CharModel.GetComponent <Renderer> ();
		materials = rend.materials;
		materials[0] = skins [skin];
		rend.materials = materials;
		skinNameDisplay.text = skinNames [skin];
	}

	public void selectAlternate (int form) {
		if (form < forms.Length) {
			if (forms [form] != null) {
				for (int newForm = 0; newForm < forms.Length; newForm++) {
					forms [newForm].SetActive (false);
				}
				forms [form].SetActive (true);
			}
		}
	}

	public void altFormCustomize () {
		for (int form = 0; form < forms.Length; form++) {
			forms [form].SetActive (false);
		}
		forms [0].SetActive (true);
		forms [0].GetComponent <ArmoryHeroScreens>().goToCustomize();
	}
}
