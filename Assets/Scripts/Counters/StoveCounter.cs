using System;
using UnityEngine;

public class StoveCounter : BaseCounter, IHasProgress {
	public event EventHandler<IHasProgress.OnProgressChangedEventArgs> OnProgressChanged;

	public event EventHandler<OnStateChangedEventArgs> OnStateChanged;
	public class OnStateChangedEventArgs : EventArgs {
		public State state;	
	}

	public enum State {
		Idle,
		Frying,
		Fried,
		Burned
	}

	[SerializeField] private FryingRecipeSO[] fryingRecipeSOArray;
	[SerializeField] private BurningRecipeSO[] burningRecipeSOArray;

	private State currentState;
	private FryingRecipeSO fryingRecipeSO;
	private float fryingTimer;
	private float burningTimer;
	private BurningRecipeSO burningRecipeSO;

	private void Start() {
		ChangeState(State.Idle);
	}

	private void Update() {
		if (!HasKitchenObject()) return;

		switch (currentState) {
			case State.Idle:

				break;

			case State.Frying:
				fryingTimer += Time.deltaTime;

				OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs() {
					progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
				});

				if (fryingTimer > fryingRecipeSO.fryingTimerMax) {
					GetKitchenObject().DestroySelf();
					KitchenObject.SpawnKitchenObject(fryingRecipeSO.output, this);
					
					ChangeState(State.Fried);
					burningTimer = 0;
					burningRecipeSO = GetBurningRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
				}
				break;

			case State.Fried:
				burningTimer += Time.deltaTime;

				OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs() {
					progressNormalized = burningTimer / burningRecipeSO.burningTimerMax
				});

				if (burningTimer > burningRecipeSO.burningTimerMax) {
					GetKitchenObject().DestroySelf();
					KitchenObject.SpawnKitchenObject(burningRecipeSO.output, this);
					ChangeState(State.Burned);
					burningTimer = 0;
				}
				break;

			case State.Burned:
				OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs() {
					progressNormalized = 0f
				});
				break;
		}
	}

	public override void Interact(Player player) {
		if (HasKitchenObject()) {
			if (!player.HasKitchenObject()) {
				GetKitchenObject().SetKitchenObjectParent(player);
				ChangeState(State.Idle);

				OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs() {
					progressNormalized = 0f
				});
			}
		} else { // The counter is empty
			if (player.HasKitchenObject() && HasRecipeWithInput(player.GetKitchenObject().GetKitchenObjectSO())) {
				player.GetKitchenObject().SetKitchenObjectParent(this);
				fryingRecipeSO = GetFryingRecipeSOWithInput(GetKitchenObject().GetKitchenObjectSO());
				ChangeState(State.Frying);
				fryingTimer = 0f;

				OnProgressChanged?.Invoke(this, new IHasProgress.OnProgressChangedEventArgs() {
					progressNormalized = fryingTimer / fryingRecipeSO.fryingTimerMax
				});
			}
		}
	}

	private void ChangeState(State state) {
		currentState = state;
		OnStateChanged?.Invoke(this, new OnStateChangedEventArgs {
			state = state
		});
	}

	private bool HasRecipeWithInput(KitchenObjectSO inputKitchenObjectSO) {
		FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
		return fryingRecipeSO != null;
	}

	private KitchenObjectSO GetOutputForInput(KitchenObjectSO inputKitchenObjectSO) {
		FryingRecipeSO fryingRecipeSO = GetFryingRecipeSOWithInput(inputKitchenObjectSO);
		if (fryingRecipeSO != null) return fryingRecipeSO.output;

		return null;
	}

	private FryingRecipeSO GetFryingRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
		foreach (FryingRecipeSO fryingRecipeSO in fryingRecipeSOArray) {
			if (fryingRecipeSO.input == inputKitchenObjectSO) {
				return fryingRecipeSO;
			}
		}
		return null;
	}

	private BurningRecipeSO GetBurningRecipeSOWithInput(KitchenObjectSO inputKitchenObjectSO) {
		foreach (BurningRecipeSO burningRecipeSO in burningRecipeSOArray) {
			if (burningRecipeSO.input == inputKitchenObjectSO) {
				return burningRecipeSO;
			}
		}
		return null;
	}
}
