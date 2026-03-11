using System;
using System.Collections.Generic;
using UnityEngine;

public class PlateCompleteVisual : MonoBehaviour {
	[Serializable]
	public struct KitchenObjectSO_GameObject {
		public KitchenObjectSO kitchenObjectSO;
		public GameObject gameObject;
	}

	[SerializeField] private PlateKitchenObject plateKitchenObject;
	[SerializeField] private List<KitchenObjectSO_GameObject> kitchenObjectSOGameObjectList;

	private void Start() {
		foreach (var item in kitchenObjectSOGameObjectList) {
			item.gameObject.SetActive(false);
		}

		plateKitchenObject.OnIngredientAdded += PlateKitchenObject_OnIngredientAdded;
	}

	private void PlateKitchenObject_OnIngredientAdded(object sender, PlateKitchenObject.OnIngredientAddedEventArgs e) {
		foreach (var item in kitchenObjectSOGameObjectList) {
			if (item.kitchenObjectSO == e.kitchenObjectSO) {
				item.gameObject.SetActive(true);
				break;
			}
		}
	}
}
