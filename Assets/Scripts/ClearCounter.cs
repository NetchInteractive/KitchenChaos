using UnityEngine;

// TODO: Implement an IInteractable interface
public class ClearCounter : MonoBehaviour {
	[SerializeField] private Transform counterTopPoint;
	[SerializeField] private KitchenObjectSO tomatoSO;

	private KitchenObject kitchenObject;
	public void Interact() {
		if (kitchenObject == null) {
			Transform tomato = Instantiate(tomatoSO.prefab, counterTopPoint);
			tomato.GetComponent<KitchenObject>().SetClearCounter(this);
		}
	}

	public Transform GetCounterTopPoint() {
		return counterTopPoint;
	}

	public void SetKitchenObject(KitchenObject kitchenObject) {
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
