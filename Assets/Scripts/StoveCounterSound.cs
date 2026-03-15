using UnityEngine;

public class StoveCounterSound : MonoBehaviour {
	[SerializeField] StoveCounter stoveCounter;
	private AudioSource fryingSFX;

	private void Awake() {
		fryingSFX = GetComponent<AudioSource>();
	}

	private void Start() {
		stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
	}

	private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e) {
		bool isStoveOn = e.state == StoveCounter.State.Fried || e.state == StoveCounter.State.Frying;
		if (isStoveOn) {
			fryingSFX.Play();
		} else {
			fryingSFX.Pause();
		}
	}
}
