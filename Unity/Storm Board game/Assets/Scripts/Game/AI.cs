using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Player {

	// Use this for initialization
	public override void Start () {
		DontDestroyOnLoad (gameObject);
		username = "AI opponent";
		controllable = false;
	}
	
	// Update is called once per frame
	void Update () {
		if (!gameController)
			linkToBoard ();
	}
}
