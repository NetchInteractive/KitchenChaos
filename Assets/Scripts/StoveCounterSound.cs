using UnityEngine;

public class StoveCounterSound : MonoBehaviour {
	[SerializeField] StoveCounter stoveCounter;
	private AudioSource fryingSFX;

	private float warningSoundTimer;
	bool playWarningSound;

	private void Awake() {
		fryingSFX = GetComponent<AudioSource>();
	}

	private void Start() {
		stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
		stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
	}

	private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
		float burnShowProgessAmount = 0.5f;
		playWarningSound = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgessAmount;
	}

	private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e) {
		bool isStoveOn = e.state == StoveCounter.State.Fried || e.state == StoveCounter.State.Frying;
		if (isStoveOn) {
			fryingSFX.Play();
		} else {
			fryingSFX.Pause();
		}
	}

	private void Update() {
		if (playWarningSound) {
			warningSoundTimer -= Time.deltaTime;
			if (warningSoundTimer <= 0f) {
				float warningSoundTimerMax = 0.2f;
				warningSoundTimer = warningSoundTimerMax;

				SoundManager.Instance.PlayWarningSound(stoveCounter.transform.position);
			}
		}
	}
}
