using System;
using UnityEngine;

public class CuttingCounter : BaseCounter, IHasProgress {

	public static event EventHandler OnAnyCut; // Every CuttingCounter fires this for the SoundManager

	new public static void ResetStaticData() {
		OnAnyCut = null; // Clear all of the listeners
	}

	public event EventHandler OnCut; // Each CuttingCounter also fires its own event for the visuals
	public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

	[SerializeField] private CuttingRecipeSO[] cuttingRecipeSOArray;

	private int cuttingProgress;

	public override void Interact(Player player) {
		if (HasKitchenObject()) {
			if (!player.HasKitchenObject()) {
				GetKitchenObject().SetKitchenObjectParent(player);
			} else {
				// Player is carrying something
				if (player.GetKitchenObject().TryGetPlate(out PlateKitchenObject plateKitchenObject)) {
					// Player is carrying a plate
					if (plateKitchenObject.TryAddIngredient(GetKitchenObject().GetKitchenObjectSO())) {
						GetKitchenObject().DestroySelf();
					}
				}

			}
		} else { // The counter is empty
			if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
				player.GetKitchenObject().SetKitchenObjectParent(this);
				cuttingProgress = 0;

				CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
				OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs() { 
					progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
				});
			}
		}
	}

	public override void InteractAlternate(Player player) {
		if (HasKitchenObject() && HasRecipeWithInput(GetKitchenObject().GetKitchenObjectSO())) {
			cuttingProgress++;

			OnCut?.Invoke(this, EventArgs.Empty);
			OnAnyCut?.Invoke(this, EventArgs.Empty);

			CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
			
			OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs () {
				progressNormalized = (float)cuttingProgress / cuttingRecipeSO.cuttingProgressMax
			});

			if (cuttingProgress >= cuttingRecipeSO.cuttingProgressMax) {
				var outputKitchenObjectSO = GetOutputForInput(GetKitchenObject().GetKitchenObjectSO());
				GetKitchenObject().DestroySelf();
				KitchenObject.SpawnKitchenObject(outputKitchenObjectSO, this);
			}
		}
	}

	private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
		CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
		return cuttingRecipeSO != null;
	}

	private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
		CuttingRecipeSO cuttingRecipeSO = GetCuttingRecipeSOWithInput(inputKitchenObjectSO);
		if (cuttingRecipeSO != null) return cuttingRecipeSO.output;

		return null;
	}

	private CuttingRecipeSO GetCuttingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
		foreach (CuttingRecipeSO cuttingRecipeSO in cuttingRecipeSOArray) {
			if (cuttingRecipeSO.input == inputKitchenObjectSO) {
				return cuttingRecipeSO;
			}
		}
		return null;
	}
}
