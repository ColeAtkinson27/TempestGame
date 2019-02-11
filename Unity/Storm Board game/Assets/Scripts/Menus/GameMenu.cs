using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameMenu : MonoBehaviour {
	public GameObject Menu;
	public GameObject UI;
	public Text AllyScore;
	public Text EnemyScore;
	public Text PlayerAP;
	public Text UnitAP;
	public Image[] AllyPortraits;
	public Image[] EnemyPortraits;
	public Image[] Abilities;
	public Controller board;

	void Start () {
		
	}

	// Use this for initialization
	public void OpenMenu () {
		Menu.SetActive (true);
		UI.SetActive (false);
	}

	public void EscapeToMainMenu () {
		SceneManager.LoadScene ("Menu");
		board.endGame ();
	}

	public void ReturnToGame () {
		Menu.SetActive (false);
		UI.SetActive (true);
	}


	//UI
	void Update () {

	}
}
