using System;
using UnityEngine;

public class ContainerCounter : BaseCounter {
	public event EventHandler OnPlayerGrabbedObject;

	[SerializeField] private KitchenObjectSO kitchenObjectSO;

	public override void Interact(Player player) {
		Transform tomato = Instantiate(kitchenObjectSO.prefab);
		tomato.GetComponent<KitchenObject>().SetKitchenObjectParent(player);
		OnPlayerGrabbedObject?.Invoke(this, EventArgs.Empty);
	}
}
