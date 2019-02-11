using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreScreen : MonoBehaviour {

	public Text winner;

	public Image [] bluePortraits;
	public Text blueName;
	public Text [] blueNames;
	public Text [] blueKills;
	public Text [] blueDeaths;
	public Text [] blueKDRatio;

	public Image [] redPortraits;
	public Text redName;
	public Text [] redNames;
	public Text [] redKills;
	public Text [] redDeaths;
	public Text [] redKDRatio;

	public Controller storage;

	public void endGame (Player winningPlayer) {
		winner.text = winningPlayer.username + " wins";
		blueName.text = storage.getPlayer (0).username;
		redName.text = storage.getPlayer (1).username;

		for (int i = 0; i < 5; i++) {
			bluePortraits [4 - i].sprite = storage.getBlueTeam (i).Portrait;
			blueNames [4 - i].text = storage.getBlueTeam (i).charName;
			blueKills [4 - i].text = (storage.getBlueTeam (i).showKills ()).ToString ();
			blueDeaths [4 - i].text = (storage.getBlueTeam (i).showDeaths ()).ToString ();
			blueKDRatio [4 - i].text = (storage.getBlueTeam (i).showKDRatio ()).ToString ();

			redPortraits [4 - i].sprite = storage.getRedTeam (i).Portrait;
			redNames [4 - i].text = storage.getRedTeam (i).charName;
			redKills [4 - i].text = (storage.getRedTeam (i).showKills ()).ToString ();
			redDeaths [4 - i].text = (storage.getRedTeam (i).showDeaths ()).ToString ();
			redKDRatio [4 - i].text = (storage.getRedTeam (i).showKDRatio ()).ToString ();
		}
	}
}
