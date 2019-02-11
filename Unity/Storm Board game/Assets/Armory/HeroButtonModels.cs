using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroButtonModels : MonoBehaviour {

	public HeroButtonsSort manager;
	public GameObject [] models;
	private int currentHero = 0;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (currentHero != manager.state) {
			getHero (manager.state);
		}
	}

	public void getHero (int hero) {
		if (hero != 0) {
			if (models [currentHero] != null) {
				models [currentHero].SetActive (false);
			}

			if (models [hero] != null) {
				models [hero].SetActive (true);
			}

			currentHero = hero;
		}
	}

	public void setSkin (int hero, Material skin) {
		if (models [hero]) {
			Renderer rend = models [hero].GetComponentInChildren <Renderer> ();
			Material[] materials = rend.materials;
			materials [0] = skin;
			rend.materials = materials;
		}
	}
}
