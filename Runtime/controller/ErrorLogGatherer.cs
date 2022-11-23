//Created by Matt Purchase.
//  Copyright (c) 2022 Matt Purchase. All rights reserved.
using System.Collections.Generic;
using System;
using UnityEngine;

[Serializable]
public class ErrorLogGatherer {
	// Properties

	public List<string> m_logs { get; private set; }

	// Initalisation Functions

	public void Initialise() {
		m_logs = new List<string>();
		Application.logMessageReceived += GatherErrorLog;
	}

	// Unity Callbacks

	// Public Functions

	public void ClearLogs() {
		m_logs.Clear();
	}

	public string GetLogs() {
		string logs = "";

		foreach (string log in m_logs) {
			logs += log + "__";
		}

		return logs;
	}

	// Private Functions
	private void GatherErrorLog(string logString, string stackTrace, LogType type) {
		if (type == LogType.Error || type == LogType.Exception) {

			string[] shortTraceArray = stackTrace.Split('/');
			string shortTrace = "";

			foreach (string item in shortTraceArray) {
				if (item.Contains(".cs")) {
					shortTrace += item;
				}
			}

			string log = logString + "_" + shortTrace;
			m_logs.Add(log);
		}
	}

}