using System;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryManager : MonoBehaviour {
	public event EventHandler OnRecipeAdded;
	public event EventHandler OnRecipeRemoved;

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
				waitingRecipeSOList.Add(recipeListSO.recipeSOList[UnityEngine.Random.Range(0, recipeListSO.recipeSOList.Count)]);
				OnRecipeAdded?.Invoke(this, EventArgs.Empty);
			}
		}
	}

	public void DeliverRecipe(PlateKitchenObject plateKitchenObject) {
		for (int i = 0; i < waitingRecipeSOList.Count; i++) {
			RecipeSO waitingRecipeSO = waitingRecipeSOList[i];

			if (waitingRecipeSO.kitchenObjectSOList.Count == plateKitchenObject.GetKitchenObjectSOList.Count) {

				bool hasAllIngredients = true;
				foreach (var recipeIngredient in waitingRecipeSO.kitchenObjectSOList) {
					if (!plateKitchenObject.GetKitchenObjectSOList.Contains(recipeIngredient)) {
						hasAllIngredients = false;
					}
				}

				if (hasAllIngredients) {
					waitingRecipeSOList.RemoveAt(i);
					OnRecipeRemoved?.Invoke(this, EventArgs.Empty);
					return;
				}
  			}
		}

		// No matches found, the player did not deliver a correct recipe
		Debug.Log($"No matches found");
	}

	public List<RecipeSO> GetWaitingRecipeSOList => waitingRecipeSOList;
}
