using UnityEngine;

public interface IKitchenObjectParent {
	public Transform GetKitchenObjectFollowTransform();

	public void SetKitchenObjectParent(KitchenObject kitchenObject);

	public KitchenObject GetKitchenObject();

	public void ClearKitchenObject();

	public bool HasKitchenObject();
}
