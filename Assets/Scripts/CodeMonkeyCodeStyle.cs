using System;
using UnityEngine;

public class CodeMonkeyCodeStyle : MonoBehaviour {
    // Constants: UpperCase SnakeCase
    public const int CONSTANT_VALUE = 10;

	// Properties: PascalCase
    public static CodeMonkeyCodeStyle Instance { get; private set; }

	// Events: PascalCase
    public event EventHandler OnSomethingHappened;

	// Fields: camelCase
    private float memberVariable;

	// Function Names: PascalCase
	private void Awake() {
		Instance = this;

		DoSomething(10f);
	}

	// Function params: camelCase
	private void DoSomething(float someParameter) {
		memberVariable = someParameter;
		
		// Local variables: camelCase
		float localVariable = memberVariable * 2f;
		Debug.Log("Local Variable: " + localVariable);
	
		OnSomethingHappened?.Invoke(this, EventArgs.Empty);
	}
}
