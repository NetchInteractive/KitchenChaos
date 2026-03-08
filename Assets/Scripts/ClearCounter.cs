using UnityEngine;

public class ClearCounter : MonoBehaviour {
	// TODO: Implement an IInteractable interface

	[SerializeField] private Transform counterTopPoint;
	[SerializeField] private KitchenObjectSO tomatoSO;
	public void Interact() {
		Transform tomato = Instantiate(tomatoSO.prefab, counterTopPoint);
		tomato.transform.localPosition = Vector3.zero;

		KitchenObjectSO so = tomato.transform.GetComponent<KitchenObject>().GetKitchenObjectSO();
		Debug.Log(so.objectName);
	}
}
