using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUI : MonoBehaviour {
	[SerializeField] private Button playButton;
	[SerializeField] private Button quitButton;

	private void Awake() {
		playButton.onClick.AddListener(() => {
			Loader.Load(Loader.Scene.GameScene);
		});

		quitButton.onClick.AddListener(() => {
			Application.Quit();
		});

		// Reset the timescale in case the player returned from the pause menu
		Time.timeScale = 1f;
	}
}
