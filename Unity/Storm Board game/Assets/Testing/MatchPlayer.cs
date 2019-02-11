using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchPlayer : MonoBehaviour {
	private int [] team = new int[5];
	public int side;
	public Image [] buttons;
	public Text nameInput;
	public string altName;
	public MatchMakingMaster master;
	public Player userPrefab;
	public HeroArchive database;

	public void setTeam(int[] t) {
		team = t;
		for (int i = 0; i < 5; i++) {
			buttons [i].sprite = database.heroes[team[i]].GetComponent<Piece>().Portrait;
		}
	}

	public void selection(int h) {
		master.goToSelectionView (h, side, team);
	}

	public void setUpPlayer() {
		Player user = Instantiate (userPrefab) as Player;
		user.setRandomHeroes ();
		user.setTeam (side);
		if (nameInput.text != "")
			user.username = nameInput.text;
		else
			user.username = altName;
		for (int i = 0; i < 5; i++) {
			user.changeHero (i, team[i]);
		}
		user.setRandomHeroes ();
	}
}
