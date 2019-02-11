using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class HeroButtonAbility: MonoBehaviour, IPointerEnterHandler, IPointerExitHandler{

	public GameObject text;

	public Sprite [] abilityImages;
	public string [] abilityDescriptions;

	void Start () {
		text.SetActive (false);
	}

	public void pickHero (int h) {
		this.GetComponent <Image> ().sprite = abilityImages [h];
		text.GetComponentInChildren <Text> ().text = abilityDescriptions [h];
	}

	public void OnPointerEnter (UnityEngine.EventSystems.PointerEventData eventData) {
		text.SetActive (true);
	}

	public void OnPointerExit (UnityEngine.EventSystems.PointerEventData eventData) {
		text.SetActive (false);
	}
}
