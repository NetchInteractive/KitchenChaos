using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {
	[SerializeField] private RecipeListSO recipeListSO;

	public static DeliveryManager Instance;

	private List<RecipeSO> waitingRecipeSOList;


	private float spawnRecipeTimer = 0f;
	private float spawnRecipeTimerMax = 4f;

	private int waitingRecipeMax = 4;

	private void Awake() {
		if (Instance != null) {
			Debug.LogError($"There should only be one instance of DeliveryManager in the scene");
		}

		Instance = this;

		waitingRecipeSOList = new List<RecipeSO>();
	}

	private void Update() {
		spawnRecipeTimer += Time.deltaTime;

		if (spawnRecipeTimer > spawnRecipeTimerMax) {
			spawnRecipeTimer = 0f;

			if (waitingRecipeSOList.Count < waitingRecipeMax) {
				// Spawn a recipe
				waitingRecipeSOList.Add(recipeListSO.recipeSOList[Random.Range(0, recipeListSO.recipeSOList.Count - 1)]);
				Debug.Log($"Added {waitingRecipeSOList[waitingRecipeSOList.Count - 1].recipeName}");
			}
		}
	}

	public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
		for (int i = 0; i < waitingRecipeSOList.Count; i++) {
			RecipeSO waitingRecipeSO = waitingRecipeSOList[i];
			Debug.Log($"Checking waitingRecipeSO {waitingRecipeSO.recipeName}");

			if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList.Count) {

				bool hasAllIngredients = true;
				foreach (var recipeIngredient in waitingRecipeSO.kitchenObjectSOList) {
					if (!plateKitchenObject.GetKitchenObjectSOList.Contains(recipeIngredient)) {
						Debug.Log($"Plate is missing {recipeIngredient.objectName} for {waitingRecipeSO.recipeName}");
						hasAllIngredients = false;
					}
				}

				if (hasAllIngredients) {
					Debug.Log("Success");
					waitingRecipeSOList.RemoveAt(i);
					return;
				}
  			}
		}

		// No matches found, the player did not deliver a correct recipe
		Debug.Log($"No matches found");
	}
}
