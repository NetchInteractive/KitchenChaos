using System;
using UnityEngine;

public class KitchenGameManager : MonoBehaviour {
	public static KitchenGameManager Instance { get; private set; }

	public event EventHandler OnGamePaused;
	public event EventHandler OnGameUnpaused;

	public event EventHandler OnStateChanged;

	private enum State { 
		WaitingToStart,
		CountdownToStart,
		GamePlaying,
		GameOver
	}

	private State state;
	private float countdownToStartTimer = 3f;
	private float gamePlayingTimer;
	private float gamePlayingTimerMax = 30f;

	private bool isGamePaused;

	private void Awake() {
		Instance = this;
		state = State.WaitingToStart;
	}

	private void Start() {
		GameInput.Instance.OnPauseAction += GameInput_OnPauseAction;

		GameInput.Instance.OnInteractAction += GameInput_OnInteractAction;
	}

	private void GameInput_OnInteractAction(object sender, EventArgs e) {
		if (state == State.WaitingToStart) {
			ChangeState(State.CountdownToStart);
		}
	}

	private void GameInput_OnPauseAction(object sender, EventArgs e) {
		TogglePauseGame();
	}

	private void Update() {
		switch (state) {
			case State.WaitingToStart:

				break;

			case State.CountdownToStart:
				countdownToStartTimer -= Time.deltaTime;
				if (countdownToStartTimer < 0f) {
					gamePlayingTimer = gamePlayingTimerMax;
					ChangeState(State.GamePlaying);
				}
				break;

			case State.GamePlaying:
				gamePlayingTimer -= Time.deltaTime;
				if (gamePlayingTimer < 0f) {
					ChangeState(State.GameOver);
				}
				break;

			case State.GameOver:

				break;
		}
	}

	private void ChangeState(State state) {
		this.state = state;
		OnStateChanged?.Invoke(this, EventArgs.Empty);
	}

	public bool IsGamePlaying() {
		return state == State.GamePlaying;
	}

	public bool IsCountdownToStartActive() {
		return state == State.CountdownToStart;
	}

	public float GetCountdownToStartTimer() {
		return countdownToStartTimer;
	}

	public bool IsGameOver() {
		return state == State.GameOver;
	}

	public float GetGamePlayingTimerNormalized() {
		// Invert since we're counting down
		return 1 - (gamePlayingTimer / gamePlayingTimerMax);
	}

	public void TogglePauseGame() {
		isGamePaused = !isGamePaused;
		if (isGamePaused) { 
			Time.timeScale = 0f;
			OnGamePaused?.Invoke(this, EventArgs.Empty);
		} else {
			Time.timeScale = 1f;
			OnGameUnpaused?.Invoke(this, EventArgs.Empty);
		}
	}
}
