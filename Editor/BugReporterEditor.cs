//Created by Matt Purchase.
//  Copyright (c) 2022 Matt Purchase. All rights reserved.
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEditor;
using UnityEngine;
using Unity.EditorCoroutines.Editor;

public class BugReporterEditor : EditorWindow {
	// Properties
	private string m_bugReportValue;
	private const int m_maxCharCount = 120;

	private string m_versionNumber;
	private string m_buildNumber;


	private static BugReporterEditor _window;
	private static BugReporterEditor s_window {
		get {
			if (_window == null) {
				_window = (BugReporterEditor)EditorWindow.GetWindow(typeof(BugReporterEditor));
				_window.minSize = Vector2.zero;
			}
			return _window;
		}
	}

	// Initalisation Functions
	[MenuItem("Window/Anchorite/Editor Bug Reporter")]
	static void Init() {
		BugReporterController ctrl = BugReporterController.Instance;
		_window = (BugReporterEditor)EditorWindow.GetWindow(typeof(BugReporterEditor));
	}

	// Unity Callbacks
	private void OnGUI() {
		if (s_window == null) {
			return;
		}
		DisplayInputField();
		DisplayButton();
	}




	// Public Functions

	// Private Functions
	private void DisplayButton() {
		if (string.IsNullOrWhiteSpace(m_bugReportValue)) {
			return;
		}

		if (GUILayout.Button("Send Report")) {
			SendReport();
		}
	}



	private void DisplayInputField() {
		EditorGUILayout.BeginHorizontal();
		GUILayout.Label("Report title:", GUILayout.Width(120f));
		m_bugReportValue = GUILayout.TextField(m_bugReportValue, m_maxCharCount);
		GUILayout.Label(m_bugReportValue.Length.ToString() + "/" + m_maxCharCount, GUILayout.Width(60f));
		EditorGUILayout.EndHorizontal();
	}

	private void SendReport() {
		EditorCoroutineUtility.StartCoroutine(DoRunReport(), this);
	}

	private IEnumerator DoRunReport() {
		yield return BugReporterController.Instance.DoreportSend(m_bugReportValue, "data", "gamedata");
		m_bugReportValue = "";
		BugReporterController.Instance.m_errorLogGatherer.ClearLogs();
	}

}