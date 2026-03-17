using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryResultUI : MonoBehaviour {
	[SerializeField] private Image backgroundImage;
	[SerializeField] private Image iconImage;
	[SerializeField] private TextMeshProUGUI messageText;

	[SerializeField] private Color successColour;
	[SerializeField] private Color failedColour;
	[SerializeField] private Sprite successSprite;
	[SerializeField] private Sprite failedSprite;

	private Animator animator;
	private const string POPUP = "Popup";

	private void Awake() {
		animator = GetComponent<Animator>();
	}


	private void Start() {
		DeliveryManager.Instance.OnDeliverSuccess += DeliveryManager_OnDeliverSuccess;
		DeliveryManager.Instance.OnDeliverFail += DeliveryManager_OnDeliverFail;

		Hide();
	}

	private void DeliveryManager_OnDeliverFail(object sender, System.EventArgs e) {
		Show();
		backgroundImage.color = failedColour;
		iconImage.sprite = failedSprite;
		messageText.text = "DELIVERY\nFAILED";
		animator.SetTrigger(POPUP);
	}

	private void DeliveryManager_OnDeliverSuccess(object sender, System.EventArgs e) {
		Show();
		backgroundImage.color = successColour;
		iconImage.sprite = successSprite;
		messageText.text = "DELIVERY\nSUCCESS";
		animator.SetTrigger(POPUP);
	}

	private void Show() {
		gameObject.SetActive(true);
	}

	private void Hide() {
		gameObject.SetActive(false);
	}
}
