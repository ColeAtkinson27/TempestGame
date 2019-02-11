using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchScreen : MonoBehaviour {

	public HeroArchive database;
	public Player [] players;
	public Text [] playerNames;
	public Image [] player1Heroes;
	public Image [] player2Heroes;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		updatePortraits ();
	}

	public void updatePortraits () {
		if (players [0] && players [1]) {
			playerNames [0].text = players [0].username;
			playerNames [1].text = players [1].username;
			for (int n = 0; n < 5; n++) {
				player1Heroes [n].sprite = database.heroes [players [0].giveHeroNumber (n)].GetComponent <Piece> ().Portrait;
				player2Heroes [n].sprite = database.heroes [players [1].giveHeroNumber (n)].GetComponent <Piece> ().Portrait;
			}
		}
	}
}
