using System;
using UnityEngine;

public class Player : MonoBehaviour {
	public static Player Instance { get; private set; }

	public event EventHandler<OnSelectedCounterChangedEventArgs> OnSelectedCounterChanged;
	public class OnSelectedCounterChangedEventArgs : EventArgs {
		public ClearCounter selectedCounter;
	}

	[SerializeField] private GameInput gameInput;
	[SerializeField] private LayerMask countersLayerMask;

	[SerializeField] private float movementSpeed = 5f;
	[SerializeField] private float rotationSpeed = 5f;

	private bool isWalking;
	private Vector3 lastInteractDirection;
	private ClearCounter selectedCounter;

	private void Awake() {
		if (Instance != null) {
			Debug.LogError("There is more than one Player instance! " + transform + " - " + Instance);
		}

		Instance = this;
	}

	private void Start() {
		gameInput.OnInteractAction += GameInput_OnInteractAction;
	}

	private void GameInput_OnInteractAction(object sender, System.EventArgs e) {
		if (selectedCounter != null) {
			selectedCounter.Interact();
		}
	}

	private void Update() {
		HandleMovement();
		HandleInteractions();
	}

	private void HandleInteractions() {
		Vector2 inputVector = gameInput.GetMovementVectorNormalized();
		Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

		if (moveDir != Vector3.zero) {
			lastInteractDirection = moveDir;
		}

		float interactDistance = 2f;
		if (Physics.Raycast(transform.position, lastInteractDirection, out RaycastHit raycastHit, interactDistance)) {
			if (raycastHit.transform.TryGetComponent(out ClearCounter clearCounter)) {
				if (clearCounter != selectedCounter) {
					SetSelectedCounter(clearCounter);
				}
			} else {
				SetSelectedCounter(null);
			}
		} else {
			SetSelectedCounter(null);
		}
	}

	private void SetSelectedCounter(ClearCounter clearCounter) {
		selectedCounter = clearCounter;
		OnSelectedCounterChanged?.Invoke(this, new OnSelectedCounterChangedEventArgs { selectedCounter = selectedCounter });
	}

	private void HandleMovement() {
		Vector2 inputVector = gameInput.GetMovementVectorNormalized();
		Vector3 moveDir = new Vector3(inputVector.x, 0, inputVector.y);

		isWalking = moveDir != Vector3.zero;

		float playerRadius = 0.7f;
		float playerHeight = 2f;
		float moveDistance = movementSpeed * Time.deltaTime;
		bool canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDir, moveDistance);

		if (!canMove) {
			// Attempt x movement
			Vector3 moveDirX = new Vector3(moveDir.x, 0, 0).normalized;
			canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirX, moveDistance);
			if (canMove) {
				moveDir = moveDirX;
			} else {
				// Attempt z movement
				Vector3 moveDirZ = new Vector3(0, 0, moveDir.z).normalized;
				canMove = !Physics.CapsuleCast(transform.position, transform.position + Vector3.up * playerHeight, playerRadius, moveDirZ, moveDistance);

				if (canMove) {
					moveDir = moveDirZ;
				}
			}
		}

		if (canMove) {
			transform.position += moveDir * moveDistance;
		}

		// Rotation
		transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
	}

	public bool IsWalking() {
		return isWalking;
	}
}
