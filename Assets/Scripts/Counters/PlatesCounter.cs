using System;
using UnityEngine;

public class PlatesCounter : BaseCounter {
	public event EventHandler OnPlateSpawned;
	public event EventHandler OnPlateRemoved;

	[SerializeField] private KitchenObjectSO plateKitchenObjectSO;

	private float spawnPlateTimer;
	private float spawnPlateTimerMax = 4f;

	private int platesSpawnedAmount;
	private int platesSpawnedAmountMax = 4;

	private void Update() {
		spawnPlateTimer += Time.deltaTime;
		if (spawnPlateTimer > spawnPlateTimerMax) {
			spawnPlateTimer = 0f;
			if (platesSpawnedAmount < spawnPlateTimerMax) {
				platesSpawnedAmount++;
				OnPlateSpawned?.Invoke(this, EventArgs.Empty);
			}
		}
	}

	public override void Interact(Player player) {
		if (!player.HasKitchenObject() && platesSpawnedAmount > 0) {
			platesSpawnedAmount--;
			OnPlateRemoved?.Invoke(this, EventArgs.Empty);

			KitchenObject.SpawnKitchenObject(plateKitchenObjectSO, player);
		}
	}
}
