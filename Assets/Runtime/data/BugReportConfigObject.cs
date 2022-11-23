//Created by Matt Purchase.
//  Copyright (c) 2022 Matt Purchase. All rights reserved.
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenuAttribute(menuName = "BugReporter/BugReportConfigObject")]
public class BugReportConfigObject : ScriptableObject {
	// Properties
	public string m_authentication;
	public string m_destination;
	public string m_notionVersion;
	public string m_gameTitle;
	public string m_currentBuild;
	// Initalisation Functions

	// Unity Callbacks

	// Public Functions

	// Private Functions

}