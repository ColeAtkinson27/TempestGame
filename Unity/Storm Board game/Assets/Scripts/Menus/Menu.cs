using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour {
	public GameObject MainMenu;
	public GameObject PlayMenu;
	public GameObject ArmoryMenu;
	public GameObject ChronicleMenu;
	public GameObject OptionsMenu;
	public GameObject ChangelogMenu;
	public GameObject LANMenu;
	public GameObject ProfileMenu;
	public GameObject MatchMakingMenu;
	public GameObject MultiplayerMatchMakingMenu;
	public GameObject HeroSelectionMenu;

	public HeroMenu armory;
	public HeroMenu chronicle;

	public Slider [] volumeSliders;
	public Toggle [] resolutionToggles;
	public int [] screenWidth;
	private int activeScreenRes;

	public Image Portrait;
	public Text Username;
	public InputField TestUsername;

	public Image [] SexBorders;
	public Image [] ClothingBorders;
	public Image [] ColorBorders;
	public Image [] ColorPortraits;
	public Image [] ClothingPortraits;

	public Sprite [] ClothingPortraitsData;
	public MenuStyleManager StylesManager;
	public GameObject view;

	public Player player1;
	public Player player2;
	public MatchScreen matchScreen;

	public GameObject userPrefab;
	public GameObject AIPrefab;

	void Start () {
		SaveLoad.Load ();
		view.SetActive (true);
		Username.GetComponent<Text>().text = PlayerPrefs.GetString ("Username");
		activeScreenRes = PlayerPrefs.GetInt ("Screen Res");
		//bool Fullscreen = (PlayerPrefs.GetInt ("Fullscreen") == 1) ? true : false;
	
		for (int i = 0; i < resolutionToggles.Length; i++) {
			resolutionToggles [i].isOn = i == activeScreenRes;
		}

		//SetFullscreen (Fullscreen);

		//Portrait sets
		/*for (int s = 0; s < 2; s++) {
			for (int c = 0; c < 6; c++) {
				for (int rgb = 0; rgb < 6; rgb++) {
					Debug.Log (s * 32 + c * 6 + rgb);
					ClothingPortraitsData [s][c][rgb] = StylesManager.getClothes (s,c,rgb);
				}
			}
		}*/
		Portrait.GetComponent<Image> ().sprite = ClothingPortraitsData [(PlayerPrefs.GetInt ("AvatarSex") * 6) + (PlayerPrefs.GetInt ("AvatarClothing") * 12) + PlayerPrefs.GetInt ("AvatarColor")];
		testSex (PlayerPrefs.GetInt ("AvatarSex"));
	}

	//MAIN MENU
	public void Play () {
		SceneManager.LoadScene ("MatchMaking");
	}

	public void Armory () {
		SceneManager.LoadScene ("Armory");
	}

	public void Chronicle () {
		MainMenu.SetActive (false);
		ChronicleMenu.SetActive (true);
	}

	public void Options () {
		MainMenu.SetActive (false);
		OptionsMenu.SetActive (true);
	}

	public void Changelog () {
		MainMenu.SetActive (false);
		ChangelogMenu.SetActive (true);
	}

	public void Quit () {
		Application.Quit();
	}

	public void EditProfile () {
		MainMenu.SetActive (false);
		ProfileMenu.SetActive (true);
		SexBorders [PlayerPrefs.GetInt ("AvatarSex")].color = new Color32 (0, 150, 200, 100);
		ClothingBorders [PlayerPrefs.GetInt ("AvatarClothing")].color = new Color32 (0, 150, 200, 100);
		ColorBorders [PlayerPrefs.GetInt ("AvatarColor")].color = new Color32 (0, 150, 200, 100);
		TestUsername.GetComponent<InputField>().text = PlayerPrefs.GetString ("Username");
		testSex (PlayerPrefs.GetInt ("AvatarSex"));
		Portrait.GetComponent<Image> ().sprite = ClothingPortraitsData [(PlayerPrefs.GetInt ("AvatarSex") * 6) + (PlayerPrefs.GetInt ("AvatarClothing") * 12) + PlayerPrefs.GetInt ("AvatarColor")];
	}

	//PLAY MENU

	public void PlayWithAI () {
		PlayMenu.SetActive (false);
		MatchMakingMenu.SetActive (true);
		matchScreen = MatchMakingMenu.GetComponent <MatchScreen> ();
		GameObject user = Instantiate (userPrefab) as GameObject;
		GameObject ai = Instantiate (AIPrefab) as GameObject;
		player1 = user.GetComponent <Player> ();
		player1.setTeam (1);
		player2 = ai.GetComponent <Player> ();
		player2.setTeam (2);
		matchScreen.players [0] = player1;
		matchScreen.players [1] = player2;
	}

	public void PlayHotSeat () {
		PlayMenu.SetActive (false);
		MultiplayerMatchMakingMenu.SetActive (true);
		matchScreen = MultiplayerMatchMakingMenu.GetComponent <MatchScreen> ();
		GameObject user1 = Instantiate (userPrefab) as GameObject;
		GameObject user2 = Instantiate (userPrefab) as GameObject;
		player1 = user1.GetComponent <Player> ();
		player1.setTeam (1);
		player1.username = PlayerPrefs.GetString ("Username");
		player2 = user2.GetComponent <Player> ();
		player2.setTeam (2);
		matchScreen.players [0] = player1;
		matchScreen.players [1] = player2;
	}

	public void ConnectViaLAN () {
		PlayMenu.SetActive (false);
		LANMenu.SetActive (true);
	}

	public void ExitLAN () {
		PlayMenu.SetActive (true);
		LANMenu.SetActive (false);
	}

	public void EnterGame () {
		if (matchScreen.playerNames [1].text == "")
			player2.username = "Player 2";
		SceneManager.LoadScene ("Game");
		player1.setRandomHeroes ();
		player2.setRandomHeroes ();
	}

	public void ExitPlayMenu () {
		PlayMenu.SetActive (false);
		MainMenu.SetActive (true);
	}

	public void ExitMatchMaking () {
		MultiplayerMatchMakingMenu.SetActive (false);
		MatchMakingMenu.SetActive (false);
		PlayMenu.SetActive (true);
	}

	//ARMORY

	public void ExitArmory () {
		ArmoryMenu.SetActive (false);
		MainMenu.SetActive (true);
	}

	//CHRONICLE

	public void ExitChronicle () {
		ChronicleMenu.SetActive (false);
		MainMenu.SetActive (true);
	}

	//OPTIONS MENU
	public void SetScreenResolution (int i) {
		/*if (resolutionToggles [i].isOn) {
			float aspectRatio = 16 / 9f;
			Screen.SetResolution (screenWidth [i], (int)(screenWidth [i] / aspectRatio), false);
			activeScreenRes = i;
			PlayerPrefs.SetInt ("Screen Res", activeScreenRes);
			PlayerPrefs.Save ();
		}*/
	}

	public void SetMasterVolume (float value) {
		//AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.Master);
	}

	public void SetMusicVolume (float value) {
		//AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.Music);
	}

	public void SetSFXVolume (float value) {
		//AudioManager.instance.SetVolume (value, AudioManager.AudioChannel.SFX);
	}

	public void SetFullscreen (bool Fullscreen) {
		for (int i = 0; i < resolutionToggles.Length; i++){
			resolutionToggles [i].interactable = !Fullscreen;
		}

		if (Fullscreen) {
			Resolution[] allResolutions = Screen.resolutions;
			Resolution maxRes = allResolutions [allResolutions.Length - 1];
			Screen.SetResolution (maxRes.width, maxRes.height, true);
		} else {
			float aspectRatio = 16 / 9f;
			Screen.SetResolution (screenWidth [activeScreenRes], (int)(screenWidth [activeScreenRes] / aspectRatio), false);
		}
		PlayerPrefs.SetInt ("Fullscreen", ((Fullscreen) ? 1 : 0));
		PlayerPrefs.Save ();
	}

	public void ExitOptions () {
		OptionsMenu.SetActive (false);
		MainMenu.SetActive (true);
	}

	//CHANGELOG
	public void ExitChangelog () {
		ChangelogMenu.SetActive (false);
		MainMenu.SetActive (true);
	}

	//PROFILE
	public void ExitProfile () {
		ProfileMenu.SetActive (false);
		MainMenu.SetActive (true);
		Username.GetComponent<Text>().text = PlayerPrefs.GetString ("Username");
				Portrait.GetComponent<Image>().sprite = ClothingPortraitsData [(PlayerPrefs.GetInt ("AvatarSex") * 6) + (PlayerPrefs.GetInt ("AvatarClothing") * 12) + PlayerPrefs.GetInt ("AvatarColor")];
	}

	public void ChangePortrait (int PorNum) {
		PlayerPrefs.SetInt ("Portrait", PorNum);
		PlayerPrefs.Save ();
	}

	public void ChangeName () {
		string NewName = TestUsername.GetComponent<InputField> ().text;
		PlayerPrefs.SetString ("Username", NewName);
		PlayerPrefs.Save ();
	}

	public void SetSex (int gender) {
		SexBorders [PlayerPrefs.GetInt ("AvatarSex")].color = new Color32 (0, 0, 0, 0);
		PlayerPrefs.SetInt ("AvatarSex", gender);
		PlayerPrefs.Save ();
		SexBorders [PlayerPrefs.GetInt ("AvatarSex")].color = new Color32 (0, 255, 255, 100);
		testSex (gender);
	}

	public void SetClothing (int costume) {
		ClothingBorders [PlayerPrefs.GetInt ("AvatarClothing")].color = new Color32 (255, 255, 0, 0);
		PlayerPrefs.SetInt ("AvatarClothing", costume);
		PlayerPrefs.Save ();
		ClothingBorders [PlayerPrefs.GetInt ("AvatarClothing")].color = new Color32 (0, 255, 255, 100);
		testSex (PlayerPrefs.GetInt ("AvatarSex"));
	}

	public void SetColor (int rainbow) {
		ColorBorders [PlayerPrefs.GetInt ("AvatarColor")].color = new Color32 (255, 255, 0, 0);
		PlayerPrefs.SetInt ("AvatarColor", rainbow);
		PlayerPrefs.Save ();
		ColorBorders [PlayerPrefs.GetInt ("AvatarColor")].color = new Color32 (0, 255, 255, 100);
		testSex (PlayerPrefs.GetInt ("AvatarSex"));
	}

	public void testSex (int gender) {
		int ClothingColor = PlayerPrefs.GetInt ("AvatarColor");
		int ClothingStyle = PlayerPrefs.GetInt ("AvatarClothing");
		for (int set = 0; set < 6; set++) {
			ColorPortraits [set].sprite = ClothingPortraitsData [gender * 6 + ClothingStyle * 12 + set];
			ClothingPortraits [set].sprite = ClothingPortraitsData [gender * 6 + set * 12 + ClothingColor];
		}
	}

	//LAN MENU
	void OnPlayerConnected (NetworkPlayer Player) {
		
	}

	//Matchmaking menu
	public void enterSelection (int player) {
		int h1, h2, h3, h4, h5;
		if (player == 1) {
			h1 = player2.giveHeroNumber (0);
			h2 = player2.giveHeroNumber (1);
			h3 = player2.giveHeroNumber (2);
			h4 = player2.giveHeroNumber (3);
			h5 = player2.giveHeroNumber (4);
		} else {
			h1 = player1.giveHeroNumber (0);
			h2 = player1.giveHeroNumber (1);
			h3 = player1.giveHeroNumber (2);
			h4 = player1.giveHeroNumber (3);
			h5 = player1.giveHeroNumber (4);
		}

		HeroSelectionMenu.SetActive (true);
		HeroSelectionMenu.GetComponentInChildren <HeroSelection> ().enterMenu (player, h1, h2, h3, h4, h5);
		MatchMakingMenu.SetActive (false);
		MultiplayerMatchMakingMenu.SetActive (false);
	}

	public void setTeam (int player,bool accept, int hero1, int hero2, int hero3, int hero4, int hero5) {
		HeroSelectionMenu.SetActive (false);
		MultiplayerMatchMakingMenu.SetActive (true);
		if (accept == true) {
			if (player == 1) {
				player2.changeHero (0, hero1);
				player2.changeHero (1, hero2);
				player2.changeHero (2, hero3);
				player2.changeHero (3, hero4);
				player2.changeHero (4, hero5);
			} else {
				player1.changeHero (0, hero1);
				player1.changeHero (1, hero2);
				player1.changeHero (2, hero3);
				player1.changeHero (3, hero4);
				player1.changeHero (4, hero5);
			}
		}
	}

	//==SHOP==//
	public void Shop () {
		SceneManager.LoadScene ("Shop");
	}

	//==DEV OPTIONS==//
	public GameObject devOptionsMenu;

	public void enterDOM () {
		devOptionsMenu.SetActive (true);
		MainMenu.SetActive (false);
	}

	public void exitDOM () {
		devOptionsMenu.SetActive (false);
		MainMenu.SetActive (true);
	}


	private int devHeroPick;
	public Toggle[] skinsDev;
	public Toggle[] portraitsDev;
	public Toggle[] levelDev;
	public Toggle unlockedDev;
	public Text DevName;
	public string [] DevNames;
	public void pickHeroDev (int h) {
		devHeroPick = h;
		for (int i = 0; i < 4; i++) {
			skinsDev [i].isOn = SaveLoad.player.skinsUnlocked [h, i + 1];
			portraitsDev [i].isOn = SaveLoad.player.portraitsUnlocked [h, i + 1];
		}
		levelDev [SaveLoad.player.level [h] - 1].isOn = true;
		unlockedDev.isOn = SaveLoad.player.heroUnlocked [h];
		DevName.text = DevNames [h];
	}

	public void DevTogglePortrait (int p) {
		SaveLoad.player.portraitsUnlocked [devHeroPick, p] = !(SaveLoad.player.portraitsUnlocked [devHeroPick, p]);
		SaveLoad.Save ();
		portraitsDev [p - 1].isOn = SaveLoad.player.portraitsUnlocked [devHeroPick, p];
	}

	public void DevToggleSkin (int s) {
		SaveLoad.player.skinsUnlocked [devHeroPick, s] = !(SaveLoad.player.skinsUnlocked [devHeroPick, s]);
		SaveLoad.Save ();
		skinsDev [s - 1].isOn = SaveLoad.player.skinsUnlocked [devHeroPick, s];
	}

	public void DevToggleLevel (int l) {
		SaveLoad.player.level [devHeroPick] = l;
		SaveLoad.Save ();
		levelDev [l - 1].isOn = true;
	}

	public void DevToggleUnlocked () {
		SaveLoad.player.heroUnlocked [devHeroPick] = !(SaveLoad.player.heroUnlocked [devHeroPick]);
		SaveLoad.Save ();
		unlockedDev.isOn = SaveLoad.player.heroUnlocked [devHeroPick];
	}
}
