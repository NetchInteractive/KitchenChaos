using UnityEngine;

public class DeliveryManagerUI : MonoBehaviour {
	[SerializeField] private Transform container;
	[SerializeField] private Transform recipeTemplate;

	private void Awake() {
		recipeTemplate.gameObject.SetActive(false);
	}

	private void Start() {
		DeliveryManager.Instance.OnRecipeAdded += DeliveryManager_OnRecipeAdded;
		DeliveryManager.Instance.OnRecipeRemoved += DeliveryManager_OnRecipeRemoved; ;
	}

	private void DeliveryManager_OnRecipeAdded(object sender, System.EventArgs e) {
		UpdateVisual();
	}

	private void DeliveryManager_OnRecipeRemoved(object sender, System.EventArgs e) {
		UpdateVisual();
	}

	private void UpdateVisual() {
		foreach (Transform t in container) {
			if (t == recipeTemplate) continue;
			Destroy(t.gameObject);
		}

		foreach (var recipeSO in DeliveryManager.Instance.GetWaitingRecipeSOList()) {
			Transform recipeTransform = Instantiate(recipeTemplate, container);
			recipeTransform.gameObject.SetActive(true);

			recipeTransform.GetComponent<DeliveryManagerSingleUI>().SetRecipeSO(recipeSO);
		}
	}
}
