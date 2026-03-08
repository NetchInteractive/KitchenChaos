using UnityEngine;

public class ClearCounter : MonoBehaviour, IKitchenObjectParent {
	[SerializeField] private Transform counterTopPoint;
	[SerializeField] private KitchenObjectSO tomatoSO;

	private KitchenObject kitchenObject;
	public void Interact(Player player) {
		if (kitchenObject == null) {
			Transform tomato = Instantiate(tomatoSO.prefab, counterTopPoint);
			tomato.GetComponent<KitchenObject>().SetKitchenObjectParent(this);
		} else {
			// Give the object to the player
			kitchenObject.SetKitchenObjectParent(player);
		}
	}

	public Transform GetKitchenObjectFollowTransform() {
		return counterTopPoint;
	}

	public void SetKitchenObjectParent(KitchenObject kitchenObject) {
		this.kitchenObject = kitchenObject;
	}

	public KitchenObject GetKitchenObject() {
		return kitchenObject;
	}

	public void ClearKitchenObject() {
		kitchenObject = null;
	}

	public bool HasKitchenObject() {
		return kitchenObject != null;
	}
}
