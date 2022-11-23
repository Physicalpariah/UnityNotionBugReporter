//  Created by Matt Purchase.
//  Copyright (c) 2022 Matt Purchase. All rights reserved.
using System;
using System.Text;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Networking;

[Serializable]
public class BugReporterController {
	// Properties
	public ErrorLogGatherer m_errorLogGatherer;
	private const int m_maxTextLength = 999;
	private const int m_maxBlockCount = 99;
	public string m_versionNumber { get; private set; }
	public string m_buildNumber { get; private set; }
	public UnityWebRequest m_lastRequest { get; private set; }

	public BugReportConfigObject m_config;
	private string m_configPath = "data/bugReporter/bugReportConfig";

	private static BugReporterController m_instance;
	public static BugReporterController Instance {
		get {
			if (m_instance == null) {
				m_instance = new BugReporterController();
				m_instance.Initialise();
			}
			return m_instance;
		}
	}
	// Initalisation Functions


	private void Initialise() {
		m_errorLogGatherer = new ErrorLogGatherer();
		m_errorLogGatherer.Initialise();

		m_config = Resources.Load(m_configPath) as BugReportConfigObject;
		if (m_config == null) {
			Debug.LogException(new Exception("no bugreport config found, please create it in " + m_configPath));
		}
	}

	private void ShutDown() {
		Unsubscribe();
	}

	private void Unsubscribe() {

	}

	// Public Functions
	public void SetVersion(string version, string build) {
		m_versionNumber = version;
		m_buildNumber = build;
	}

	public string BuildJSON(string reportData, string playerData, string gameData, string gameTitle) {
		var parent = new parent(m_config.m_destination);
		string logs = m_errorLogGatherer.GetLogs();

		n_bugReportPriority reportPriority = GetPriority(logs);

		Name report = CreateName(reportData);

		SelectProp os = CreateSelect(SystemInfo.operatingSystem.ToString());
		SelectProp app = CreateSelect(m_versionNumber + "-" + m_buildNumber);
		SelectProp device = CreateSelect(Application.platform.ToString());
		SelectProp gameType = CreateSelect(gameTitle);
		SelectProp priority = CreateSelect(reportPriority.ToString());

		Block game = CreateBlock(SanitiseEmbeddedJson(gameData), "Game Data");
		Block player = CreateBlock(SanitiseEmbeddedJson(playerData), "Player Data");

		List<Block> blocks = new List<Block>();
		blocks.Add(game);
		blocks.Add(player);

		foreach (string log in m_errorLogGatherer.m_logs) {
			Block stack = CreateBlock(SanitiseLog(log), "Stack Trace");
			if (blocks.Count < m_maxBlockCount) {
				blocks.Add(stack);
			}
		}

		Properties prop = new Properties(report, os, device, app, gameType, priority);
		Root root = new Root(parent, prop, blocks);

		string json = JsonUtility.ToJson(root);
		return json.Replace("_object", "object");
	}

	public IEnumerator DoreportSend(string playerReport, string playerData, string gameData, string gameTitle) {
		var url = $"https://api.notion.com/v1/pages";

		string json = BuildJSON(playerReport, playerData, gameData, gameTitle);

		UnityWebRequest www = new UnityWebRequest(url, "POST");

		byte[] bodyRaw = Encoding.UTF8.GetBytes(json);
		www.uploadHandler = (UploadHandler)new UploadHandlerRaw(bodyRaw);
		www.downloadHandler = (DownloadHandler)new DownloadHandlerBuffer();

		www.SetRequestHeader("Authorization", m_config.m_authentication);
		www.SetRequestHeader("Notion-Version", m_config.m_notionVersion);
		www.SetRequestHeader("Content-Type", "application/json");

		Debug.Log("sending request!");
		Debug.Log(json);

		yield return www.SendWebRequest();

		if (www.result != UnityWebRequest.Result.Success) {
			Debug.Log(www.error);
			Debug.Log(www.result);
		}
		else {
			Debug.Log("Form upload complete!");
			Debug.Log(www.result);
		}

		m_lastRequest = www;
	}

	// Private Functions
	private n_bugReportPriority GetPriority(string logs) {
		n_bugReportPriority reportPriority = n_bugReportPriority.regular;
		if (logs.Contains("Exception")) {
			reportPriority = n_bugReportPriority.high;
		}

		return reportPriority;
	}

	private string SanitiseEmbeddedJson(string input) {
		string editedData = input.Replace("\"", "");

		if (editedData.Length > m_maxTextLength) {
			editedData = editedData.Substring(0, m_maxTextLength);
		}
		return editedData;
	}

	private string SanitiseLog(string input) {
		string editedData = input.Replace("\"", "");
		string editedData2 = editedData.Replace("#", "");

		if (editedData2.Length > m_maxTextLength) {
			editedData2 = editedData2.Substring(0, m_maxTextLength);
		}

		return editedData2;
	}

	private Block CreateBlock(string content, string heading) {
		var txt = new Text(content);
		var title = new PlainText(txt);
		return new Block(new List<PlainText>() { title }, heading);
	}

	private SelectProp CreateSelect(string content) {
		return new SelectProp(content);
	}

	private Name CreateName(string content) {
		var txt = new Text(content);
		var title = new Title(txt);
		var nam = new Name(new List<Title>() { title });
		return nam;
	}
}

public enum n_bugReportPriority {
	regular,
	high,
}
