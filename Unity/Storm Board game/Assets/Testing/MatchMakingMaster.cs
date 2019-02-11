using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MatchMakingMaster : MonoBehaviour {
	public GameObject draftView;
	public GameObject selectionView;
	public GameObject customizeView;
	public MatchMakingSort sorter;
	public MatchPlayer player1;
	public MatchPlayer player2;

	private int currentPlayer;
	private int[] heroes = new int[6];

	void Start () {
		SaveLoad.Load ();
	}

	public void pickHero(int h) {

	}

	public void goToDraftView(bool accept) {
		draftView.SetActive (true);
		selectionView.SetActive (false);
		if (accept) {
			if (sorter.player == 1) {
				player1.setTeam (sorter.team);
			} else if (sorter.player == 2) {
				player2.setTeam (sorter.team);
			}
		}
	}

	public void goToSelectionView(int h, int t, int[] team) {
		draftView.SetActive (false);
		selectionView.SetActive (true);
		customizeView.SetActive (false);
		sorter.openMenu (h, t, team);
	}

	public void goToCustomizeView() {
		customizeView.SetActive (true);
		selectionView.SetActive (false);
	}

	public void leaveMatchMaking () {
		SceneManager.LoadScene ("Menu");
	}


	public void startGame() {
		player1.setUpPlayer ();
		player2.setUpPlayer ();
		SceneManager.LoadScene ("Game");
	}
}
