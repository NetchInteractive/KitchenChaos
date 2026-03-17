using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GamePausedUI : MonoBehaviour {
	[SerializeField] private Button mainMenuButton;
	[SerializeField] private Button optionsButton;
	[SerializeField] private Button resumeButton;

	private void Start() {
		KitchenGameManager.Instance.OnGamePaused += KitchenGameManager_OnGamePaused;
		KitchenGameManager.Instance.OnGameUnpaused += KitchenGameManager_OnGameUnpaused;

		mainMenuButton.onClick.AddListener(() => {
			Loader.Load(Loader.Scene.MainMenuScene);
		});

		resumeButton.onClick.AddListener(() => {
			KitchenGameManager.Instance.TogglePauseGame();
		});

		optionsButton.onClick.AddListener(() => {
			Hide();
			OptionsUI.Instance.Show(Show);
		});

		Hide();
	}
	
	private void KitchenGameManager_OnGameUnpaused(object sender, System.EventArgs e) {
		Hide();
	}

	private void KitchenGameManager_OnGamePaused(object sender, System.EventArgs e) {
		Show();
	}

	private void Show() { 
		gameObject.SetActive(true);

		// Select the first button for gamepad support
		// This ensures the user can navigate using a gamepad
		resumeButton.Select();
	}

	private void Hide() {
		gameObject.SetActive(false);
	}
 }
