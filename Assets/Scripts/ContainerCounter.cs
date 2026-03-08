using System;
using UnityEngine;

public class ContainerCounter : BaseCounter {
	public event EventHandler OnPlayerGrabbedObject; // Event for the animator to listen to

	[SerializeField] private KitchenObjectSO kitchenObjectSO;

	public override void Interact(Player player) {
		if (!player.HasKitchenObject()) {
			Transform kitchenObject = Instantiate(kitchenObjectSO.prefab);
			kitchenObject.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
			OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
		}
	}
}
