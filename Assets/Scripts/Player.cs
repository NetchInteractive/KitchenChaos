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

		transform.position += moveDir * Time.deltaTime * movementSpeed;
		transform.forward = Vector3.Slerp(transform.forward, moveDir, Time.deltaTime * rotationSpeed);
	}

	public bool IsWalking() {
		return isWalking;
	}
}
