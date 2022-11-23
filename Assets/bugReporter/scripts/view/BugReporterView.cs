//  Created by Matt Purchase.
//  Copyright (c) 2021 Matt Purchase. All rights reserved.
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class BugReporterView : MonoBehaviour {
	// Properties
	[SerializeField] private string m_playerReportData;
	[SerializeField] private string m_testPlayerData;
	[SerializeField] private string m_testGameData;

	[SerializeField] private TMP_InputField m_inputPlayerReport;
	[SerializeField] private Button m_btnSubmit;

	private Coroutine c_reportRoutine;
	// Initalisation Functions

	// Unity Callbacks
	public void OnEnable() {
		BugReporterController ctrl = BugReporterController.Instance;

		m_inputPlayerReport.onEndEdit.RemoveAllListeners();
		m_inputPlayerReport.onEndEdit.AddListener(SetReportData);

		m_btnSubmit.onClick.RemoveAllListeners();
		m_btnSubmit.onClick.AddListener(SendReport);
	}

	public void ShutDown() {
		Unsubscribe();
	}

	private void Unsubscribe() {

	}

	// Public Functions
	public void SetReportData(string report) {
		m_playerReportData = report;
	}

	public void SendReport() {
		if (string.IsNullOrWhiteSpace(m_playerReportData)) {
			Debug.Log("No report found");
			return;
		}

		if (c_reportRoutine != null) {
			StopCoroutine(c_reportRoutine);
			c_reportRoutine = null;
		}

		c_reportRoutine = StartCoroutine(BugReporterController.Instance.DoreportSend(m_playerReportData, m_testPlayerData, m_testGameData, "charge"));
	}

	// Private Functions

}