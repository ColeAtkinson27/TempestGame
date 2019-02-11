using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public static class SaveLoad {
	public static PlayerData player = new PlayerData();
	public static int [] expCaps = new int [11];

	public static void Save () {
		BinaryFormatter bf = new BinaryFormatter ();
		FileStream file = File.Create (Application.persistentDataPath + "/playerData.er");
		bf.Serialize (file, SaveLoad.player);
		file.Close ();
		Load ();
	}

	public static void Load () {
		if(File.Exists(Application.persistentDataPath + "/playerData.er")) {
			BinaryFormatter bf = new BinaryFormatter ();
			FileStream file = File.Open (Application.persistentDataPath + "/playerData.er", FileMode.Open);
			SaveLoad.player = (PlayerData)bf.Deserialize (file);
			file.Close ();
			setEXPCaps ();
		}
	}

	private static void setEXPCaps() {
		expCaps [1] = 2;
		expCaps [2] = 3;
		expCaps [3] = 5;
		expCaps [4] = 7;
		expCaps [5] = 9;
		expCaps [6] = 12;
		expCaps [7] = 15;
		expCaps [8] = 18;
		expCaps [9] = 22;
		expCaps [10] = 0;
	}
}
