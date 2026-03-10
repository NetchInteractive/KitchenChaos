using UnityEngine;

public class ClearCounter : BaseCounter {
	[SerializeField] private KitchenObjectSO kitchenObjectSO;

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
				} else {
					// Player is carrying something other than a plate
					if (GetKitchenObject().TryGetPlate(out plateKitchenObject)) { 
						// Counter has a plate, try to add ingredient the player is holding
						if (plateKitchenObject.TryAddIngredient(player.GetKitchenObject().GetKitchenObjectSO())) {
							player.GetKitchenObject().DestroySelf();
						}
					}
				}
			}
		} else { // The counter is empty
			if (player.HasKitchenObject()) {
				player.GetKitchenObject().SetKitchenObjectParent(this);
			}
		}
	}
}
