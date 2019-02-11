using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopButton : MonoBehaviour {

	public int hero;
	public GameObject check;

	void Start () {
		if (SaveLoad.player.heroUnlocked [hero]) {
			this.GetComponent<Button> ().interactable = false;
			if (check)
				check.SetActive (true);
		}
	}

	public void unlock () {
		SaveLoad.player.heroUnlocked [hero] = true;
		SaveLoad.Save ();
		this.GetComponent<Button> ().interactable = false;
		if (check)
			check.SetActive (true);
	}
}
