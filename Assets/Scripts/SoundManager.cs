using UnityEngine;
using UnityEngine.UIElements;

public class SoundManager : MonoBehaviour {
	public static SoundManager Instance { get; private set; }

	private const string PLAYER_PREFS_SOUND_EFFECTS_VOLUME = "SoundEffectsVolume";

	[SerializeField] private AudioClipRefsSO audioClipRefsSO;

	private float volume = 1f;

	private void Awake() {
		Instance = this;
		volume = PlayerPrefs.GetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, 1f);
	}

	private void Start() {
		DeliveryManager.Instance.OnDeliverSuccess += DeliveryManager_OnDeliverSuccess;
		DeliveryManager.Instance.OnDeliverFail += DeliveryManager_OnDeliverFail;

		Player.Instance.OnPickedUpSomething += Player_OnPickedUpSomething;

		// Static events
		CuttingCounter.OnAnyCut += CuttingCounter_OnAnyCut;
		BaseCounter.OnAnyObjectPlacedHere += BaseCounter_OnAnyObjectPlacedHere;
		TrashCounter.OnAnyObjectTrashed += TrashCounter_OnAnyObjectTrashed;
	}

	private void TrashCounter_OnAnyObjectTrashed(object sender, System.EventArgs e) {
		TrashCounter trashCounter = sender as TrashCounter;
		PlaySound(audioClipRefsSO.trash, trashCounter.transform.position);
	}

	private void BaseCounter_OnAnyObjectPlacedHere(object sender, System.EventArgs e) {
		BaseCounter baseCounter = sender as BaseCounter;
		PlaySound(audioClipRefsSO.objectDrop, baseCounter.transform.position);
	}

	private void Player_OnPickedUpSomething(object sender, System.EventArgs e) {
		PlaySound(audioClipRefsSO.objectPickup, Player.Instance.transform.position);
	}

	private void CuttingCounter_OnAnyCut(object sender, System.EventArgs e) {
		CuttingCounter cuttingCounter = sender as CuttingCounter;
		PlaySound(audioClipRefsSO.chop, cuttingCounter.transform.position);
	}

	private void DeliveryManager_OnDeliverSuccess(object sender, System.EventArgs e) {
		PlaySound(audioClipRefsSO.deliverySuccess, DeliveryCounter.Instance.transform.position);
	}

	private void DeliveryManager_OnDeliverFail(object sender, System.EventArgs e) {
		PlaySound(audioClipRefsSO.deliveryFail, DeliveryCounter.Instance.transform.position);
	}

	private void PlaySound(AudioClip audioClip, Vector3 position, float volumeMultiplier = 1f) {
		AudioSource.PlayClipAtPoint(audioClip, position, volume * volumeMultiplier);
	}

	private void PlaySound(AudioClip[] audioClipArray, Vector3 position, float volumeMultiplier = 1f) {
		AudioSource.PlayClipAtPoint(audioClipArray[Random.Range(0, audioClipArray.Length)], position, volume * volumeMultiplier);
	}

	public void PlayFootstepsSound(Vector3 position, float volumeMultiplier) {
		PlaySound(audioClipRefsSO.footsteps, position, volume * volumeMultiplier);
	}

	public void ChangeVolume() {
		volume += .1f;
		if (volume > 1f) volume = 0f;

		PlayerPrefs.SetFloat(PLAYER_PREFS_SOUND_EFFECTS_VOLUME, volume);
		PlayerPrefs.Save();
	}

	public float GetVolume() {
		return volume;
	}
}
