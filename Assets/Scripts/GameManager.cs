using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameManager : MonoBehaviour
{
	public static GameManager instance;

	public GameObject imageMenu;
	public GameObject mainMenu;
	public GameObject optionsMenu;
	public GameObject loaderMenu;
	public GameObject timerUI;
	public GameObject completeScreen;
	// public Slider loader;
	public Camera menuCamera;

	public Transform playerPrefab;

	public AudioSource completeMaze;
	public AudioSource mazeTheme;
	public AudioSource menuTheme;

	private Transform player;
	private float timer;

	void Awake() {
		if ( instance != null && instance != this ) {
			Destroy(this.gameObject);
		} else {
			instance = this;
		}
	}

	void Start() {
		PlayTheme(menuTheme);
	}

	public void StartGame() {
		if ( playerPrefab != null ) {
			player = Instantiate( playerPrefab );
		}
		timer = 0;
		StartCoroutine("TimerController");

		HideMenu();
		if ( timerUI != null ) {
			timerUI.gameObject.SetActive(true);
		}

		StopTheme( GameManager.instance.menuTheme );
		PlayTheme( GameManager.instance.mazeTheme );
	}

	public void CompleteMaze() {
		StopTheme(mazeTheme);
		PlayTheme(completeMaze);
		StopCoroutine("TimerController");

		menuCamera.gameObject.SetActive(true);
		timerUI.gameObject.SetActive(false);
		completeScreen.gameObject.SetActive(true);
		StartCoroutine("FillCompleteMazeScreen");

		Destroy(player.gameObject);
	}

	IEnumerator FillCompleteMazeScreen() {
		float completeTime = timer;
		int delta = (int) (timer * 0.02f ) + 1;
		while ( timer > 0 ) {
			int diff = (int) ( completeTime - timer );
			timer -= delta;

			completeScreen.GetComponentInChildren<TextMeshProUGUI>().text = "Complete in : " + TimeFormater(diff);

			yield return new WaitForSeconds(0.01f);
		}
	}

	public void SetLoaderMenuValue(float value) {
		loaderMenu.GetComponentInChildren<TextMeshProUGUI>().text = Mathf.Floor(value * 100) + "%";
		loaderMenu.GetComponentInChildren<Slider>().value = value;
	}

	IEnumerator TimerController() {
		while (true) {
			timer += 1f;

			timerUI.GetComponent<TextMeshProUGUI>().text = TimeFormater(timer);

			yield return new WaitForSeconds(0.2f);
		}
	}

	string TimeFormater(float time) {
		int seconds = (int) ( time % 60 );
		int minutes = (int) ( ( time - seconds ) / 60 ) % 60;
		int hours = (int) ( ( time - minutes * 60 - seconds ) / 3600 );

		string format = "";
		if ( hours > 0 ) {
			if ( hours < 10 ) {
				format += "0";
			}
			format += hours + ":";
		}
		// if ( minutes > 0 || hours > 0) {
			if ( minutes < 10 ) {
				format += "0";
			}
			format += minutes + ":";
		// }
		if ( seconds < 10 ) {
			format += "0";
		}
		format += seconds;

		return format;
	}

	// public void ShowMenu(GameObject menu) {	}

	public void HideMenu() {
		menuCamera.gameObject.SetActive(false);
		mainMenu.gameObject.SetActive(false);
		optionsMenu.gameObject.SetActive(false);
		loaderMenu.gameObject.SetActive(false);
		imageMenu.gameObject.SetActive(false);
	}

	// public void HideMenu(GameObject menu) {
	// 	menu.gameObject.SetActive(false);
	// }

	public void PlayTheme(AudioSource source) {
		if ( source != null && source.isPlaying == false ) {
			source.Play();
		}
	}

	public void StopTheme(AudioSource source) {
		if ( source != null ) {
			source.Stop();
		}
	}

	public void QuitApplication() {
		Application.Quit();
	}

}
