using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchHeroButton : MonoBehaviour {

	private MatchMakingSort handler;

	public string heroName;
	public string race;
	public int role;
	public int attack;
	public int level;
	public int hero;
	public string characterName;
	public Text levelText;
	public GameObject locked;

	void Start () {
		handler = GetComponentInParent<MatchMakingSort> ();
		Button self = GetComponent <Button> ();
		self.onClick.AddListener (delegate { taskOnClick (hero); });
		level = SaveLoad.player.level [hero];
		levelText.text = level.ToString();
		if (!SaveLoad.player.heroUnlocked [hero]) {
			self.interactable = false;
			locked.SetActive (true);
			levelText.text = "";
		}
	}

	public void removeButton () {
		GameObject.Destroy(gameObject);
	}

	void taskOnClick (int h) {
		handler.pickHero (h);
	}
}
