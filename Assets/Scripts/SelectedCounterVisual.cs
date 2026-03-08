using UnityEngine;

public class SelectedCounterVisual : MonoBehaviour {
	[SerializeField] private ClearCounter counter;
	[SerializeField] private GameObject visualGameObject;

	private void Start() {
		Player.Instance.OnSelectedCounterChanged += Instance_OnSelectedCounterChanged;
	}

	private void Instance_OnSelectedCounterChanged(object sender, Player.OnSelectedCounterChangedEventArgs e) {
		if (e.selectedCounter == counter) {
			Show();
		} else {
			Hide();
		}
	}

	private void Show() {
		visualGameObject.SetActive(true);
	}

	private void Hide() { 
		visualGameObject.SetActive(false);
	}
}
