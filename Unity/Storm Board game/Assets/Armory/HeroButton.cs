using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HeroButton : MonoBehaviour {

	private HeroButtonsMaster handler;

	public string heroName;
	public string race;
	public int role;
	public int attack;
	public int level;
	public int hero;
	public string characterName;

	void Start () {
		handler = GetComponentInParent<HeroButtonsMaster> ();
		Button self = GetComponent <Button> ();
		self.onClick.AddListener (delegate { taskOnClick (hero); });
	}

	public void removeButton () {
		GameObject.Destroy(gameObject);
	}

	void taskOnClick (int h) {
		handler.accessHero (h + 1);
	}
}
