using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeliveryManagerSingleUI : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI recipeText;

    [SerializeField] private Transform iconContainer;
    [SerializeField] private Transform iconTemplate;

	private void Awake() {
		iconTemplate.gameObject.SetActive(false);
	}

	public void SetRecipeSO(RecipeSO recipeSO) {
        recipeText.text = recipeSO.recipeName;

        foreach (Transform t in iconContainer) {
            if (t == iconTemplate) continue;
            Destroy(t.gameObject);
        }

		foreach (var ingredient in recipeSO.kitchenObjectSOList) {
			Image newIcon = Instantiate(iconTemplate, iconContainer).GetComponent<Image>();
			newIcon.gameObject.SetActive(true);
			newIcon.sprite = ingredient.sprite;
		}
	}
}
