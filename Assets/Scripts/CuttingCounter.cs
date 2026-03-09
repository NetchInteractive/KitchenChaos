using UnityEngine;

public class CuttingCounter : BaseCounter {
	[SerializeField] private KitchenObjectSO slicedObject;

	public override void Interact(Player player) {
		if (HasKitchenObject()) {
			if (!player.HasKitchenObject()) {
				GetKitchenObject().SetKitchenObjectParent(player);
			}
		} else { // The counter is empty
			if (player.HasKitchenObject()) {
				player.GetKitchenObject().SetKitchenObjectParent(this);
			}
		}
	}

	public override void InteractAlternate(Player player) {
		if (HasKitchenObject()) {
			GetKitchenObject().DestroySelf();
			KitchenObject.SpawnKitchenObject(slicedObject, this);
		}
	}
}
