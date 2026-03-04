using UnityEngine;

public class Player : MonoBehaviour {
	[SerializeField] private GameInput gameInput;
	[SerializeField] private float movementSpeed = 5f;
	[SerializeField] private float rotationSpeed = 5f;

	private bool isWalking;

	private void Update() {
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
