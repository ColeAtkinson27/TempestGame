using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MatchUI : MonoBehaviour {

	public Image [] blueTeam;
	public Image [] redTeam;
	public Text redScore, blueScore;
	public Image active1, active2, passive;
	public GameObject active1Targetting, active2Targetting;
	public Text active1Cooldown, active2Cooldown;
	public Text playerAP, charAP;
	public Slider [] blueHealth;
	public Slider [] redHealth;
	public Text [] blueRespawn;
	public Text [] redRespawn;
	public Text currentTurn;
	private int currentPlayer;

	public Controller controller;
	private Piece selectedUnit;

	private Color32 dead, alive;
	public Color32 [] statuses;

	// Use this for initialization
	public void startUpMenuSystem () {
		dead = new Color32 (200, 50, 50, 255);
		alive = new Color32 (255, 255, 255, 255);
		for (int i = 0; i < 5; i++) {
			blueTeam [4 - i].sprite = controller.getBlueTeam (i).Portrait;
			redTeam [4 - i].sprite = controller.getRedTeam (i).Portrait;
		}
	}
	
	// Update is called once per frame
	void Update () {
		selectedUnit = controller.getSelectedUnit ();
		currentPlayer = controller.currentPlayerTurn;
		redScore.text = (controller.redScore).ToString ();
		blueScore.text = (controller.blueScore).ToString ();
		for (int n = 0; n < 5; n++) {
			if (controller.getBlueTeam (4 - n).checkIsDead () == true) {
				blueTeam [n].color = dead;
				blueRespawn [4 - n].text = controller.getBlueTeam (4 - n).getRespawn ().ToString ();
			} else {
				blueTeam [n].color = statuses [controller.getBlueTeam (4 - n).getStatusEffect ()];
				blueRespawn [n].text = null;
			}

			if (controller.getRedTeam (4 - n).checkIsDead () == true) {
				redTeam [n].color = dead;
				redRespawn [n].text = controller.getRedTeam (4 - n).getRespawn ().ToString ();
			} else {
				redTeam [n].color = statuses [controller.getRedTeam (4 - n).getStatusEffect ()];
				redRespawn [n].text = null;
			}
		}

		if (selectedUnit) {
			playerAP.text = (controller.getPlayer(currentPlayer).checkAP ()).ToString ();
			charAP.text = (selectedUnit.checkAP ()).ToString ();
			active1.sprite = selectedUnit.Active1;
			active2.sprite = selectedUnit.Active2;
			passive.sprite = selectedUnit.Passive;
			passive.GetComponent <Button> ().interactable = selectedUnit.useablePassive;
			currentTurn.text = controller.currentPlayersName ();
			for (int i = 0; i < 5; i++) {
				Piece bluePiece = controller.getBlueTeam (i);
				if (bluePiece.tempHealth == 0) {
					blueHealth [i].maxValue = bluePiece.getMaxHealth ();
					blueHealth [i].value = bluePiece.HP;
					blueHealth [i].fillRect.GetComponent<Image> ().color = new Color32 (0, 255, 0, 255);
					blueHealth [i].handleRect.GetComponent<Image> ().color = new Color32 (0, 255, 0, 255);
				} else {
					blueHealth [i].maxValue = bluePiece.topTempHealth;
					blueHealth [i].value = bluePiece.tempHealth;
					blueHealth [i].fillRect.GetComponent<Image> ().color = new Color32 (0, 255, 255, 255);
					blueHealth [i].handleRect.GetComponent<Image> ().color = new Color32 (0, 255, 255, 255);
				}
				Piece redPiece = controller.getRedTeam (i);
				if (redPiece.tempHealth == 0) {
					redHealth [4 - i].maxValue = redPiece.getMaxHealth ();
					redHealth [4 - i].value = redPiece.HP;
					redHealth [4 - i].fillRect.GetComponent<Image> ().color = new Color32 (0, 255, 0, 255);
					redHealth [4 - i].handleRect.GetComponent<Image> ().color = new Color32 (0, 255, 0, 255);
				} else {
					redHealth [4 - i].maxValue = redPiece.topTempHealth;
					redHealth [4 - i].value = redPiece.tempHealth;
					redHealth [4 - i].fillRect.GetComponent<Image> ().color = new Color32 (0, 255, 255, 255);
					redHealth [4 - i].handleRect.GetComponent<Image> ().color = new Color32 (0, 255, 255, 255);
				}
			}
			if (selectedUnit.getActive1Cooldown () != 0) {
				active1Cooldown.text = selectedUnit.getActive1Cooldown ().ToString();
				active1.GetComponent<Button> ().interactable = false;
			} else {
				active1Cooldown.text = "";
				active1.GetComponent<Button> ().interactable = true;
			}
			if (selectedUnit.getActive2Cooldown () != 0) {
				active2Cooldown.text = selectedUnit.getActive2Cooldown ().ToString();
				active2.GetComponent<Button> ().interactable = false;
			} else {
				active2Cooldown.text = "";
				active2.GetComponent<Button> ().interactable = true;
			}
			if (selectedUnit.checkState () == 1) {
				active1Targetting.SetActive (true);
				active2Targetting.SetActive (false);
			} else if (selectedUnit.checkState () == 2) {
				active1Targetting.SetActive (false);
				active2Targetting.SetActive (true);
			} else {
				active1Targetting.SetActive (false);
				active2Targetting.SetActive (false);
			}
		}
	}

	public void useActive1 () {
		if (selectedUnit.checkState () == 1) {
			selectedUnit.setAttackState (0);
			controller.searchTiles ();
		} else {
			selectedUnit.setAttackState (1);
			if (selectedUnit.getActive1Cooldown() == 0) {
				int target = 0;
				if (selectedUnit.team == 1) {
					target = 2;
				} else if (selectedUnit.team == 2) {
					target = 1;
				}
				controller.resetTiles ();
				controller.searchForTargets (target);
			}
		}
	}

	public void useActive2 () {
		if (selectedUnit.checkState () == 2) {
			selectedUnit.setAttackState (0);
			controller.searchTiles ();
		} else {
			selectedUnit.setAttackState (2);
			if (selectedUnit.getActive2Cooldown() == 0) {
				int target = 0;
				if (selectedUnit.team == 1) {
					target = 2;
				} else if (selectedUnit.team == 2) {
					target = 1;
				}
				controller.resetTiles();
				controller.searchForTargets (target);
			}
		}
	}

	public void usePassive () {
		selectedUnit.passiveAbility ();
	}
}
