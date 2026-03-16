using UnityEditor;
using UnityEngine;
using TMPro;

/// <summary>
/// Listens for new GameObjects added to the scene and, if they carry a
/// TextMeshProUGUI component, applies the studio defaults:
///   • RectTransform width  = 0
///   • RectTransform height = 0
///   • Text wrapping mode   = NoWrap
/// </summary>
[InitializeOnLoad]
public static class TextMeshProUGUIDefaults {
	private const float DefaultWidth = 0f;
	private const float DefaultHeight = 0f;
	private const TextWrappingModes DefaultWrapping = TextWrappingModes.NoWrap;
	private const bool enableDebugLogs = true; 

	static TextMeshProUGUIDefaults() {
		ObjectChangeEvents.changesPublished += OnChangesPublished;
	}

	private static void OnChangesPublished(ref ObjectChangeEventStream stream) {
		for (int i = 0; i < stream.length; i++) {
			if (stream.GetEventType(i) != ObjectChangeKind.CreateGameObjectHierarchy) continue;

			stream.GetCreateGameObjectHierarchyEvent(i, out var data);

			GameObject go = EditorUtility.EntityIdToObject(data.instanceId) as GameObject;
			if (go == null) continue;

			foreach (TextMeshProUGUI tmp in go.GetComponentsInChildren<TextMeshProUGUI>(true)) {
				ApplyDefaults(tmp);
			}
		}
	}

	private static void ApplyDefaults(TextMeshProUGUI tmp) {
		// Record for full Undo support (Ctrl+Z works correctly).
		Undo.RecordObject(tmp.rectTransform, "Set TMP RectTransform Defaults");
		Undo.RecordObject(tmp, "Set TMP Wrapping Default");

		Vector2 size = tmp.rectTransform.sizeDelta;

		if (size.x == DefaultWidth && size.y == DefaultHeight && tmp.textWrappingMode == DefaultWrapping) return;

		size.x = DefaultWidth;
		size.y = DefaultHeight;
		tmp.rectTransform.sizeDelta = size;

		tmp.textWrappingMode = DefaultWrapping;

		EditorUtility.SetDirty(tmp.rectTransform);
		EditorUtility.SetDirty(tmp);

		if (enableDebugLogs) Debug.Log($"Set default TMP settings - Width: {DefaultWidth} | Height: {DefaultHeight} | Wrapping: {DefaultWrapping}", tmp);
	}
}