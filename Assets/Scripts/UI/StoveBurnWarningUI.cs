using UnityEngine;

public class StoveBurnWarningUI : MonoBehaviour {
	[SerializeField] private StoveCounter stoveCounter;
	[SerializeField] private Transform warningImageTransform;

	private void Start() {
		stoveCounter.OnProgressChanged += StoveCounter_OnProgressChanged;
		Hide();
	}

	private void StoveCounter_OnProgressChanged(object sender, IHasProgress.OnProgressChangedEventArgs e) {
		float burnShowProgessAmount = 0.5f;
		bool show = stoveCounter.IsFried() && e.progressNormalized >= burnShowProgessAmount;

		if (show) {
			Show();
		} else {
			Hide();	
		}
	}


	private void Show() {
		warningImageTransform.gameObject.SetActive(true);
	}

	private void Hide() { 
		warningImageTransform.gameObject.SetActive(false);
	}
}
