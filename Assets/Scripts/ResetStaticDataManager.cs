using UnityEngine;

// Reset any static data that might persist through scene changes
// This script should only exist in the main menu

// For example the static event from the CuttingCounter script
// The listeners from this event aren't cleared automatically:
//	public static event EventHandler OnAnyCut;
public class ResetStaticDataManager : MonoBehaviour {
	private void Awake() {
		CuttingCounter.ResetStaticData();
		TrashCounter.ResetStaticData();
		BaseCounter.ResetStaticData();
	}
}
