using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour {

	public static GameManager Instance { set; get; }

	public GameObject mainMenu;
	public GameObject hostMenu;
	public GameObject connectMenu;

	public GameObject serverPrefab;
	public GameObject clientPrefab;

	// Use this for initialization
	void Start () {
		Instance = this;
		DontDestroyOnLoad (gameObject);
	}
	
	// Update is called once per frame
	public void ConnectMenuButton () {
		mainMenu.SetActive (false);
		connectMenu.SetActive (true);
	}

	public void HostMenuButton () {
		try {
			Server s = Instantiate (serverPrefab).GetComponent <Server>();
		}
		catch (Exception e) {
			Debug.Log (e.Message);
		}

		mainMenu.SetActive (false);
		hostMenu.SetActive (true);

	}

	public void ConnectToServer () {
		string hostAddress = GameObject.Find ("HostInput").GetComponent <InputField> ().text;
		if (hostAddress == "")
			hostAddress = "127.0.0.1";

		try {
			Client c = Instantiate (clientPrefab).GetComponent <Client>();
			c.ConnectToServer (hostAddress, 6321);
			connectMenu.SetActive (false);
		}
		catch (Exception e) {
			Debug.Log (e.Message);
		}
	}

	public void CancelButton () {
		mainMenu.SetActive (true);
		connectMenu.SetActive (false);
		hostMenu.SetActive (false);
	}
}
