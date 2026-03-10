using UnityEngine;

public class StoveCounterVisual : MonoBehaviour {
	[SerializeField] private StoveCounter stoveCounter;
	[SerializeField] private GameObject[] visualGameObjects;

	private void Start() {
		stoveCounter.OnStateChanged += StoveCounter_OnStateChanged;
	}

	private void StoveCounter_OnStateChanged(object sender, StoveCounter.OnStateChangedEventArgs e) {
		bool showVisual = e.state == StoveCounter.State.Frying || e.state == StoveCounter.State.Fried;
		ToggleVisuals(showVisual);
	}

	private void ToggleVisuals(bool enable) {
		foreach (var gameObject in visualGameObjects) {
			gameObject.SetActive(enable);
		}
	}
}
