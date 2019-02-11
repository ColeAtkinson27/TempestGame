using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HeroButtonsMaster : MonoBehaviour {
	public GameObject screenView;
	public GameObject menuView;
	public GameObject models;

	void Start () {
		SaveLoad.Load ();
	}

	public void accessHero (int hero) {
		screenView.SetActive (true);
		models.SetActive (true);
		menuView.SetActive (false);
		screenView.GetComponent<HeroButtonScreen> ().selectHero (hero);
	}

	public void leaveHero () {
		screenView.SetActive (false);
		menuView.SetActive (true);
		models.SetActive (false);
	}

	public void leaveArmory () {
		SceneManager.LoadScene ("Menu");
	}

}
