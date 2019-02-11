using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controller : MonoBehaviour {

	//Tile positions
	public Circle[,] boardTiles = new Circle [19, 9];
	//Hero positions
	public Piece [,] pieces = new Piece[19, 9];
	//Placement of water and pedastals
	private int[,] terrain = new int [19,9];

	public GameObject [] redTeamPrefabs;
	public GameObject [] blueTeamPrefabs;
	public GameObject tilePrefab;
	public CameraView CameraManager;

	private Camera CurrentCamera;
	private Piece selectedUnit;

	private Piece [] blueTeam = new Piece [5];
	private Piece [] redTeam = new Piece [5];
	public double redScore, blueScore;
	public int currentPlayerTurn;
	public MatchUI UICanvas;

	private Player [] players = new Player[2];
	public HeroArchive database;
	public int setAP;
	private int maxPoints = 8;

	public GameObject gameUI;
	public GameObject endGameScreen;

	private void Start () {
		SaveLoad.Load ();
		currentPlayerTurn = 0;
		GenerateBoard ();
		SetBoardTerrain ();
		redScore = 0;
		blueScore = 0;
	}

	private void Update () {
		UICanvas.startUpMenuSystem ();
		MouseUpdate ();
		checkForWin ();
	}

	public void endTurn () {
		if (currentPlayerTurn == 0) {
			players [0].startTurn (setAP);
			for (int h = 0; h < 5; h++) {
				redTeam [h].startTurn ();
				blueTeam [h].endTurn ();
			}
			currentPlayerTurn = 1;
		} else if (currentPlayerTurn == 1) {
			players [1].startTurn (setAP);
			for (int h = 0; h < 5; h++) {
				blueTeam [h].startTurn ();
				redTeam [h].endTurn ();
			}
			currentPlayerTurn = 0;
		}
		if (selectedUnit) {
			searchTiles ();
		}
	}

	public Piece getSelectedUnit () {
		return selectedUnit;
	}

	private void MouseUpdate () {
		if (Input.GetMouseButtonDown (0)) {
			CurrentCamera = CameraManager.currentCamera ();
			RaycastHit hit;
			Ray ray = CurrentCamera.ScreenPointToRay (Input.mousePosition);
			if (Physics.Raycast (ray, out hit)) {
				GameObject hitObject = hit.collider.gameObject;
				if (hitObject.GetComponent<Piece> () != null) {
					if (boardTiles [hitObject.GetComponent<Piece>().getX (), hitObject.GetComponent<Piece>().getY ()].getState() == 5) {
						attackTarget (hitObject.GetComponent<Piece>().getX (), hitObject.GetComponent<Piece>().getY ());
					} else {
						if (selectedUnit) {
							selectedUnit.setAttackState (0);
							resetTiles ();
							boardTiles [selectedUnit.getX (), selectedUnit.getY ()].setState (selectedUnit.team);
						}
						selectedUnit = hitObject.GetComponent<Piece>();
						boardTiles [selectedUnit.getX (), selectedUnit.getY ()].setState (3);
						if (currentPlayerTurn == selectedUnit.team - 1) {
							searchTiles ();
						}
					}
				} else if (hitObject.GetComponent<Circle> ()) {
					if (hitObject.GetComponent<Circle> ().getState() == 4){
						resetTiles ();
						setBoardTile (selectedUnit.getX (), selectedUnit.getY (), 0);
						selectedUnit.moveCharacter (hitObject.GetComponent<Circle> ());
						selectedUnit.useAP ();
						players [currentPlayerTurn].takeAction ();
						resetTiles ();
						searchTiles ();
						setBoardTile (selectedUnit.getX (), selectedUnit.getY (), 3);
					} else if (hitObject.GetComponent<Circle> ().getState() == 1) {
						if (selectedUnit) {
							resetTiles ();
							boardTiles [selectedUnit.getX (), selectedUnit.getY ()].setState (selectedUnit.team);
						}
						Circle selectedTile = hitObject.GetComponent <Circle> ();
						selectedUnit = pieces [selectedTile.x, selectedTile.y];
						boardTiles [selectedUnit.getX (), selectedUnit.getY ()].setState (3);
						resetTiles ();
						searchTiles ();
					} else if (hitObject.GetComponent<Circle> ().getState() == 2) {
						if (selectedUnit) {
							resetTiles ();
							boardTiles [selectedUnit.getX (), selectedUnit.getY ()].setState (selectedUnit.team);
						}
						Circle selectedTile = hitObject.GetComponent <Circle> ();
						selectedUnit = pieces [selectedTile.x, selectedTile.y];
						boardTiles [selectedUnit.getX (), selectedUnit.getY ()].setState (3);
						resetTiles ();
						searchTiles ();
					} else if (hitObject.GetComponent <Circle> ().getState () == 5) {
						Circle selectedTile = hitObject.GetComponent <Circle> ();
						attackTarget (selectedTile.x, selectedTile.y);
					}
				}
			}
		}
	}

	private void GenerateBoard () {
		//Tiles
		for (int y = 0; y < 9; y++) {
			for (int x = 0; x < 19; x++) {
				GenerateTile (x, y);
			}
		}
		for (int boardX = 0; boardX < 19; boardX += 18) {
			for (int boardY = 0; boardY < 9; boardY += 2) {
				Circle c = boardTiles [boardX, boardY];
				c.transform.Translate (Vector3.up * 7f);
				if (boardX == 0) {
					c.setState (1);
				} else if (boardX == 18) {
					c.setState (2);
				}
			}
		}
	}

	//==SET UP GAME==//
	public void setPlayer (Player player, int team) {
		players [team] = player;
		if (team == 1) {
			//red team
			for (int y = 0; y < 9; y += 2) {
				GenerateRedPiece (1, y);
			}
		} else if (team == 0) {
			//blue team
			for (int y = 0; y < 9; y += 2) {
				GenerateBluePiece (1, y);
			}
		}
	}
	//Team Generation
	public void GenerateRedPiece (int x, int y) {
		GameObject go = Instantiate (database.heroes [players[1].giveHeroNumber (y / 2)]) as GameObject;
		go.transform.SetParent (transform);
		players [1].setHero ((y / 2), go);
		Piece p = go.GetComponent <Piece> ();
		p.grid = GetComponentInParent <Controller> ();
		p.team = 2;
		pieces [18, y] = p;
		p.setPosition (18, y);
		p.setSpawn (18, y);
		redTeam [y / 2] = p;
		p.transform.localRotation *= Quaternion.Euler (0, 0, 90);
		SetRedPiece (p, x, y);
	}

	public void GenerateBluePiece (int x, int y) {
		GameObject go = Instantiate (database.heroes [players[0].giveHeroNumber (y / 2)]) as GameObject;
		go.transform.SetParent (transform);
		players [0].setHero ((y / 2), go);
		Piece p = go.GetComponent <Piece> ();
		p.grid = GetComponentInParent <Controller> ();
		p.team = 1;
		p.endTurn ();
		pieces [0, y] = p;
		p.setPosition (0, y);
		p.setSpawn (0, y);
		blueTeam [y / 2] = p;
		p.transform.localRotation *= Quaternion.Euler (0, 0, -90);
		SetBluePiece (p, x, y);
	}

	private void SetRedPiece (Piece p, int x, int y) {
		p.transform.position = (Vector3.right * x * 40 * 9) + (Vector3.forward * y * 40) + (Vector3.back * 40 * 4) + (Vector3.up * 55f);
	}

	private void SetBluePiece (Piece p, int x, int y) {
		p.transform.position = (Vector3.right * x * 40 * -9) + (Vector3.forward * y * 40) + (Vector3.back * 40 * 4) + (Vector3.up * 55f);
	}

	//Tile Generation
	private void GenerateTile (int x, int y) {
		GameObject go = Instantiate (tilePrefab) as GameObject;
		go.transform.SetParent (transform);
		Circle c = go.GetComponent <Circle> ();
		boardTiles [x, y] = c;
		c.transform.position = (Vector3.right * x * 40) + (Vector3.forward * y * 40) + (Vector3.back * 40 * 4) + (Vector3.left * 40 * 9) + (Vector3.up * 32f);
		c.x = x;
		c.y = y;
	}

	private void SetBoardTerrain () {
		Board Terrain = this.GetComponent <Board> ();
		for (int row = 0; row < 19; row++) {
			for (int column = 0; column < 9; column++) {
				terrain [row, column] = Terrain.getTerrain (row, column);
			}
		}
	}

	//==CHECK STATUSES==//
	public Circle checkBoardTiles (int x, int y) {
		return boardTiles [x,y];
	}

	public Piece checkHeroPositions (int x, int y) {
		return pieces [x,y];
	}

	public int checkBoardTerrain (int x, int y) {
		return terrain [x,y];
	}

	//==SET STATUSES==//
	public void setBoardTile (int x, int y, int status) {
		boardTiles [x, y].setState (status);
	}

	public void setHeroPositions (int x, int y, Piece hero) {
		pieces [x, y] = hero;
	}

	//==MANAGE TILES==//
	//0 = Inactive
	//1 = Player 1
	//2 = Player 2
	//3 = Selected
	//4 = Green
	//5 = Red

	//==MANAGE HEROES==//
	public Piece getBlueTeam (int unit) {
		return blueTeam [unit];
	}

	public Piece getRedTeam (int unit) {
		return redTeam [unit];
	}

	public void searchTiles (){
		resetTiles ();
		if (selectedUnit.team == players [currentPlayerTurn].checkTeam ()) {
			if (selectedUnit.checkAP () != 0 && players [currentPlayerTurn].checkAP () != 0) {
				if (!selectedUnit.checkIsDead ()) {
					if (selectedUnit.getStatusEffect () == 0 || selectedUnit.getStatusEffect () == 1) {
						int x = selectedUnit.getX ();
						int y = selectedUnit.getY ();
						if ((x + 1) <= 18) {
							if (terrain [x + 1, y] == 0) {
								if (boardTiles [x + 1, y].getState () == 0) {
									setBoardTile (x + 1, y, 4);
								}
							} else if (selectedUnit.canWaterWalk == true && terrain [x + 1, y] == 1) {
								setBoardTile (x + 1, y, 4);
							}
						}
						if ((x - 1) >= 0) {
							if (terrain [x - 1, y] == 0) {
								if (boardTiles [x - 1, y].getState () == 0) {
									setBoardTile (x - 1, y, 4);
								}
							} else if (selectedUnit.canWaterWalk == true && terrain [x - 1, y] == 1) {
								setBoardTile (x - 1, y, 4);
							}
						}
						if ((y + 1) <= 8) {
							if (terrain [x, y + 1] == 0) {
								if (boardTiles [x, y + 1].getState () == 0) {
									setBoardTile (x, y + 1, 4);
								}
							} else if (selectedUnit.canWaterWalk == true && terrain [x, y + 1] == 1) {
								setBoardTile (x, y + 1, 4);
							}
						}
						if ((y - 1) >= 0) {
							if (terrain [x, y - 1] == 0) {
								if (boardTiles [x, y - 1].getState () == 0) {
									setBoardTile (x, y - 1, 4);
								}
							} else if (selectedUnit.canWaterWalk == true && terrain [x, y - 1] == 1) {
								setBoardTile (x, y - 1, 4);
							}
						}
					}
					if (selectedUnit.checkCanAttack () == true) {
						int target;
						if (selectedUnit.team == 1)
							target = 2;
						else
							target = 1;
						searchForTargets (target);
					}
				}
			}
		} else {
			resetTiles ();
		}
	}
		
	public void checkTargets (int target) {
		int range = 0;
		if (selectedUnit.checkState () == 0) {
			range = selectedUnit.getRange ();
		}
		//Quadrant 1
		for (int xRange = 0; xRange <= range; xRange++) {
			for (int yRange = 0; yRange <= range; yRange++) {
				if (yRange + xRange <= range) {
					if (selectedUnit.getX () + xRange < 19) {
						if (selectedUnit.getY () + yRange < 9) {
							int xTile = selectedUnit.getX () + xRange;
							int yTile = selectedUnit.getY () + yRange;
							if (terrain [xTile, yTile] != 2) {
								if (boardTiles [xTile, yTile].getState () == target && pieces [xTile, yTile].checkIsDead () == false) {
									setBoardTile (xTile, yTile, 5);
								}
							}
						}
					}
				}
			}
		}
		//Quadrant 2
		for (int xRange = 0; xRange < range; xRange++) {
			for (int yRange = 0; yRange < range; yRange++) {
				if (yRange + xRange <= range) {
					if (selectedUnit.getX () - xRange > 0) {
						if (selectedUnit.getY () + yRange < 9) {
							int xTile = selectedUnit.getX () - xRange;
							int yTile = selectedUnit.getY () + yRange;
							if (terrain [xTile, yTile] != 2) {
								if (boardTiles [xTile, yTile].getState () == target && pieces [xTile, yTile].checkIsDead () == false) {
									setBoardTile (xTile, yTile, 5);
								}
							}
						}
					}
				}
			}
		}
		//Quadrant 3
		for (int xRange = 0; xRange <= range; xRange++) {
			for (int yRange = 0; yRange <= range; yRange++) {
				if (yRange + xRange <= range) {
					if (selectedUnit.getX () - xRange >= 0) {
						if (selectedUnit.getY () - yRange >= 0) {
							int xTile = selectedUnit.getX () - xRange;
							int yTile = selectedUnit.getY () - yRange;
							if (terrain [xTile, yTile] != 2) {
								if (boardTiles [xTile, yTile].getState () == target && pieces [xTile, yTile].checkIsDead () == false) {
									setBoardTile (xTile, yTile, 5);
								}
							}
						}
					}
				}
			}
		}
		//Quadrant 4
		for (int xRange = 0; xRange < range; xRange++) {
			for (int yRange = 0; yRange < range; yRange++) {
				if (yRange + xRange <= range) {
					if (selectedUnit.getX () + xRange < 19) {
						if (selectedUnit.getY () - yRange >= 0) {
							int xTile = selectedUnit.getX () + xRange;
							int yTile = selectedUnit.getY () - yRange;
							if (terrain [xTile, yTile] != 2) {
								if (boardTiles [xTile, yTile].getState () == target && pieces [xTile, yTile].checkIsDead () == false) {
									setBoardTile (xTile, yTile, 5);
								}
							}
						}
					}
				}
			}
		}
	}

	public void searchForTargets (int target) {
		if (selectedUnit.getStatusEffect () != 3 && selectedUnit.getStatusEffect () != 4) {
			if (selectedUnit.checkState () == 0) {
				checkTargets (target);
			} else if (selectedUnit.checkState () == 1) {
				selectedUnit.active1Search (target);
			} else if (selectedUnit.checkState () == 2) {
				selectedUnit.active2Search (target);
			}
		}
	}

	public void radialTargetting (int target, int range) {
		//Quadrant 1
		for (int xRange = 0; xRange <= range; xRange++) {
			for (int yRange = 0; yRange <= range; yRange++) {
				if (yRange + xRange <= range) {
					if (selectedUnit.getX () + xRange < 19) {
						if (selectedUnit.getY () + yRange < 9) {
							int xTile = selectedUnit.getX () + xRange;
							int yTile = selectedUnit.getY () + yRange;
							if (terrain [xTile, yTile] != 2) {
								if (boardTiles [xTile, yTile].getState () == target && pieces [xTile, yTile].checkIsDead () == false) {
									boardTiles [xTile, yTile].setState (5);
								} else {
									boardTiles [xTile, yTile].setState (6);
								}
							}
						}
					}
				}
			}
		}
		//Quadrant 2
		for (int xRange = 1; xRange < range; xRange++) {
			for (int yRange = 1; yRange < range; yRange++) {
				if (yRange + xRange <= range) {
					if (selectedUnit.getX () - xRange > 0) {
						if (selectedUnit.getY () + yRange < 9) {
							int xTile = selectedUnit.getX () - xRange;
							int yTile = selectedUnit.getY () + yRange;
							if (terrain [xTile, yTile] != 2) {
								if (boardTiles [xTile, yTile].getState () == target && pieces [xTile, yTile].checkIsDead () == false) {
									boardTiles [xTile, yTile].setState (5);
								} else {
									boardTiles [xTile, yTile].setState (6);
								}
							}
						}
					}
				}
			}
		}
		//Quadrant 3
		for (int xRange = 0; xRange <= range; xRange++) {
			for (int yRange = 0; yRange <= range; yRange++) {
				if (yRange + xRange <= range) {
					if (selectedUnit.getX () - xRange >= 0) {
						if (selectedUnit.getY () - yRange >= 0) {
							int xTile = selectedUnit.getX () - xRange;
							int yTile = selectedUnit.getY () - yRange;
							if (terrain [xTile, yTile] != 2) {
								if (boardTiles [xTile, yTile].getState () == target && pieces [xTile, yTile].checkIsDead () == false) {
									boardTiles [xTile, yTile].setState (5);
								} else {
									boardTiles [xTile, yTile].setState (6);
								}
							}
						}
					}
				}
			}
		}
		//Quadrant 4
		for (int xRange = 1; xRange < range; xRange++) {
			for (int yRange = 1; yRange < range; yRange++) {
				if (yRange + xRange <= range) {
					if (selectedUnit.getX () + xRange < 19) {
						if (selectedUnit.getY () - yRange >= 0) {
							int xTile = selectedUnit.getX () + xRange;
							int yTile = selectedUnit.getY () - yRange;
							if (terrain [xTile, yTile] != 2) {
								if (boardTiles [xTile, yTile].getState () == target && pieces [xTile, yTile].checkIsDead () == false) {
									boardTiles [xTile, yTile].setState (5);
								} else {
									boardTiles [xTile, yTile].setState (6);
								}
							}
						}
					}
				}
			}
		}
		boardTiles [selectedUnit.getX (), selectedUnit.getY ()].setState (3);
	}

	public void directionalTargetting (int target, int range) {
		for (int xRange = -range; xRange <= range; xRange++) {
			if (xRange != 0) {
				if ((selectedUnit.getX () + xRange) < 18 && (selectedUnit.getX () + xRange) >= 0) {
					int xTile = selectedUnit.getX () + xRange;
					if (terrain [xTile, selectedUnit.getY ()] != 2) {
						if (boardTiles [xTile, selectedUnit.getY ()].getState () == target && pieces [xTile, selectedUnit.getY ()].checkIsDead () == false) {
							boardTiles [xTile, selectedUnit.getY ()].setState (5);
						} else {
							boardTiles [xTile, selectedUnit.getY ()].setState (6);
						}
					}
				}
			}
		}
		for (int yRange = -range; yRange <= range; yRange++) {
			if (yRange != 0) {
				if ((selectedUnit.getY () + yRange) < 9 && (selectedUnit.getY () + yRange) >= 0) {
					int yTile = selectedUnit.getY () + yRange;
					if (terrain [selectedUnit.getX (), yTile] != 2) {
						if (boardTiles [selectedUnit.getX (), yTile].getState () == target && pieces [selectedUnit.getX (), yTile].checkIsDead () == false) {
							boardTiles [selectedUnit.getX (), yTile].setState (5);
						} else {
							boardTiles [selectedUnit.getX (), yTile].setState (6);
						}
					}
				}
			}
		}
		for (int xRange = -range; xRange <= range; xRange++) {
			if (xRange != 0) {
				if (selectedUnit.getX () + xRange < 18 && selectedUnit.getX () + xRange >= 0) {
					if (selectedUnit.getY () + xRange < 9 && selectedUnit.getY () + xRange >= 0) {
						int xTile = selectedUnit.getX () + xRange;
						int yTile = selectedUnit.getY () + xRange;
						if (terrain [xTile, yTile] != 2) {
							if (boardTiles [xTile, yTile].getState () == target && pieces [xTile, yTile].checkIsDead () == false) {
								boardTiles [xTile, yTile].setState (5);
							} else {
								boardTiles [xTile, yTile].setState (6);
							}
						}
					}
				}
			}
		}
		for (int xRange = -range; xRange <= range; xRange++) {
			if (xRange != 0) {
				if (selectedUnit.getX () + xRange < 18 && selectedUnit.getX () + xRange >= 0) {
					if (selectedUnit.getY () - xRange < 9 && selectedUnit.getY () - xRange >= 0) {
						int xTile = selectedUnit.getX () + xRange;
						int yTile = selectedUnit.getY () - xRange;
						if (terrain [xTile, yTile] != 2) {
							if (boardTiles [xTile, yTile].getState () == target && pieces [xTile, yTile].checkIsDead () == false) {
								boardTiles [xTile, yTile].setState (5);
							} else {
								boardTiles [xTile, yTile].setState (6);
							}
						}
					}
				}
			}
		}
		boardTiles [selectedUnit.getX (), selectedUnit.getY ()].setState (3);
	}

	public void resetTiles() {
        int x = selectedUnit.getX();
        int y = selectedUnit.getY();
        if ((x + 1) <= 18) {
            if (!pieces[x + 1, y]) {
                if (boardTiles[x + 1, y].getState() == 4) {
                    setBoardTile(x + 1, y, 0);
                }
            }
        }
        if ((x - 1) >= 0) {
            if (!pieces[x - 1, y]) {
                if (boardTiles[x - 1, y].getState() == 4) {
                    setBoardTile(x - 1, y, 0);
                }
            }
        }
        if ((y + 1) <= 8) {
            if (!pieces[x, y + 1]) {
                if (boardTiles[x, y + 1].getState() == 4) {
                    setBoardTile(x, y + 1, 0);
                }
            }
        }
        if ((y - 1) >= 0) {
            if (!pieces[x, y - 1]) {
                if (boardTiles[x, y - 1].getState() == 4) {
                    setBoardTile(x, y - 1, 0);
                }
            }
        }
        if (selectedUnit) {
			for (int xTile = 0; xTile <= 18; xTile++) {
				for (int yTile = 0; yTile < 9; yTile++) {
					if (terrain [xTile, yTile] != 2) {
						if (boardTiles [xTile, yTile].getState () == 5 || boardTiles [xTile, yTile].getState () == 6) {
							if (pieces [xTile, yTile] != null) {
								int heroTeam = pieces [xTile, yTile].team;
								boardTiles [xTile, yTile].setState (heroTeam);
							} else {
								boardTiles [xTile, yTile].setState (0);
							}
						}
					}
                }
            }
        }
    }

	private void attackTarget (int x, int y) {
		if (selectedUnit) {
			if (currentPlayerTurn == selectedUnit.team - 1) {
				if (selectedUnit.checkAP () != 0 && players [currentPlayerTurn].checkAP () != 0) {
					if (!selectedUnit.checkIsDead ()) {
						if (pieces [x, y]) {
							Piece target = pieces [x, y];
							if (selectedUnit.checkState () == 0 && selectedUnit.checkCanAttack () == true) {
								selectedUnit.attackEnemy (target);
							} else if (selectedUnit.checkState () == 1 && selectedUnit.getActive1Cooldown () == 0) {
								selectedUnit.activeAbility1 (target);
							} else if (selectedUnit.checkState () == 2 && selectedUnit.getActive2Cooldown () == 0) {
								selectedUnit.activeAbility2 (target);
							}
						} else {
							Circle target = boardTiles[x, y];
							if (selectedUnit.checkState () == 1 && selectedUnit.getActive1Cooldown () == 0) {
								selectedUnit.activeAbility1 (target);
							} else if (selectedUnit.checkState () == 2 && selectedUnit.getActive2Cooldown () == 0) {
								selectedUnit.activeAbility2 (target);
							}
						}
						selectedUnit.setAttackState (0);
						players [currentPlayerTurn].takeAction ();
						resetTiles ();
						searchTiles ();
					}
				}
			}
		}
	}

	public string currentPlayersName () {
		return players [currentPlayerTurn].username;
	}

	public Player getPlayer (int n) {
		return players [n];
	}

	public void givePoint (int team) {
		if (team == 1) {
			blueScore++;
		} else if (team == 0) {
			redScore++;
		}
	}

	public void checkForWin () {
		if (redScore >= maxPoints) {
			gameUI.SetActive (false);
			endGameScreen.SetActive (true);
			endGameScreen.GetComponent <ScoreScreen> ().endGame (players [1]);
		} else if (blueScore >= maxPoints) {
			gameUI.SetActive (false);
			endGameScreen.SetActive (true);
			endGameScreen.GetComponent <ScoreScreen> ().endGame (players [0]);
		}
	}

	public void endGame () {
		players [0].endGame ();
		players [1].endGame ();
	}
}