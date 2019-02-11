using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Board : MonoBehaviour {

	private Material[] materials;
	public Material[] lands;
	public Material[] waters;
	private float terrain;

	public int [] BoardTerrain;

	// Use this for initialization
	void Start () {
		Renderer rend = GetComponent <Renderer> ();
		materials = rend.materials;
		terrain = Random.Range (0.0f, 4.0f);
		materials [2] = waters [(int)terrain];
		materials [3] = lands [(int)terrain];

		rend.materials = materials;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int getTerrain (int row, int column) {
		return BoardTerrain [row * 9 + column];
	}
}
