using UnityEditor;
using UnityEngine;
using UnityEditor.ShortcutManagement;
using System;
using System.Reflection;

// Editor script to add a shortcut for switching between tabs in the Editor using Ctrl + Tab like Google Chrome
// For some reason the default "Main Menu / Window / Next Window" shortcut doesn't work
public static class CtrlTabSwitcher {
	[Shortcut("Window/Switch Tab Forward", KeyCode.Tab, ShortcutModifiers.Action)]
	private static void SwitchTabForward() {
		SwitchTab(1);
	}

	[Shortcut("Window/Switch Tab Backward", KeyCode.Tab, ShortcutModifiers.Action | ShortcutModifiers.Shift)]
	private static void SwitchTabBackward() {
		SwitchTab(-1);
	}

	private static void SwitchTab(int direction) {
		EditorWindow focused = EditorWindow.focusedWindow;
		if (focused == null) return;

		var parentField = typeof(EditorWindow)
			.GetField("m_Parent", BindingFlags.NonPublic | BindingFlags.Instance);

		if (parentField == null) return;

		var dockArea = parentField.GetValue(focused);
		if (dockArea == null) return;

		var panesField = dockArea.GetType().GetField("m_Panes", BindingFlags.NonPublic | BindingFlags.Instance);

		var panes = panesField?.GetValue(dockArea) as System.Collections.IList;
		if (panes == null || panes.Count < 2) return;

		int currentIndex = -1;
		for (int i = 0; i < panes.Count; i++) {
			if (panes[i] == focused) {
				currentIndex = i;
				break;
			}
		}

		if (currentIndex == -1) return;

		int nextIndex = (currentIndex + direction + panes.Count) % panes.Count;

		var nextWindow = panes[nextIndex] as EditorWindow;
		nextWindow?.Focus();
	}
}